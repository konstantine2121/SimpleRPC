using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SimpleRPC
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RPC : Attribute
    {
        protected static byte[] markers =  { 0x55, 0xFF, 0x00, 0x55, 0xAA, 0xCC, 0xF0, 0xCC, 0x55, 0xFF, 0x00, 0x55, 0xAA, 0xCC, 0xF0, 0xCC };

        public static byte Markers(int i)
        {
            if (i >= 0 & i < markers.Length)
                return markers[i];
            else return 0;

        }
        public static int MarkersLenght
        {
            get
            {
                return markers.Length;
            }
        }

        /// <summary>
        /// Проверяем, является ли данное сообщение RPC
        /// </summary>
        /// <param name="data">входящее сообщение</param>
        /// <returns></returns>
        public static bool isMessageRPC(byte[] data)
        {
            if (data != null && data.Length > markers.Length)
            {


                for (int i = 0; i < markers.Length; i++)
                {
                    if (markers[i] != data[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }


        public enum RPCsentTo { All, Client, Server };



        /// <summary>
        /// Отправка пакета
        /// </summary>
        /// <param name="datagram">содержение пакета</param>
        /// <param name="remoteIPAddress">IP адрес получателя</param>
        /// <param name="remotePort">Порт получателя</param>
        public static void Send(string datagram, string remoteIPAddress, int remotePort)
        {

            // Создаем UdpClient
            UdpClient sender = new UdpClient();
            try
            {
                // Создаем endPoint по информации об удаленном хосте
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(remoteIPAddress), remotePort);
                // Преобразуем данные в массив байтов
                byte[] bytes = Encoding.UTF8.GetBytes(datagram);
                // Отправляем данные
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
            finally
            {
                // Закрыть соединение
                sender.Close();
            }
        }


        /// <summary>
        /// Получение данных. (Нужно вызывать в отдельном потоке)
        /// </summary>
        public void RecievingFunction(int localPort)
        {
            // Создаем UdpClient для чтения входящих данных
            UdpClient receivingUdpClient = new UdpClient(localPort);
            IPEndPoint RemoteIpEndPoint = null;

            try
            {
                while (true)
                {
                    // Ожидание дейтаграммы
                    byte[] receiveBytes = receivingUdpClient.Receive(ref RemoteIpEndPoint);

                    //Can it be rpc call
                    bool isRPC = RPC.isMessageRPC(receiveBytes);

                    // Преобразуем и отображаем данные
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                }
            }
            catch (Exception ex)
            {
                //  Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        //End of RPC
    }


}
