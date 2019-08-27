using System;
using awsmanagerLib.Models;
using awsmanagerLib.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace awsmanager.ViewModels
{
  public  class UrlViewModel : MainViewModel
    {
       
        public ObservableCollection<ApiGatewayUrl> urls { get; set; }
        public UrlRepository UrlRepository { get; set; }

        public UrlViewModel()
        {
            UrlRepository = new UrlRepository();
            urls = new ObservableCollection<ApiGatewayUrl>(UrlRepository.urlrepository);

        }

    }
}
