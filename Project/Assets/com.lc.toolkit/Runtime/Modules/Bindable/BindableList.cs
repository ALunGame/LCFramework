using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace LCToolkit
{
    /// <summary>
    /// 绑定集合类型
    /// </summary>
    public class BindableList<T> : BindableValue<List<T>>, IEnumerable<T>, IList<T>
    {
        #region Fields

        private event Action<T> onAdded;
        private event Action<int,T> onInserted;
        private event Action<T> onRemoved;
        private event Action<T> onItemChanged;
        private event Action onClear;

        #endregion

        #region Override

        public T this[int index]
        {
            get { return Value[index]; }
            set { SetItem(index, value); }
        }

        public int Count
        {
            get { return Value.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            return Value.IndexOf(item);
        }

        public void Add(T item)
        {
            Value.Add(item);
            onAdded?.Invoke(item);
        }

        public void Insert(int index, T item)
        {
            Value.Insert(index, item);
            onInserted?.Invoke(index,item);
        }

        public bool Remove(T item)
        {
            if (Value.Remove(item))
            {
                onRemoved?.Invoke(item);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            T v = Value[index];
            Remove(Value[index]);
        }

        public bool Contains(T item)
        {
            return Value.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Value.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            for (int i = 0; i < Value.Count; i++)
            {
                RemoveAt(i);
            }
            Value.Clear();
            onClear?.Invoke();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        } 

        #endregion

        public override void ClearChangedEvent()
        {
            base.ClearChangedEvent();
            onAdded = null;
            onInserted = null;
            onRemoved = null;
            onItemChanged = null;
            onClear = null;
        }

        #region Private

        protected void SetItem(int index, T item)
        {
            Value[index] = item;
            onItemChanged?.Invoke(item);
        }

        #endregion

        #region Public

        /// <summary>
        /// 娉ㄥ唽娣诲姞
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void RegisterAdd(Action<T> onItemAdd)
        {
            this.onAdded += onItemAdd;
        }

        /// <summary>
        /// 鍒犻櫎娣诲姞
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void UnregisterAdd(Action<T> onItemAdd)
        {
            this.onAdded -= onItemAdd;
        }

        /// <summary>
        /// 娉ㄥ唽鎻掑叆
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void RegisterInserted(Action<T> onItemAdd)
        {
            this.onAdded += onItemAdd;
        }

        /// <summary>
        /// 鍒犻櫎鎻掑叆
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void UnregisterInserted(Action<T> onItemAdd)
        {
            this.onAdded -= onItemAdd;
        }

        /// <summary>
        /// 娉ㄥ唽绉婚櫎
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void RegisterRemove(Action<T> onItemAdd)
        {
            this.onAdded += onItemAdd;
        }

        /// <summary>
        /// 鍒犻櫎绉婚櫎
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void UnregisterRemove(Action<T> onItemAdd)
        {
            this.onAdded -= onItemAdd;
        }

        /// <summary>
        /// 娉ㄥ唽鏀瑰彉
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void RegisterItemChange(Action<T> onItemAdd)
        {
            this.onAdded += onItemAdd;
        }

        /// <summary>
        /// 鍒犻櫎鏀瑰彉
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void UnregisterItemChange(Action<T> onItemAdd)
        {
            this.onAdded -= onItemAdd;
        }


        /// <summary>
        /// 娉ㄥ唽娓呯┖
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void RegisterClear(Action<T> onItemAdd)
        {
            this.onAdded += onItemAdd;
        }

        /// <summary>
        /// 鍒犻櫎娓呯┖
        /// </summary>
        /// <param name="onItemAdd"></param>
        public void UnregisterClear(Action<T> onItemAdd)
        {
            this.onAdded -= onItemAdd;
        }

        #endregion
    }
}