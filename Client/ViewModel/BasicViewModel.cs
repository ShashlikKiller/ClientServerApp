using Client.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Client.Core.Methods.ClientConnection;

namespace Client.ViewModel
{
    public class BasicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BasicViewModel()
        {
            //udpSocket.Bind(udpEndPoint);
            //Thread.Sleep(5000);
            //Groups = GetList<Group>("groups", udpSocket, serverEndPoint);
            //Students = GetList<Student>("students", udpSocket, serverEndPoint);
            //LearningStatuses = GetList<LearningStatus>("learningstatuses", udpSocket, serverEndPoint);
        }
    }
}
