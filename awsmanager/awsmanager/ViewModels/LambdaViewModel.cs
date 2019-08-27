using System;
using System.Collections.ObjectModel;
using awsmanagerLib.Models;
using awsmanagerLib.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsmanager.ViewModels
{
    public class LambdaViewModel : MainViewModel
    {
        public ObservableCollection<Lambda> Functions { get; set; }
        public LambdaRepository LambdaRepository { get; set; }

        public LambdaViewModel()
        {
            LambdaRepository = new LambdaRepository();
            Functions = new ObservableCollection<Lambda>(LambdaRepository.lambdaRepository);
        }
    }
}
