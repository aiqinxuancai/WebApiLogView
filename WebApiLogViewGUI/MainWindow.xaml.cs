using MahApps.Metro.Controls;
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
using WebApiLogViewGUI.Service;

namespace WebApiLogViewGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {




        public MainWindow()
        {
            InitializeComponent();

            this.mainLogViewDataGrid.DataContext = LogManager.GetInstance().Logs;

            LogManager.GetInstance().Test();

            mainWindow.Title = $"WebApiLogView [{LogManager.GetInstance().GetAddress()}]";

        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LogManager.GetInstance().Stop();
        }

        private void buttonExportLog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void switchAutoToBottom_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    //_mainLogViewDataGrid.
                }
                else
                {
                }
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //GlobalNotification.Default.Register(LogManager.kNewLogModel, this, (msg =>
            //{
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        //mainLogViewDataGrid.ScrollIntoView(msg.Source);
            //    });
            //}));

            //Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            //{


            //});

        }
    }
}
