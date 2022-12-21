using System;
using System.Collections.Generic;

namespace Demo.UserData
{
    public class EventData : BaseUserData<EventData>
    {
        private List<int> currEventIds = new List<int>();
        /// <summary>
        /// 当前事件列表
        /// </summary>
        public List<int> CurrEventIds { get => currEventIds; }
        /// <summary>
        /// 当前事件变化
        /// </summary>
        public event Action OnCurrEventChanged;
        
        private List<int> finsihEventIds = new List<int>();
        /// <summary>
        /// 完成事件列表
        /// </summary>
        public List<int> FinsihEventIds { get => finsihEventIds; }
        /// <summary>
        /// 完成事件变化
        /// </summary>
        public event Action OnFinsihEventChanged;


        #region Add
        
        public void AddCurrEvent(int pEventId)
        {
            if (!currEventIds.Contains(pEventId))
                currEventIds.Add(pEventId);
            OnCurrEventChanged?.Invoke();
        }
        
        public void AddFinsihEvent(int pEventId)
        {
            if (currEventIds.Contains(pEventId))
                currEventIds.Remove(pEventId);
            if (!finsihEventIds.Contains(pEventId))
                finsihEventIds.Add(pEventId);
            OnFinsihEventChanged?.Invoke();
        }

        #endregion

        #region Remove

        public void RemoveEvent(int pEventId)
        {
            if (currEventIds.Contains(pEventId))
                currEventIds.Remove(pEventId);
            OnCurrEventChanged?.Invoke();
        }

        #endregion


        #region Check

        

        #endregion

    }
}