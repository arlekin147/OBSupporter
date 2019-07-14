using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;

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

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var removeActionWindow = new RemoveActionWindow(ref actions);
            if (removeActionWindow.ShowDialog().Value)
            {
                this.Changes = true;
            }
        }

        private void ChooseActionSetButtonClecked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName != "")
            {
                this.actions.Clear();
                Console.WriteLine("Choose: " + File.ReadAllText(dialog.FileName));
                var newActions = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(dialog.FileName));
                foreach(var action in newActions)
                {
                    this.actions.Add(action.Key, action.Value);
                }
                this.Changes = true;
            }
        }

        private void SaveActionSetButtonClecked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName != "")
            {
                var jsonActions = JsonConvert.SerializeObject(this.actions);
                using (var sw = new StreamWriter(File.Create(dialog.FileName)))
                {
                    Console.WriteLine("Save " + jsonActions);
                    sw.WriteLine(jsonActions);
                }
            }
        }
    }
}
