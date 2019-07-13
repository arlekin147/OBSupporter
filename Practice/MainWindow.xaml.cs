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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Practice.Observers;
using Practice.Configuration;
using Practice.Factory;
using System.IO;

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private const int WindowWidth = 200;
        private readonly DispatcherTimer timer;
        private readonly IOBSStatus obsStatus = DependencyRegistrator.OBSStatus;
        private readonly IActionCreator actionCreator = DependencyRegistrator.ActionCreator;
        private readonly LinkedList<string> logs = new LinkedList<string>();//File.CreateText("ogo.log");
        private Dictionary<string, string> actions = new Dictionary<string, string>();
        private Stack<string> redoStack = new Stack<string>();
        public MainWindow()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(Timer_Tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Start();

            this.actionCreator.ActionHasHappened += (str) => { this.logs.AddLast(str); redoStack.Clear(); };
            this.actionCreator.ActionHasHappened += (str) => { this.LogsPanel.Items.Add(new Label { Content = str }); redoStack.Clear(); };
            this.actionCreator.Origin = this.RecordTime;
            this.ContinuePanel.Children.Add(this.actionCreator.CreateAction("Num 0", "Continiue", WindowWidth));


            foreach (var el in this.actions)
            {
                this.Actions.Items.Add(el);
            }
        }



        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ogo");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("CurrentTime = " + GetCurrentUnixTime());
            Console.WriteLine("RecordStartTime = " + this.obsStatus.Time);
            this.OBSStatus.Content = this.obsStatus.Time != 0 ? "Inactive :(" : "Active! :)";
            this.CameraStatus.Content = "Inactive :(";
            this.RecordTime.Content = this.obsStatus.Time != 0 ? new TimeSpan((long)(GetCurrentUnixTime() - this.obsStatus.Time) * 10000000).ToString() : "00:00:00";
        }

        private double GetCurrentUnixTime()
        {
            Console.WriteLine((double)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds);
            return (double)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void OpenAddActionWindow(object sender, RoutedEventArgs e)
        {
            var addActionWindow = new AddActionWindow();

            if (addActionWindow.ShowDialog().Value)
            {
                this.actions.Add(addActionWindow.Shortcut, addActionWindow.Action);
                this.Actions.Items.Clear();
                foreach(var el in actions)
                {
                    this.Actions.Items.Add(this.actionCreator.CreateAction(el.Key, el.Value, WindowWidth));
                }
            }
        }

        private void OpenSettingsWindow(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(ref this.actions);
            if (settingsWindow.ShowDialog().Value && settingsWindow.Changes)
            {
                this.Actions.Items.Clear();
                foreach (var el in actions)
                {
                    this.Actions.Items.Add(this.actionCreator.CreateAction(el.Key, el.Value, WindowWidth));
                }
            }
        }

        public void Dispose()
        {
            this.obsStatus.Dispose();
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName != "")
            {
                using (var file = File.CreateText(dialog.FileName))
                {
                    foreach (var str in this.logs)
                    {
                        file.WriteLine(str);
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void UndoButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.logs.Count != 0)
            {
                this.redoStack.Push(this.logs.Last.Value);
                this.logs.RemoveLast();
                this.LogsPanel.Items.RemoveAt(this.LogsPanel.Items.Count - 1);
            }
        }

        private void RedoButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.redoStack.Count != 0)
            {
                this.logs.AddLast(this.redoStack.Pop());
                this.LogsPanel.Items.Add(new Label() { Content = this.logs.Last.Value });
            }
        }

        private void ResetButtonClicked(object sender, RoutedEventArgs e)
        {
            this.logs.Clear();
            this.LogsPanel.Items.Clear();
        }

    }
}
