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
        static void Main(string[] args)
        {
            Console.WriteLine("This is server.");
            //logger.Info($"Server started at:{DateTime.Now}");

            const string ip = "127.0.0.1";
            const int port = 8081; // У КЛИЕНТА И СЕРВЕРА РАЗНЫЕ ПОРТЫ! СВЯЗЬ ЧЕРЕЗ СЕРВЕР И КЛИЕНТ ЭНДПОИНТ
            #region database init
            using (var db = new dbEntities())
            {
                #region test
                Console.Write("Students:\n");
                foreach (Student student in db.Students)
                {
                    Console.WriteLine($"student id: {student.id}, student's name: {student.name}, student's surname: {student.surname}, student's group:{student.group_id}, {student.Group.name}");
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
                    var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    udpSocket.Bind(udpEndPoint);
                    StartReceiving(udpSocket, db);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            #endregion
        }

        private static async void StartReceiving(Socket udpSocket, dbEntities db) // = task
        {
            string server_answer; // Переменная ответа сервера клиенту
            string data; // Данные сообщения от клиента
            List<Student> Students = db.Students.ToList();
            List<Group> Groups = db.Groups.ToList();
            List<LearningStatus> LearningStatuses = db.LearningStatuses.ToList();
            IPAddress clientIP = IPAddress.Parse("127.0.0.1"); // this is client's ip and port
            const int clientPort = 8082;
            EndPoint senderEndPoint = new IPEndPoint(clientIP, clientPort); // Эндпоинт клиента(отправителя сообщений)
            Console.WriteLine("Server started successfully!");
            while (true)
            {
                data = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
                await LoggerMessageOutput($"Client menu's choice: {data}", "info");
                SendDataAsync(udpSocket, senderEndPoint, "Invalid data. Try again."); // NO AWAIT
                Console.WriteLine("Received invalid data");
                //}
            }
        }
    }
}
