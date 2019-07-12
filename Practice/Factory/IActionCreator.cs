using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Practice.Factory
{
    interface IActionCreator
    {
        WrapPanel CreateAction(string shortcut, string action, int width);
    }
}
