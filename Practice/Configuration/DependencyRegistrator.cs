using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Observers;
using Practice.Elements;

namespace Practice.Configuration
{
    public static class DependencyRegistrator
    {
        static internal IOBSStatus OBSStatus { get => new OBSStatus();}
        static internal IActionElement EmptyActionElement { get => new ActionElement(); }
    }
}
