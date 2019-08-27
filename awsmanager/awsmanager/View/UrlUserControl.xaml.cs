using awsmanagerLib.Models;
using awsmanager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Configuration;
using awsmanagerLib.Configuration;
using awsmanagerLib.Repositories;

namespace awsmanager.View
{
    public partial class UrlUserControl : UserControl
    {
        public UrlViewModel UrlVM { get; set; }
        public ServiceSection ConfigSection { get; private set; }

        public UrlUserControl()
        {
            InitializeComponent();
            UrlVM = new UrlViewModel();
            UrlList.ItemsSource = UrlVM.urls;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UrlList.Visibility = Visibility.Visible;
            var url = new ApiGatewayUrl();
            url.CheckUrl(UrlTxt.Text);
            UrlVM.UrlRepository.addNewUrl(UrlTxt.Text);
            UrlVM.urls.Add(url);

            var config = ConfigurationManager.OpenMappedMachineConfiguration(new ConfigurationFileMap(@"..\..\ServiceConfiguration.config"));
            ConfigSection = config.GetSection("serviceSection") as ServiceSection;

            ConfigSection.Add(new Service
            {
                ServiceType = "ApiGateWay",
                ServiceName = url.Url,
                Metric = "url",
                Value = url.Code.Description
            });
            config.Save(ConfigurationSaveMode.Modified);
            UrlList.Items.Refresh();
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }
    }
}




