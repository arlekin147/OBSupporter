using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Practice.Factory
{
    public delegate void WriteActionDelegate(string action);
    interface IActionCreator
    {
        WrapPanel CreateAction(string shortcut, string action, int width);
        WriteActionDelegate ActionHasHappened { get; set; }
        ContentControl Origin { set; }
    }
}
