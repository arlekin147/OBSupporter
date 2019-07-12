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

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WindowWidth = 200;
        private readonly DispatcherTimer timer;
        private readonly IOBSStatus obsStatus = DependencyRegistrator.OBSStatus;
        private readonly IActionCreator actionCreator = DependencyRegistrator.ActionCreator; 
        private readonly Dictionary<string, string> actions = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(Timer_Tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Start();
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
            this.RecordTime.Content = UnixTimeStampToDateTime(GetCurrentUnixTime() - this.obsStatus.Time);
        }

        private double GetCurrentUnixTime()
        {
            return (double)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
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
                    this.Actions.Items.Add(this.actionCreator.CreateAction(el.Key, el.Value, this.WindowWidth));
                }
            }
        }
    }
}
