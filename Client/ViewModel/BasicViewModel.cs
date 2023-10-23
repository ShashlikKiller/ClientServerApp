using Client.Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

        private ObservableCollection<Student> students;
        private ObservableCollection<Group> groups;
        private ObservableCollection<LearningStatus> learningStatuses;

        internal ObservableCollection<Student> Students { get => students; set => students = value; }
        internal ObservableCollection<Group> Groups { get => groups; set => groups = value; }
        internal ObservableCollection<LearningStatus> LearningStatuses { get => learningStatuses; set => learningStatuses = value; }

        public BasicViewModel()
        {
            #region client initialization
            const string ip = "127.0.0.1"; // this is client's ip and port
            const int port = 8082;
            EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);
            //ObservableCollection<Group> Groups = new ObservableCollection<Group>();
            //ObservableCollection<Student> Students = new ObservableCollection<Student>();
            //ObservableCollection<LearningStatus> Statuses = new ObservableCollection<LearningStatus>();
            #endregion
            while (true)
            {
                var buffer = new byte[4096]; // Инициализация буфера, размера сообщения и данных
                int size;
                var data = new StringBuilder();
                //string message = Console.ReadLine();
                string message1 = "groups";
                string message2 = "students";
                string message3 = "learningstatuses";
                //udpSocket.SendTo(Encoding.UTF8.GetBytes(message), serverEndPoint);
                do
                {
                    if (message1 == "groups") Groups = GetList<Group>(message1, udpSocket, serverEndPoint);
                    else if (message2 == "students") Students = GetList<Student>(message2, udpSocket, serverEndPoint);
                    else if (message3 == "learningstatuses") LearningStatuses = GetList<LearningStatus>(message3, udpSocket, serverEndPoint);
                }
                while (udpSocket.Available > 0);
                //if (Groups != null) foreach (Group group in Groups)
                //    {
                //        Console.WriteLine($"Group id: {group.id}, group name: {group.name}");
                //    }
                //foreach (Student student in Students) if (Students != null)
                //    {
                //        Console.WriteLine($"Student id: {student.id}, Student name: {student.name} {student.surname}; group: {student.Group}");
                //    }
                //foreach (LearningStatus status in Statuses) if (Statuses != null)
                //    {
                //        Console.WriteLine($"Status id: {status.id}, status name: {status.status}");
                //    }
                //Console.WriteLine(data);
            }
        }
    }
}
