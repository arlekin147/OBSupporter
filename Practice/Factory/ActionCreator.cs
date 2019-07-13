using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Practice.Factory
{
    public class ActionCreator : IActionCreator
    {
        public ContentControl Origin { private get; set; }

        public WriteActionDelegate ActionHasHappened { get; set; }

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
            button.Click += (object sender, RoutedEventArgs e) => { this.ActionHasHappened(this.Origin.Content + " " + action); };
            panel.Children.Add(button);
            return panel;
        }
    }
}
