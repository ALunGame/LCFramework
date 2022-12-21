using System;

namespace Demo.UserData
{
    public class BaseUserData<T> where T : new()
    {
        private static readonly T instance = new T();
        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        protected BaseUserData() { }
    }
}