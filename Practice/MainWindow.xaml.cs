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

namespace Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private readonly IOBSStatus obsStatus = DependencyRegistrator.OBSStatus;
        public MainWindow()
        {
            InitializeComponent();
            this.timer = new DispatcherTimer();
            this.timer.Tick += new EventHandler(Timer_Tick);
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Start();
            for (int i = 0; i < 100; ++i)
            {
                var noise = new WrapPanel();//DependencyRegistrator.EmptyActionElement;
                var label = new Label
                {
                    Content = "Num 1",
                    Width = 100,
                };
                var button = new Button
                {
                    Content = "Шум",
                    Width = 100,
                };
                noise.Children.Add(label);
                noise.Children.Add(button);
                this.Actions.Items.Add(noise);
            }
        }



        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("ogo");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.RecordTime.Content = GetCurrentUnixTime() - this.obsStatus.Time;
        }

        private int GetCurrentUnixTime()
        {
            return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
