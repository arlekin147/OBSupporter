using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Practice.Factory
{
    public class ActionCreator : IActionCreator
    {
        public WrapPanel CreateAction(string shortcut, string action, int width)
        {
            var panel = new WrapPanel();
            var label = new Label()
            {
                Content = shortcut,
                Width = width/3,
            };
            panel.Children.Add(label);
            var button = new Button()
            {
                Content = action,
                Width = (width / 3) * 2,
            };
            panel.Children.Add(button);
            return panel;
        }
    }
}
