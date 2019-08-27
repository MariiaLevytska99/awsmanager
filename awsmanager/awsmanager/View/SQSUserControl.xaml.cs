using awsmanager.ViewModels;
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

namespace awsmanager.View
{
    public partial class SQSUserControl : UserControl
    {
        public  QueueViewModel QueueVM { get; set; }
        public SQSUserControl()
        {
            InitializeComponent();
            QueueVM = new QueueViewModel();
            SQSList.ItemsSource = QueueVM.Queues;
            DesignTable();
            SQSList.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }

        private void DesignTable()
        {

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SQSList.RowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Collapsed)
                SQSList.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            else SQSList.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

        }
    }
}
