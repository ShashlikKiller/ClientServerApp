using System;
using System.Net.Sockets;
using System.Net;
using ClientServerApp.Database;
using static ClientServerApp.BackEnd.Methods.ServerCommandsAsync;
using Group = ClientServerApp.Database.Group;
using System.Threading;
using System.Linq;

namespace ClientServerApp
{
    internal class Server
    {
        static IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        const int serverPort = 8081;

        static IPAddress clientIP = IPAddress.Parse("127.0.0.1");
        const int clientPort = 8082;

        static void Main(string[] args)
        {
            Console.WriteLine("This is server.\n");
            using (var db = new dbContext())
            {
                Console.Write("Starting and initializing the server...\n");
                #region test
                Console.Write("Students:\n");
                foreach (Student student in db.Student.ToList())
                {
                    Console.WriteLine($"student id: {student.id}, student's name: {student.name}, student's surname: {student.surname}, student's group: {student.Group.name}, student status: {student.LearningStatus.status}");
                }
                Console.Write("Groups:\n");
                foreach (Group group in db.Group.ToList())
                {
                    Console.WriteLine($"group id: {group.id}, group's name: {group.name}");
                }
                Console.Write("Statuses:\n");
                foreach (LearningStatus status in db.LearningStatus.ToList())
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
                    LoggerMessageOutput("error", $"Server can't start: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Основной метод работы сервера, работает через while-true.
        /// </summary>
        /// <param name="udpSocket">Сокет сервера</param>
        /// <param name="db">Контекст базы данных</param>
        private static async void StartReceiving(Socket udpSocket, dbContext db)
        {
            string data; // Данные сообщения от клиента
            EndPoint senderEndPoint = new IPEndPoint(clientIP, clientPort);
            LoggerMessageOutput("start", "Server started successfully!");
            while (true)
            {
                data = ReceiveData(udpSocket, senderEndPoint);
                switch(data)
                {
                    case "delete": // Получение команды на удаление записи по индексу
                        ReceiveDataForDeleteAsync(udpSocket, senderEndPoint, db);
                        break;
                    case "add": // Получение команды на добавление записи
                        ReceiveDataForWriteAsync(udpSocket, senderEndPoint, db);
                        break;
                                            // Получение команды на отправку клиенту
                    case "groups":          // Групп;
                    case "students":        // Студентов;
                    case "learningstatuses":// Статусов.
                        SendList(data, udpSocket, senderEndPoint, db); // Отправка выбранного листа клиенту
                        break;
                    default:
                        await LoggerMessageOutput("error", "Error: can't recognize client's answer. Check the StartReceiving() method.");
                        break;
                }
            }
        }
    }
}
