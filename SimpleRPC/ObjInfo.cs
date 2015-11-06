using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;


namespace SimpleRPC
{
    /// <summary>
    /// Структура для перевода объекта структуры в массив байт и обратно без потери информации. Подходит для работы со структурами в которых нет ссылочных значений. Не подходит для работы с классами.
    /// </summary>
    public struct ObjectInfo
    {
        public short nameLenght;
        public short objectLenght;
        public string typeName;
        public byte[] objectData;
        

        /// <summary>
        /// Задать через экземпляр объекта
        /// </summary>
        /// <param name="obj"></param>
        public ObjectInfo(object obj)
        {
            Type t = obj.GetType();
            typeName = t.FullName;
            nameLenght = (short)typeName.Length;


            IntPtr ptr1 = new IntPtr();
            
            objectLenght = (short)Marshal.SizeOf(obj);//!!
            IntPtr ptr = Marshal.AllocHGlobal(objectLenght);
            Marshal.StructureToPtr(obj, ptr, false);

            objectData = new byte[objectLenght];

            Marshal.Copy(ptr, objectData, 0, objectLenght);
            Marshal.FreeHGlobal(ptr);

        }

        /// <summary>
        /// Задать через отформатированный массив байт
        /// </summary>
        /// <param name="data"></param>
        public ObjectInfo(byte[] data)
        {
            MemoryStream str = new MemoryStream(data);
            BinaryReader br = new BinaryReader(str);
            nameLenght = br.ReadInt16();
            objectLenght = br.ReadInt16();
            typeName = br.ReadString();
            objectData = br.ReadBytes(objectLenght);
        }

        /// <summary>
        /// Возвращает представление объекта в массиве байт
        /// </summary>
        /// <returns></returns>
        public byte[] GetMass()
        {

            MemoryStream str = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(str);
            bw.Write(nameLenght);
            bw.Write(objectLenght);
            bw.Write(typeName);
            bw.Write(objectData);
            return str.ToArray();
        }

        /// <summary>
        /// Задает представление объекта массивом байт
        /// </summary>
        /// <param name="data"></param>
        public void Set(byte[] data)
        {
            MemoryStream str = new MemoryStream(data);
            BinaryReader br = new BinaryReader(str);
            nameLenght = br.ReadInt16();
            objectLenght = br.ReadInt16();
            typeName = br.ReadString();
            objectData = br.ReadBytes(objectLenght);
        }

        /// <summary>
        /// Возвращает ссылку на объект
        /// </summary>
        /// <returns></returns>
        public object GetObject()
        {
            IntPtr ptr = Marshal.AllocHGlobal(objectLenght);
            Marshal.Copy(objectData, 0, ptr, objectLenght);
            Object obj = Marshal.PtrToStructure(ptr, this.GetObjType());
            return obj;
        }

        /// <summary>
        /// Возвращает тип сохраненного объекта
        /// </summary>
        /// <returns></returns>
        public Type GetObjType()
        {
            return Type.GetType(this.typeName);
        }

        /// <summary>
        /// Преобразование объекта в массив байт
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static byte[] ObjectToByteMass(object ob)
        {
            ObjectInfo oi = new ObjectInfo(ob);
            byte[] rez = oi.GetMass();
            return rez;
        }

        /// <summary>
        /// Преобразование массива в объект
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object ByteMassToObject(byte[] data)
        {
            ObjectInfo oi = new ObjectInfo(data);
            object rez = oi.GetObject();
            return rez;
        }


    }



    public class huh
    {
        public int [] ll;
        public huh(object o)
        {
             ll= new int[1];
        }
    }
}
