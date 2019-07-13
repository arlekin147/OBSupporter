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
        public event TimeUpdateDelegate UpdateTime;
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
            var pipeClient =
                    new NamedPipeClientStream(".", "ObsPluginForRecordStateReportingPipe",
                        PipeDirection.InOut, PipeOptions.None,
                        TokenImpersonationLevel.Impersonation);
            pipeClient.Connect();
            Console.WriteLine("Connected!");
            StreamString ss = new StreamString(pipeClient);
            while (pipeClient.IsConnected)
            {
                Console.WriteLine("ogo");
                var time = encoder.GetString(ss.ReadString());
                var endOfTime = 0;
                foreach(var s in time) if(char.IsDigit(s)) ++endOfTime;
                Console.WriteLine("time: " + time.Substring(0, endOfTime));
            }
        }

        public void Dispose()
        {
            this.conectionThread.Interrupt();
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

        public byte[] ReadString()
        {
            int len;
            len = 2 * 256;
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

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
