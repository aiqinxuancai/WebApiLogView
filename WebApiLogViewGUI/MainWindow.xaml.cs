
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WebApiLogViewGUI.Model;
using WebApiLogViewGUI.Service;
using Wpf.Ui.Controls;

namespace WebApiLogViewGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _autoToBottom = true;
        private bool _openRegexp = false;

        private ICollectionView defaultView;

        public MainWindow()
        {
            InitializeComponent();

            //this.mainLogViewDataGrid.DataContext = LogManager.GetInstance().Logs;

            this.defaultView = CollectionViewSource.GetDefaultView(LogManager.GetInstance().Logs);
            this.defaultView.Filter =
                w =>
                {
                    if (_openRegexp)
                    {
                        Regex regex = new Regex(textBoxFilter.Text);
                        return regex.IsMatch(((LogModel)w).Message);
                    }
                    else
                    {
                        return ((LogModel)w).Message.Contains(textBoxFilter.Text);
                    }
                };

            mainLogViewDataGrid.ItemsSource = this.defaultView;

            TitleBar.Title = $"WebApiLogView [{LogManager.GetInstance().GetAddress()}]";

        }


        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GlobalNotification.Default.Register(LogManager.kNewLogModel, this, (msg =>
            {
                if (_autoToBottom)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        mainLogViewDataGrid.ScrollIntoView(msg.Source);
                    });
                }

            }));


            LogManager.GetInstance().GetAddress();


#if DEBUG
            LogManager.GetInstance().Test();
#endif
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LogManager.GetInstance().Stop();
            Environment.Exit(0);
        }



        private void switchAutoToBottom_Toggled(object sender, RoutedEventArgs e)
        {
            CheckBox toggleSwitch = sender as CheckBox;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsChecked == true)
                {
                    //_mainLogViewDataGrid.
                   
                }
                else
                {
                }

                _autoToBottom = (bool)toggleSwitch.IsChecked;
            }
        }


        private async void buttonExportLog_Click(object sender, RoutedEventArgs e)
        {
            var fileName = LogManager.GetInstance().Save();
            //MessageDialogResult messageResult = await this.ShowMessageAsync("导出成功！", $"文件保存为：{fileName}");
        }


        private void buttonClearLog_Click(object sender, RoutedEventArgs e)
        {
            LogManager.GetInstance().ClearAll();
        }
         
        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            defaultView.Refresh();
        }

        private void switchAutoToBottom_Checked(object sender, RoutedEventArgs e)
        {
            _autoToBottom = (bool)switchAutoToBottom.IsChecked;
        }

        private void switchAutoToBottom_Unchecked(object sender, RoutedEventArgs e)
        {
            _autoToBottom = (bool)switchAutoToBottom.IsChecked;
        }


        private void checkBoxOpenRegexp_Checked(object sender, RoutedEventArgs e)
        {
            _openRegexp = (bool)checkBoxOpenRegexp.IsChecked;
            defaultView.Refresh();
        }

        private void checkBoxOpenRegexp_Unchecked(object sender, RoutedEventArgs e)
        {
            _openRegexp = (bool)checkBoxOpenRegexp.IsChecked;
            defaultView.Refresh();
        }

        private void mainLogViewDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.OriginalSource
            if (e.AddedItems.Count > 0)
            {
                LogModel logModel = (LogModel)e.AddedItems[0];
                textBoxMessage.Text = logModel.Message;
            }
        }
    }
}
