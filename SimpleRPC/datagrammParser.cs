using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimpleRPC
{
    /// <summary>
    /// Упаковщик / Распаковщик RPC  
    /// </summary>
    class datagrammParser
    {
        protected List<Type> listOfEnabledTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOfTypes">Перечнь дупустимых передаваемых элементов</param>
        public datagrammParser()
        {
            listOfEnabledTypes = new List<Type>();
            listOfEnabledTypes.Add(typeof(string));
            listOfEnabledTypes.Add(typeof(bool));
            listOfEnabledTypes.Add(typeof(double));
            listOfEnabledTypes.Add(typeof(float));
            listOfEnabledTypes.Add(typeof(Int32));
            listOfEnabledTypes.Add(typeof(Int64));
            listOfEnabledTypes.Add(typeof(System.Drawing.Point));
            listOfEnabledTypes.Add(typeof(System.Drawing.PointF));

            
        }


        #region isInListOfEnabledTypes

        /// <summary>
        /// Является ли данный объект доступным для передачи в качестве параметра
        /// </summary>
        /// <param name="obj">Сам объект</param>
        /// <returns></returns>
        public bool isInListOfEnabledTypes(object obj)
        {
            Type type = obj.GetType();

            foreach (var t in listOfEnabledTypes)
                if (type == t) return true;

            return false;
        }

        /// <summary>
        /// Является ли данный объект доступным для передачи в качестве параметра
        /// </summary>
        /// <param name="obj">Object.GetType()</param>
        /// <returns></returns>
        public bool isInListOfEnabledTypes(Type type)
        {
            foreach (var t in listOfEnabledTypes)
                if (type == t) return true;

            return false;
        }



        /// <summary>
        /// Является ли данный объект доступным для передачи в качестве параметра
        /// </summary>
        /// <param name="TypeHash">Object.GetType().GetHashCode()</param>
        /// <returns></returns>
        public bool isInListOfEnabledTypes(int TypeHash)
        {
            

            foreach (var t in listOfEnabledTypes)
                if (TypeHash == t.GetHashCode()) return true;

            return false;
        }

        #endregion

        /// <summary>
        /// Упаковывает переменные в байтовый массив
        /// </summary>
        /// <param name="vals">'funcName', [] funcParams</param>
        /// <returns></returns>
        public byte [] pack(params object[] vals)
        {
            List<byte> dataToSent = new List<byte>();
            
            for (int i = 0; i < RPC.MarkersLenght; i++)
                dataToSent.Add(RPC.Markers(i));

            foreach (var v in vals)
            {
                if (isInListOfEnabledTypes(v))
                {
                    MemoryStream str = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(str);
                    //Write type hash
                    Type t = v.GetType();
                    bw.Write((Int32)t.GetHashCode());
                    

                    //Choose type params

                    if (t == typeof(string))
                    {
                        string s = (string)v;
                        bw.Write(s);
                    }
                    else if (t == typeof(bool))
                    {
                        bool b = (bool)v;
                        bw.Write(b);
                    }
                    else if (t == typeof(double))
                    {
                        double d = (double)v;
                        bw.Write(d);
                    }
                    else if (t == typeof(float))
                    {
                        float f = (float)v;
                        bw.Write(f);
                    }
                    else if (t == typeof(Int32))
                    {
                        Int32 i = (Int32)v;
                        bw.Write(i);
                    }
                    else if (t == typeof(Int64))
                    {
                        Int64 l = (Int64)v;
                        bw.Write(l);
                    }
                    else if (t == typeof(System.Drawing.Point))
                    {
                        System.Drawing.Point p = (System.Drawing.Point)v;
                        bw.Write(p.X);
                        bw.Write(p.Y);
                    }
                    else if (t == typeof(System.Drawing.PointF))
                    {
                        System.Drawing.PointF p = (System.Drawing.PointF)v;
                        bw.Write(p.X);
                        bw.Write(p.Y);
                    }

                    dataToSent.AddRange(str.ToArray());                    
                }
                else
                {
                    throw new Exception("This object can't be packed!");
                }
            }//end foreach
            return dataToSent.ToArray();
                
        }

        public Object[] unpack(byte [] mass)
        {
            List<Object> objList = new List<object>();
            MemoryStream str = new MemoryStream(mass);
            BinaryReader br = new BinaryReader(str);

            for (int i = 0; i < RPC.MarkersLenght; i++)
            {
                br.ReadByte();
            }

            //while ()
           
            Int32 tHash = br.ReadInt32();


            //int a = o;  

            /* listOfEnabledTypes.Add(typeof(string));
             * listOfEnabledTypes.Add(typeof(bool));
            listOfEnabledTypes.Add(typeof(double));
            listOfEnabledTypes.Add(typeof(float));
            listOfEnabledTypes.Add(typeof(Int32));
            listOfEnabledTypes.Add(typeof(Int64));
            listOfEnabledTypes.Add(typeof(System.Drawing.Point));
            listOfEnabledTypes.Add(typeof(System.Drawing.PointF));
             */


            return null;
        }

    }
}
