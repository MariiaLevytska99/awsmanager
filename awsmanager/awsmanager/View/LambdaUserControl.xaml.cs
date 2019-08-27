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
using awsmanagerLib.Models;

namespace awsmanager.View
{

    public partial class LambdaUserControl : UserControl
    {
        public LambdaViewModel LambdaVM { get; set; }
        public LambdaUserControl()
        {
            InitializeComponent();
            LambdaVM = new LambdaViewModel();
            LambdaList.ItemsSource = LambdaVM.Functions;
            LambdaList.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

        }
        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedItem = this.LambdaList.CurrentItem as Lambda;
            if(comboBox.SelectedItem.ToString().Equals("AllowExecution"))
            LambdaVM.LambdaRepository.ResumeLambda(selectedItem.FunctionName);
            else
                LambdaVM.LambdaRepository.StopLambda(selectedItem.FunctionName);



        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (LambdaList.RowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Collapsed)
                LambdaList.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            else LambdaList.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

        }
    }
}
