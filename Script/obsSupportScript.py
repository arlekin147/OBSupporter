import obspython as obs
import time
import win32pipe, win32file, pywintypes
import threading

#Just a way to create constant in that goddamned language
def pipe_name():
    return 'ObsPluginForRecordStateReportingPipe'


mutex_state_sending = threading.Lock()  #Guards pipe_send_state() critical section
                                        #pipe_send_state() creates seek_for_client() thread on fail,
                                        #so running two copies of pipe_send_state() can result in two threads,
                                        #end, since seek_for_client() writes non-zero state on connection,
                                        #multiwrite of same data, which is not the only problem, but is very bad itself

state = 0   #Can be edited only under mutex_state_sending_mutex,
            #because can be edited from different threads.
            #Also pipe_send_state() requires that variable unchanged while running

have_client_to_speak_with = False   #Can be edited only under mutex_state_sending_mutex,
                                    #because can be edited from different threads.
                                    #Also pipe_send_state() requires that variable unchanged while running

pipe = None #Immutable object, initializes at script_load just once

mutex_logger = threading.Lock() #We can't IO in a stream concurrently without lock, so...

def log_threadsafe(level, prompt):
    """For those, who are too lazy/have bad memory to lock mutex first"""
    with mutex_logger:
        obs.script_log(level, prompt)

def init_client_seeker():
    """
    Creates thread, which will await new client and tell the client current (if non-zero)
    state on connection.
    
    "daemon = True" means "die if all other threads are dead"
    
    !! Should be called only on main thread with have_client_to_speak_with set to False first.
       Otherwise can result in multiple threads, each one writing state to new client, which is
       not the only problem, but is very bad itself
    """
    client_seeker = threading.Thread(target=seek_for_client)
    client_seeker.daemon = True
    client_seeker.start()

def seek_for_client():
    """
    Disconnects old (dead) client and wait for a new one.
    
    On connection sends curren (if non-zero) state to the new client.
    Calls pipe_send_state(), reads state and modyfies have_client_to_speak_with.
    That's why most of operations on that variables/function must be guarded with mutex_state_sending.
    See comments above for details.
    """
    global have_client_to_speak_with
    
    win32pipe.DisconnectNamedPipe(pipe) #in case there were connection to dead client
    log_threadsafe(obs.LOG_DEBUG, 'Waiting for a new client')
    win32pipe.ConnectNamedPipe(pipe, None) #seek for new client
    log_threadsafe(obs.LOG_DEBUG, 'A client connected')
    with mutex_state_sending:
        have_client_to_speak_with = True
        if state != 0:
            log_threadsafe(obs.LOG_DEBUG, 'Sending non-zero state sending to newcomer: %d' % state)
            pipe_send_state()
        else:
            log_threadsafe(obs.LOG_DEBUG, 'No need in sharing state with newcomer, already 0')

def pipe_send_state(should_seek_new_client_on_fail=True):
    """
    Changes have_client_to_speak_with, reads state (which must not be changed while
    the function is running) and calls init_client_seeker().
    
    !! Is called form another thread. Before call u must lock mutex_state_sending.
    """
    global have_client_to_speak_with
    
    if have_client_to_speak_with:
        log_threadsafe(obs.LOG_DEBUG, 'Sending %d to pipe' % state)
        try:
            win32file.WriteFile(pipe, str(state).encode('utf-8'))
        except pywintypes.error: #Assume every error is due to client disconnected, sorry)
            log_threadsafe(obs.LOG_DEBUG, 'Attemption failed. No client, possibly')
            have_client_to_speak_with = False
            if should_seek_new_client_on_fail:
                init_client_seeker()
    else:
        log_threadsafe(obs.LOG_DEBUG, 'Can\'t send %d to pipe: no client' % state)

#OBS Triggers
def trigger_recording_started(_):
    """Updates state and sends it"""
    log_threadsafe(obs.LOG_DEBUG, 'Recording started')
    
    global state
    with mutex_state_sending:
        state = int(time.time())
        pipe_send_state()
    
def trigger_recording_stopped(_):
    """Updates state and sends it if changed"""
    log_threadsafe(obs.LOG_DEBUG, 'Recording stopped')
    
    global state
    if state != 0: #for the case if script was loaded when obs had been already recording smth
        with mutex_state_sending:
            state = 0
            pipe_send_state()

# OBS API Hooks Start Below
def script_description():
    return 'TBD'

def script_load(settings):
    """
    * Sets up triggers on obs events (such as start recording and stop recording)
    * Creates pipe. Pipe is immutable handle, which never changes after initialization
    * Calls init_client_seeker() to find a client 
    """
    log_threadsafe(obs.LOG_DEBUG, 'Plugin loaded')
    
    recording_signal_handler = obs.obs_output_get_signal_handler(obs.obs_frontend_get_recording_output())
    obs.signal_handler_connect(recording_signal_handler, "start", trigger_recording_started)
    obs.signal_handler_connect(recording_signal_handler, "stop", trigger_recording_stopped)
    
    global pipe
    pipe = win32pipe.CreateNamedPipe(
       r'\\.\pipe\%s' % pipe_name(),
       win32pipe.PIPE_ACCESS_DUPLEX,
       win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_READMODE_MESSAGE | win32pipe.PIPE_WAIT,
       1, 65536, 65536,
       0,
       None)
       
    #have_client_to_speak_with = False #It is the default value, but it must be intact
    init_client_seeker()
    
def script_unload():
    """
    * If state is non-zero, sends termination signal to client, since we can't do it latter
    * Closes handle of pipe, which is good manner, even if windows can fix it itself
    * No need to kill seeker-thread, even it's exist, because it's daemon, and will die following us
    """
    log_threadsafe(obs.LOG_DEBUG, 'Plugin unloaded')
    
    global state
    
    if state != 0:
        with mutex_state_sending:
            state = 0
            pipe_send_state(should_seek_new_client_on_fail=False)
        
    win32file.CloseHandle(pipe)
    
    