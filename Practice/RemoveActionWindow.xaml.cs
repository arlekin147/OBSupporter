using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for RemoveActionWindow.xaml
    /// </summary>
    /// 


    public class ComboBoxText
    {
        public ComboBoxText(string text)
        {
            this.Text = text;
        }
        public string Text { get; set; }
    }

    public partial class RemoveActionWindow : Window
    {
        Dictionary<string, string> actions;


        public RemoveActionWindow(ref Dictionary<string, string> actions)
        {
            InitializeComponent();
            this.actions = actions;
            var list = new LinkedList<ComboBoxText>();
            foreach (var action in actions)
            {
                list.AddLast(new ComboBoxText(action.Key + " " + action.Value));
            }

                this.ActionsToRemove.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            this.actions.Remove(((ComboBoxText)this.ActionsToRemove.SelectedItem).Text.Split(' ')[0]);
            this.DialogResult = true;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
