﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LCHelp
{
    /// <summary>
    /// 深拷贝
    /// </summary>
    public class LCDeepCopy
    {
        public static T DeepCopyByBin<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
    }
}
