using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Configuration;

namespace Practice.Observers
{
    public class OBSStatus : IOBSStatus
    {
        private readonly Random random = new Random();
        private readonly ITimeObserver observer = DependencyRegistrator.TimeObserver;

        public OBSStatus()
        {
            Console.WriteLine("obs status start");
            this.observer.StartObserve();
        }
        public double Time
        {
            get
            {
                return random.Next();
            }
        }

        public void Dispose()
        {
            this.observer.Dispose();
        }
    }
}
