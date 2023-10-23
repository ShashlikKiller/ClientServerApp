using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.Json;
using Client.Core.Entities;
using System.Threading;

namespace ClientConsole
{
    internal class Program
    {
        private static ObservableCollection<Student> students;
        private static ObservableCollection<Group> groups;
        private static ObservableCollection<LearningStatus> learningStatuses;

        internal static ObservableCollection<Student> Students { get => students; set => students = value; }
        internal static ObservableCollection<Group> Groups { get => groups; set => groups = value; }
        internal static ObservableCollection<LearningStatus> LearningStatuses { get => learningStatuses; set => learningStatuses = value; }

        static void Main(string[] args)
        {

            #region client initialization
            const string ip = "127.0.0.1"; // this is client's ip and port
            const int port = 8082;
            EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
            EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);
            #endregion
            while (true)
            {
                Thread.Sleep(5000);
                do
                {
                    Groups = GetList<Group>("groups", udpSocket, serverEndPoint);
                    Students = GetList<Student>("students", udpSocket, serverEndPoint);
                    LearningStatuses = GetList<LearningStatus>("learningstatuses", udpSocket, serverEndPoint);
                }
                while (udpSocket.Available > 0);
                if (Groups != null) foreach (Group group in Groups)
                    {
                        Console.WriteLine($"Group id: {group.id}, group name: {group.name}");
                    }
                if (Students != null)  foreach (Student student in Students)
                    {
                        Console.WriteLine($"Student id: {student.id}, Student name: {student.name} {student.surname}; group: {student.Group.name}");
                    }
                if (LearningStatuses != null) foreach (LearningStatus status in LearningStatuses)
                    {
                        Console.WriteLine($"Status id: {status.id}, status name: {status.status}");
                    }
                //Console.WriteLine(data);

                #region рабочий код.
                //#region client initialization
                //const string ip = "127.0.0.1"; // this is client's ip and port
                //const int port = 8082;
                //EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
                //EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint
                //var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //udpSocket.Bind(udpEndPoint);
                //ObservableCollection<Group> Groups = new ObservableCollection<Group>();
                //ObservableCollection<Student> Students = new ObservableCollection<Student>();
                //ObservableCollection<LearningStatus> Statuses = new ObservableCollection<LearningStatus>();
                //#endregion
                //while (true)
                //{
                //    var buffer = new byte[4096]; // Инициализация буфера, размера сообщения и данных
                //    int size;
                //    var data = new StringBuilder();
                //    string message = Console.ReadLine();
                //    //udpSocket.SendTo(Encoding.UTF8.GetBytes(message), serverEndPoint);
                //    //Groups = GetList<Group>("groups", udpSocket, serverEndPoint);
                //    do
                //    {
                //        if (message == "groups") Groups = GetList<Group>(message, udpSocket, serverEndPoint);
                //        else if (message == "students") Students = GetList<Student>(message, udpSocket, serverEndPoint);
                //        else if (message == "learningstatuses") Statuses = GetList<LearningStatus>(message, udpSocket, serverEndPoint);
                //    }
                //    while (udpSocket.Available > 0);
                //    if (Groups != null) foreach (Group group in Groups)
                //    {
                //        Console.WriteLine($"Group id: {group.id}, group name: {group.name}");
                //    }
                //    foreach (Student student in Students) if (Students != null)
                //    {
                //        Console.WriteLine($"Student id: {student.id}, Student name: {student.name} {student.surname}; group: {student.Group}");
                //    }
                //    foreach (LearningStatus status in Statuses) if (Statuses != null)
                //    {
                //        Console.WriteLine($"Status id: {status.id}, status name: {status.status}");
                //    }
                //    Console.WriteLine(data);
                #endregion
            }
        }

            /// <summary>
            /// Метод получения сереализованных листов с сервера
            /// </summary>
            /// <typeparam name="T"> Класс объектов листа, который должен получить клиент(универсальный)</typeparam>
            /// <param name="ListToGet">Какой именно лист нам нужен: на стороне сервера идет реализация через switch-case конструкцию. (см. ServerCommandAsync.cs)</param>
            /// <returns></returns>
            public static ObservableCollection<T> GetList<T>(string ListToGet, Socket udpSocket, EndPoint serverEndPoint)
        {
            ObservableCollection<T> NewList;
            SendData(udpSocket, serverEndPoint, ListToGet);
            string answer = ReceiveData(udpSocket, serverEndPoint);
            NewList = new ObservableCollection<T>(JsonSerializer.Deserialize<List<T>>(answer));
            return NewList;
        }

        public static string ReceiveData(Socket udpSocket, EndPoint senderEndPoint)
        {
            string data;
            int size;
            byte[] buffer = new byte[65535];
            size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
            data = Encoding.UTF8.GetString(buffer, 0, size);
            return data;
        }

        public static async Task<string> ReceiveDataAsync(Socket udpSocket, EndPoint senderEndPoint)
        {
            return await Task.Run(() => ReceiveData(udpSocket, senderEndPoint));
        }

        public static void SendData(Socket udpSocket, EndPoint senderEndPoint, string server_answer)
        {
            byte[] responseBuffer;
            responseBuffer = Encoding.UTF8.GetBytes(server_answer);
            udpSocket.SendTo(responseBuffer, SocketFlags.None, senderEndPoint);
        }

        public static async Task SendDataAsync(Socket udpSocket, EndPoint senderEndPoint, string server_answer)
        {
            await Task.Run(() => SendData(udpSocket, senderEndPoint, server_answer));
        }
    }
}