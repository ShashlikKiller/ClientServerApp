﻿using System;
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
        public static async Task LoggerMessageOutput(string type, string message)
        {
            switch (type)
            {
                case "error":
                    await Task.Run(() =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        logger.Error(message);
                    });
                    break;
                case "info":
                    await Task.Run(() =>
                    {
                        Console.WriteLine(message);
                        logger.Info(message);
                    });
                    break;
                case "start":
                    await Task.Run(() =>
                    {
                        string output = $"Server start at: {DateTime.Now}.\n {message}";
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(output);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        logger.Info(output);
                    });
                    break;
                default:
                    Console.WriteLine("Error at LoggerMessageOutput. Check this method at ServerCommandAsync.cs");
                    logger.Error("Something went wrong. Check the ServerCommandAsync.cs and LoggerMessageOutput() using.");
                    break;
            }
        }

        /// <summary>
        /// Метод, отправляющий листы сущностей объектов, хранящихся в базе данных.
        /// </summary>
        /// <param name="clientMessage">Строка, указывающая какой лист надо отправить("groups", "students", "learningstatuses").</param>
        /// <returns></returns>
        public static async void SendList(string clientMessage, Socket udpSocket, EndPoint senderEndPoint, dbContext db)
        {
            try
            {
                switch (clientMessage)
                {
                    case "groups":
                        List<Group> groups = db.Group.ToList();
                        SendData(udpSocket, senderEndPoint, JsonSerializer.Serialize(groups));
                        await LoggerMessageOutput("info", "List of groups was sended to client.");
                        break;
                    case "students":
                        List<Student> students = db.Student.ToList();
                        SendData(udpSocket, senderEndPoint, JsonSerializer.Serialize(students));
                        await LoggerMessageOutput("info", "List of students was sended to client.");

                        break;
                    case "learningstatuses":
                        List<LearningStatus> learningStatuses = db.LearningStatus.ToList();
                        SendData(udpSocket, senderEndPoint, JsonSerializer.Serialize(learningStatuses));
                        await LoggerMessageOutput("info", "List of statuses was sended to client.");

                        break;
                    default:
                        await LoggerMessageOutput("error", "Error. Check the ClientConnection.cs and Send/Receive method 'GetList<T>.'");
                        break;
                }
            }
            catch (SocketException e)
            {
                await LoggerMessageOutput($"Error: {e.Message}", "error");
            }
        }

        public static async Task ReceiveDataForWriteAsync(Socket udpSocket, EndPoint senderEndPoint, dbContext db)
        {
                string _receivedStudent = await ReceiveDataAsync(udpSocket, senderEndPoint);
                try
                {
                    AddStudent(JsonSerializer.Deserialize<Student>(_receivedStudent), db);
                    await LoggerMessageOutput("info", "Success add.");
                }
                catch (Exception e)
                {
                    await LoggerMessageOutput($"Error: {e.Message}", "error");
                }
        }

        public static async Task ReceiveDataForDeleteAsync(Socket udpSocket, EndPoint senderEndPoint, dbContext db)
        {
            string _receivedIndex = await ReceiveDataAsync(udpSocket, senderEndPoint);
            try
            {
                if (int.TryParse(_receivedIndex, out int index))
                {
                    DeleteStudent(index, db);
                    await LoggerMessageOutput("info", $"User delete student with id = {index}");
                }
                else await LoggerMessageOutput("error", $"error: {_receivedIndex} is'nt a int. Check the client side.");
            }
            catch (Exception e)
            {
                await LoggerMessageOutput($"Error: {e.Message}", "error");
            }
        }
    }
}
