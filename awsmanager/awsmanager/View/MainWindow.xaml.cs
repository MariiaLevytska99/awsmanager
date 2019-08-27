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

namespace awsmanager.View
{
    /// <summary>
    /// Interaction logic for WindowDemo.xaml
    /// </summary>
    public partial class WindowDemo : Window
    {
        public static SQSUserControl sqs { get; set; }
        public static LambdaUserControl lambda { get; set; }
        public static UrlUserControl url { get; set; }
        public static LoaingUserControl load { get; set; }
        private string currentControl { get; set; }
        public WindowDemo()
        {
            InitializeComponent();
            sqs = new SQSUserControl();
            lambda = new LambdaUserControl();
            url = new UrlUserControl();
            load = new LoaingUserControl();
            contentControl.Content = sqs;
            currentControl = "sqs";
        }
        private void MenuItem_SQSClick(object sender, RoutedEventArgs e)
        {
            contentControl.Content = sqs;
            currentControl = "sqs";
        }

        private void MenuItem_LambdaClick(object sender, RoutedEventArgs e)
        {

            contentControl.Content = lambda;
            currentControl = "lambda";
        }

        private void MenuItem_GateWayClick(object sender, RoutedEventArgs e)
        {
            contentControl.Content = url;
            currentControl = "url";
        }
        private async void MenuItem_RefreshClick(object sender, RoutedEventArgs e)
        {
            contentControl.Content = load;
            await Task.Run(() => System.Threading.Thread.Sleep(2000));
            switch (currentControl)
              {
                  case "sqs":
                       sqs = new SQSUserControl();

                       contentControl.Content = sqs;
                      break;
                  case "lambda":
                       lambda = new LambdaUserControl();
                       contentControl.Content = lambda;
                      break;
                  case "url":
                      url = new UrlUserControl();
                      contentControl.Content = url;
                      break;
                  default:
                      break;
              }

        }
       
        }
    }
