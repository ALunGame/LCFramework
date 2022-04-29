using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LCToolkit
{
    public class Utility
    {

        

    }

    /// <summary>
    /// 深拷贝
    /// </summary>
    /// <typeparam name="TIn">输入类型</typeparam>
    /// <typeparam name="TOut">输出类型</typeparam>
    public static class DeepCopy
    {
        //private static readonly Func<TIn, TOut> cache = GetFunc();
        //private static Func<TIn, TOut> GetFunc()
        //{
        //    ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
        //    List<MemberBinding> memberBindingList = new List<MemberBinding>();

        //    foreach (var item in typeof(TOut).GetProperties())
        //    {
        //        if (!item.CanWrite) continue;
        //        MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
        //        MemberBinding memberBinding = Expression.Bind(item, property);
        //        memberBindingList.Add(memberBinding);
        //    }

        //    MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
        //    Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

        //    return lambda.Compile();
        //}

        //public static TOut Trans(TIn tIn)
        //{
        //    return cache(tIn);
        //}

        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static object CloneObject(object objSource)
        {

            //步骤1:获取源对象的类型并创建该类型的新实例
            Type typeSource = objSource.GetType();

            object objTarget = Activator.CreateInstance(typeSource);

            //步骤2:获取源对象类型的所有属性
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //步骤3:将所有源属性分配给标记对象的属性
            foreach (PropertyInfo property in propertyInfo)
            {

                //检查属性是否可以写入
                if (property.CanWrite)
                {

                    //步骤4:检查属性类型是值类型，枚举类型还是字符串类型
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {

                        property.SetValue(objTarget, property.GetValue(objSource, null), null);

                    }
                    //其他属性类型是对象/复杂类型，因此需要递归调用此方法，直到到达树的末尾
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);

                        }
                        else
                        {
                            property.SetValue(objTarget, CloneObject(objPropertyValue), null);
                        }

                    }

                }

            }
            return objTarget;
        }
    }
}
