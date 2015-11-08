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

    public class RPC_Example
    {
        public int localPort = 27015;
        protected bool continueReciving = true;
        datagrammParser parser;

        protected Thread RecievingThread;

        protected MethodInfo[] RpcsMethods;

        public bool puckk(params  object[] obj)
        {
            parser.oldPack(obj);
            return true;
        }


        public RPC_Example()
        {
            GetRPClist();
            continueReciving = true;



            parser = new datagrammParser(typeof(Int32), typeof(Int64), typeof(String), typeof(float), typeof(double));

        }

        public bool Call(string methodName, RPC.RPCsentTo whomToSent, params Object[] param)
        {
            if (!isInRPClist(methodName)) return false;

            foreach (var p in param)
                if (!parser.isInListOfEnabledTypes(p)) return false;


            return true;
        }




        /// <summary>
        /// Получение данных. (Нужно вызывать в отдельном потоке)
        /// </summary>
        protected void RecievingFunction()
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


        /// <summary>
        /// Получить текущий список фукнкций с атрибутом RPC
        /// </summary>
        protected void GetRPClist()
        {
            MethodInfo[] mi = this.GetType().GetMethods();
            if (mi != null && mi.Length > 0)
            {
                List<MethodInfo> ml = new List<MethodInfo>();
                foreach (var m in mi)
                {
                    object[] types = m.GetCustomAttributes(true);
                    foreach (var t in types)
                    {
                        if (t.GetType() == typeof(RPC))
                        {
                            ml.Add(m); break;
                        }
                    }

                    int a;
                }

                RpcsMethods = ml.ToArray();
            }
        }


        /// <summary>
        /// Отправка пакета
        /// </summary>
        /// <param name="datagram">содержение пакета</param>
        /// <param name="remoteIPAddress">IP адрес получателя</param>
        /// <param name="remotePort">Порт получателя</param>
        protected static void Send(string datagram, string remoteIPAddress, int remotePort)
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


        protected bool isInRPClist(string MethodName)
        {
            if (MethodName != null && MethodName.Length > 0)
            {
                foreach (var n in RpcsMethods)
                {
                    if (MethodName == n.Name)
                        return true;
                }
            }
            return false;
        }

    }
}
