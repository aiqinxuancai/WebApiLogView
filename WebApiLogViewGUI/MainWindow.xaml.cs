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

            this._mainLogViewDataGrid.DataContext = LogManager.GetInstance().Logs;

            LogManager.GetInstance().Test();

            mainWindow.Title = $"WebApiLogView [{LogManager.GetInstance().GetAddress()}]";
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LogManager.GetInstance().Stop();
        }
    }
}
