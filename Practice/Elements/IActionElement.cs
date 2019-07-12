using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Practice.Elements
{
    public interface IActionElement
    {
        Label Label { get; set; }
        Button Button { get; set; }
    }
}
