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
using System.Text.Json.Serialization;

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
        public static void LoggerMessageOutput(string type, string message)
        {
            switch (type)
            {
                case "error":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    logger.Error(message);
                    break;
                case "info":
                    Console.WriteLine(message);
                    logger.Info(message);
                    break;
                case "start":
                    string output = $"Server start at: {DateTime.Now}.\n {message}";
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(output);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    logger.Info(output);
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
        public static void SendList(string clientMessage, Socket udpSocket, EndPoint senderEndPoint, dbEntities db)
        {
            try
            {
                switch (clientMessage)
                {
                    case "groups":
                        List<Group> groups = db.Groups.ToList();
                        SendData(udpSocket, senderEndPoint, JsonSerializer.Serialize(groups));
                        LoggerMessageOutput("info", "List of groups was sended to client.");
                        break;
                    case "students":
                        List<Student> students = db.Students.ToList();
                        SendData(udpSocket, senderEndPoint, JsonSerializer.Serialize(students));
                        LoggerMessageOutput("info", "List of students was sended to client.");

                        break;
                    case "learningstatuses":
                        List<LearningStatus> learningStatuses = db.LearningStatuses.ToList();
                        SendData(udpSocket, senderEndPoint, JsonSerializer.Serialize(learningStatuses));
                        LoggerMessageOutput("info", "List of statuses was sended to client.");

                        break;
                    default:
                        LoggerMessageOutput("error", "Error. Check the ClientConnection.cs and Send/Receive method 'GetList<T>.'");
                        break;
                }
            }
            catch (SocketException e)
            {
                LoggerMessageOutput($"Error: {e.Message}", "error");
            }
        }

        public static async Task ReceiveDataForWriteAsync(Socket udpSocket, EndPoint senderEndPoint, dbEntities db)
        {
                string _receivedStudent = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
                try
                {
                    AddStudent(JsonSerializer.Deserialize<Student>(_receivedStudent), db);
                    LoggerMessageOutput("info", "Success add.");
                }
                catch (Exception e)
                {
                    LoggerMessageOutput($"Error: {e.Message}", "error");
                }
        }

        public static async Task ReceiveDataForDeleteAsync(Socket udpSocket, EndPoint senderEndPoint, dbEntities db)
        {
            string _receivedIndex = ReceiveDataAsync(udpSocket, senderEndPoint).Result;
            try
            {
                if (int.TryParse(_receivedIndex, out int index))
                {
                    DeleteStudent(index, db);
                    logger.Info($"User delete student with id = {index}");
                }
                else LoggerMessageOutput("error", $"error: {_receivedIndex} is'nt a int. Check the client side.");
            }
            catch (Exception e)
            {
                LoggerMessageOutput($"Error: {e.Message}", "error");
            }
        }
    }
}
