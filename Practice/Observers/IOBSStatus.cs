using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Observers
{
    public interface IOBSStatus : IDisposable
    {
        double Time { get; }
    }
}
