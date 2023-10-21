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
            Logger logger = LogManager.GetCurrentClassLogger();
            Console.WriteLine("This is server.");
            logger.Info($"Server started at:{DateTime.Now}");

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
                    //StartReceiving(udpSocket, logger, db);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    logger.Error($"Socket/EndPoint error: {e.Message}");
                }
            }
            #endregion
        }

        private static async void StartReceiving(Socket udpSocket, Logger logger, dbEntities db) // = task
        {
            string server_answer; // Переменная ответа сервера клиенту
            string data; // Данные сообщения от клиента
            List<Student> Students = db.Students.ToList();
            List<Group> Groups = db.Groups.ToList();
            List<LearningStatus> LearningStatuses = db.LearningStatuses.ToList();
            List<Student> newStudents = new List<Student>(); // Лист новых, добавленных во время работы приложения студентов
            IPAddress clientIP = IPAddress.Parse("127.0.0.1"); // this is client's ip and port
            const int clientPort = 8082;
            EndPoint senderEndPoint = new IPEndPoint(clientIP, clientPort); // Эндпоинт клиента(отправителя сообщений)
            logger.Info($"Server started at {DateTime.Now}");
            Console.WriteLine("Server started successfully!");
            while (true)
            {
                data = ReceiveDataAsync(udpSocket, senderEndPoint).Result;

                //if (TypeCheck.IntCheckBool(data))
                //{
                Console.WriteLine($"Client choice: {data}"); // Вывод выбора пользователя в меню.
                logger.Info($"Client choice: {data}");
                //switch (Convert.ToInt32(data))
                //{
                //    case 1: // Вывод всех записей на экран 
                //        server_answer = "test";// OutputAllAsync(Students).Result;
                //        break;
                //    case 2: // Вывод записи по номеру 
                //        server_answer = OutputByIDAsync(udpSocket, senderEndPoint, Students).Result;
                //        break;
                //    case 3: // Запись данных в файл 
                //        server_answer = RecordsSaveAsync(newStudents, db).Result;
                //        break;
                //    case 4: // Удалить запись по номеру
                //        server_answer = DeleteRecordByIDAsync(udpSocket, senderEndPoint, Students, db).Result;
                //        break;
                //    case 5: // Добавление новой записи
                //        server_answer = AddNewRecordAsync(udpSocket, senderEndPoint, Students, newStudents, Groups, LearningStatuses).Result;
                //        break;
                //    default:
                //        server_answer = "Incorrect input. Please press the button from 1 to 5.";
                //        logger.Error("Incorrect user input");
                //        break;
                //}
                //SendDataAsync(udpSocket, senderEndPoint, server_answer + BackToMenu); // NO AWAIT
                server_answer = "test";
                logger.Info($"server send: {server_answer}");
                //}
                //else
                //{
                SendDataAsync(udpSocket, senderEndPoint, "Invalid data. Try again."); // NO AWAIT
                logger.Error("Incorrect user input");
                Console.WriteLine("Received invalid data");
                //}
            }
        }
        private static string BackToMenu = "Вы возвращены в меню. Выберите пункт от 1 до 5.\n";
    }
}
