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
        private double currentTime = 0;

        public OBSStatus()
        {
            Console.WriteLine("obs status start");
            this.observer.StartObserve();
            this.observer.UpdateTime += this.Updatetime;
        }
        public double Time
        {
            get
            {
                return currentTime;
            }
        }

        public void Updatetime(double newTime)
        {
            Console.WriteLine("Time Updated");
            this.currentTime = newTime;
        }

        public void Dispose()
        {
            this.observer.Dispose();
        }
    }
}
