using System;
using awsmanagerLib.Models;
using awsmanagerLib.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awsmanager.ViewModels
{
    public class QueueViewModel : MainViewModel
    {
        public ObservableCollection<Queue> Queues { get; set; }
        private QueueRepository QueueRepository { get; set; }

        public QueueViewModel()
        {
            QueueRepository = new QueueRepository();
            Queues = new ObservableCollection<Queue>(QueueRepository.queueRepository);

        }
    }
}
