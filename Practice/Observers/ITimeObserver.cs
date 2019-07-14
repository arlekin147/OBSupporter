using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Observers
{

    public delegate void TimeUpdateDelegate(double time);
    public interface ITimeObserver : IDisposable
    {
        event TimeUpdateDelegate UpdateTime;
        void StartObserve();

        bool Status { get; }
    }
}
