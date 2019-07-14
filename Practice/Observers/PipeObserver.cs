using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice.Observers
{
    class PipeObserver : ITimeObserver
    {
        private TimeUpdateDelegate timeUpdateDelegate = null;
        private bool interrupt = false;
        private NamedPipeClientStream pipeClient;
        public bool Status { get => this.pipeClient.IsConnected; }
        public event TimeUpdateDelegate UpdateTime
        {
            add
            {
                Console.WriteLine("Registred!");
                this.timeUpdateDelegate += value;
            }
            remove
            {
                this.timeUpdateDelegate -= value;
            }
        }
        private Thread conectionThread;
        private Encoding encoder = Encoding.UTF8;
        public void StartObserve()
        {
            Console.WriteLine("start observe");
            this.conectionThread = new Thread(Observe);
            this.conectionThread.Start();
        }


        public void Observe()
        {
            this.pipeClient =
                    new NamedPipeClientStream(".", "ObsPluginForRecordStateReportingPipe",
                        PipeDirection.InOut, PipeOptions.None,
                        TokenImpersonationLevel.Impersonation);
            while (!this.interrupt) {
                pipeClient.Connect();
                Console.WriteLine("Connected!");
                StreamString ss = new StreamString(pipeClient);
                while (pipeClient.IsConnected)
                {
                    string time;
                    Console.WriteLine("ogo");
                    try
                    {
                        time = encoder.GetString(ss.ReadString().Result);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                    var endOfTime = 0;
                    foreach (var s in time) if (char.IsDigit(s)) ++endOfTime;
                    Console.WriteLine("time: " + time.Substring(0, endOfTime));
                    try {
                        this.timeUpdateDelegate(Double.Parse(time.Substring(0, endOfTime)));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            pipeClient.Dispose();
            Console.WriteLine("Interrupted");
        }

        public void Dispose()
        {
            Console.WriteLine("Closed!");
            this.interrupt = true;
            this.pipeClient.Close();
            this.pipeClient.Dispose();
            this.conectionThread.Interrupt();
            this.conectionThread.Abort();
        }
    }


    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public async Task<byte[]> ReadString()
        {
            int len;
            len = 2 * 256;
            byte[] inBuffer = new byte[len];
            await ioStream.ReadAsync(inBuffer, 0, len);
            return inBuffer;
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }
    }
}
