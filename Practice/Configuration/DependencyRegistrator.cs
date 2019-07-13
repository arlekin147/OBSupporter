using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Observers;
using Practice.Factory;

namespace Practice.Configuration
{
    public static class DependencyRegistrator
    {
        static internal IOBSStatus OBSStatus { get => new OBSStatus();}
        static internal IActionCreator ActionCreator { get => new ActionCreator(); }
        static internal ITimeObserver TimeObserver { get => new PipeObserver(); }
        
    }
}
