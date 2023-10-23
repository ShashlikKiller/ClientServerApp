using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Client.Core.Entities;
using System.Collections.ObjectModel;

namespace Client.Core.Methods
{
    public class ClientConnection
    {

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
    }
}
