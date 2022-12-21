using System.Collections.Generic;

namespace LCToolkit
{
    public static class CSharpEx
    {
        /// <summary>
        /// 列表是否合法
        /// </summary>
        /// <param name="pList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsLegal<T>(this List<T> pList)
        {
            if (pList == null || pList.Count <= 0)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 堆栈是否合法
        /// </summary>
        /// <param name="pStack"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsLegal<T>(this Stack<T> pStack)
        {
            if (pStack == null || pStack.Count <= 0)
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 队列是否合法
        /// </summary>
        /// <param name="pStack"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsLegal<T>(this Queue<T> pQueue)
        {
            if (pQueue == null || pQueue.Count <= 0)
            {
                return false;
            }

            return true;
        }
    }
}