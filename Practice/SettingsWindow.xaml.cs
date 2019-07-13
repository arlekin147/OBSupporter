using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Practice
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Dictionary<string, string> actions;
        public bool Changes { get; set; }

        public SettingsWindow(ref Dictionary<string, string> actions)
        {
            InitializeComponent();
            this.Changes = false;
            this.actions = actions;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void AddActionClicked(object sender, RoutedEventArgs e)
        {
            var addActionWindow = new AddActionWindow();

            if (addActionWindow.ShowDialog().Value && addActionWindow.DialogResult.Value)
            {
                if (!this.actions.ContainsKey(addActionWindow.Shortcut))
                {
                    this.actions.Add(addActionWindow.Shortcut, addActionWindow.Action);
                    this.Changes = true;
                }
                else
                {
                    MessageBox.Show("You can't have actions with the same shorcuts",
                                          "Warning");
                }
            }
        }

        private void CloseSettings(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void AcceptSettings(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void DeclineSettings(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Changes = false;
        }
    }
}
