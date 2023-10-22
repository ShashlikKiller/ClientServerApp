using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Text.Json;
using static ClientServerApp.BackEnd.Methods.DBController;
using ClientServerApp.Database;
using System.Reflection;

namespace ClientServerApp.BackEnd.Methods
{
    public class ServerCommandsAsync
    {
        #region Receive/Send data methods
        public static string ReceiveData(Socket udpSocket, EndPoint senderEndPoint)
        {
            string data;
            int size;
            byte[] buffer = new byte[256];
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
        #endregion

        static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Вывод информации в консоль и логи на стороне сервера.
        /// </summary>
        /// <param name="message">Сообщение, которое нужно вывести в консоль и логи</param>
        /// <param name="type">Тип сообщения ("error", "info", "start")</param>
        /// <returns></returns>
        public async static Task LoggerMessageOutput(string message, string type)
        {
            Console.WriteLine(message);
            switch (type)
            {
                case "error":
                    logger.Error(message);
                    break;
                case "info":
                    logger.Info(message);
                    break;
                case "start":
                    logger.Info($"Server start at: {DateTime.Now}.\n {message}");
                    break;
                default:
                    logger.Error("Something went wrong. Check the ServerCommandAsync.cs and LoggerMessageOutput() using.");
                    break;
            }
        }

        /// <summary>
        /// Метод, отправляющий листы сущностей объектов, хранящихся в базе данных.
        /// </summary>
        /// <param name="clientMessage">Строка, указывающая какой лист надо отправить("groups", "students", "learningstatuses").</param>
        /// <returns></returns>
        public async Task SendList(string clientMessage, Socket udpSocket, EndPoint senderEndPoint, dbEntities db)
        {
            while (true)
            {
                try
                {
                    switch (clientMessage)
                    {
                        case "groups":
                            await SendDataAsync(udpSocket, senderEndPoint, JsonSerializer.Serialize(GetGroups(db)));
                            break;
                        case "students":
                            await SendDataAsync(udpSocket, senderEndPoint, JsonSerializer.Serialize(GetStudents(db)));
                            break;
                        case "learningstatuses":
                            await SendDataAsync(udpSocket, senderEndPoint, JsonSerializer.Serialize(GetStatuses(db)));
                            break;
                        default:
                            await LoggerMessageOutput("Error. Check the ClientConnection.cs and Send/Receive method 'GetList<T>.'", "error");
                            break;
                    }
                }
                catch (SocketException e)
                {
                    await LoggerMessageOutput($"Error: {e.Message}", "error");
                }
            }
        }

        public async Task ReceiveDataForWriteAsync(Socket udpSocket, EndPoint senderEndPoint, dbEntities db)
        {
            while (true)
            {
                string _ReceivedDataFromClient = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
                try
                {
                    AddStudent(JsonSerializer.Deserialize<Student>(_ReceivedDataFromClient), db);
                    Console.WriteLine("User add new student.");
                    logger.Trace("Добавление записи прошло успешно");
                }
                catch (Exception e)
                {
                    await LoggerMessageOutput($"Error: {e.Message}", "error");
                }
            }
        }

        public async Task RecieveDataForDeleteAsync(Socket udpSocket, EndPoint senderEndPoint, dbEntities db)
        {
            while (true)
            {
                string message = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
                try
                {
                    if (int.TryParse(message, out int index)) // Чтение по индексу
                    {
                        DeleteStudent(index, db);
                        logger.Info($"User delete student with id = {index}");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    await LoggerMessageOutput($"Error: {e.Message}", "error");
                }
            }
        }
    }
}
