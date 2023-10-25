using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClientServerApp.Database;
using NLog;
using static ClientServerApp.BackEnd.Methods.ServerCommandsAsync;
using Group = ClientServerApp.Database.Group;

namespace ClientServerApp
{
    internal class Server
    {
        // 1. Клиент производить только отображение данных.
        // To Do: Следующие операции производятся на стороне сервера:
        //   Передача запрошенных данных из файла:
        //   Передача всех данных
        //   Передача записи по номеру
        //   Запись новых данных от клиента в файл
        //   Удаление записи из файла по её номеру
        // 2. Логирование операций на стороне сервера с помощью библиотеки NLog
        // 3. Сервер должен многопоточным.
        // Многопоточность реализовать с помощью асинхронных методов класса UDPClient

        static IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        const int serverPort = 8081;

        static void Main(string[] args)
        {
            Console.WriteLine("This is server.\n");
            //logger.Info($"Server started at:{DateTime.Now}");
            using (var db = new dbEntities())
            {
                Console.Write("Starting and initializing the server...\n");
                #region test
                Console.Write("Students:\n");
                foreach (Student student in db.Students)
                {
                    Console.WriteLine($"student id: {student.id}, student's name: {student.name}, student's surname: {student.surname}, student's group: {student.Group.name}, student status: {student.LearningStatus.status}");
                }
                Console.Write("Groups:\n");
                foreach (Group group in db.Groups)
                {
                    Console.WriteLine($"group id: {group.id}, group's name: {group.name}");
                }
                Console.Write("Statuses:\n");
                foreach (LearningStatus status in db.LearningStatuses)
                {
                    Console.WriteLine($"status id: {status.id}, status: {status.status}");
                }
                #endregion
                try
                {
                    var udpEndPoint = new IPEndPoint(serverIP, serverPort);
                    var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    udpSocket.Bind(udpEndPoint);
                    StartReceiving(udpSocket, db);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static async void StartReceiving(Socket udpSocket, dbEntities db) // = task
        {
            string server_answer; // Переменная ответа сервера клиенту
            string data; // Данные сообщения от клиента
            //List<Student> Students = db.Students.ToList();
            //List<Group> Groups = db.Groups.ToList();
            //List<LearningStatus> LearningStatuses = db.LearningStatuses.ToList();
            //List<Student> newStudents = new List<Student>(); // Лист новых, добавленных во время работы приложения студентов
            IPAddress clientIP = IPAddress.Parse("127.0.0.1"); // this is client's ip and port
            int clientPort = 8082;
            EndPoint senderEndPoint = new IPEndPoint(clientIP, clientPort); // Эндпоинт клиента(отправителя сообщений)
            //logger.Info($"Server started at {DateTime.Now}");
            Console.WriteLine("Server started successfully!");
            while (true)
            {
                data = ReceiveData(udpSocket, senderEndPoint);
                SendList(data, udpSocket, senderEndPoint, db);
                //SendDataAsync(udpSocket, senderEndPoint, "test"); // NO AWAIT
            }
        }
    }
}
