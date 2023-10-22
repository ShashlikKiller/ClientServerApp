using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Client.Core.Methods
{
    public class ClientConnection
    {
        static byte[] buffer = new byte[255]; // Инициализация буфера, размера сообщения и данных
        const string ip = "127.0.0.1"; // this is client's ip and port
        const int port = 8082;
        static readonly Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        static EndPoint udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // client's endpoint
        static EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081); // server's endpoint

        public static void ClientInitialization()
        {
            #region client initialization
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);
            #endregion
            #region working with server
            while (true)
            {
                int size;
                var data = new StringBuilder();
                string message = "test"; // TODO: Remake this
                udpSocket.SendTo(Encoding.UTF8.GetBytes(message), serverEndPoint);
                do
                {
                    size = udpSocket.ReceiveFrom(buffer, ref serverEndPoint);
                    data.Append(Encoding.UTF8.GetString(buffer));
                }
                while (udpSocket.Available > 0);
            }
            // TODO: Закрытие сокета
            #endregion
        }

        /// <summary>
        /// Метод получения сереализованных листов с сервера
        /// </summary>
        /// <typeparam name="T"> Класс объектов листа, который должен получить клиент(универсальный)</typeparam>
        /// <param name="ListToGet">Какой именно лист нам нужен: на стороне сервера идет реализация через switch-case конструкцию. (см. ServerCommandAsync.cs)</param>
        /// <returns></returns>
        public List<T> GetList<T>(string ListToGet)
        {
            var _buffer = buffer; // Инициализация буфера, размера сообщения и данных
            List<T> NewList;
            udpSocket.SendTo(Encoding.UTF8.GetBytes(ListToGet), serverEndPoint);
            NewList = JsonSerializer.Deserialize<List<T>>(udpSocket.ReceiveFrom(_buffer, ref serverEndPoint));
            return NewList;
        }
    }
}
