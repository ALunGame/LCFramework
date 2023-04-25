using System.Collections.Generic;
using System.Reflection;

namespace LCMap
{
    public static partial class ActorDelegateNames
    {
        public static List<string> AllDelegateNames = new List<string>();
        static ActorDelegateNames()
        {
            FieldInfo[] fieldInfos = typeof(ActorDelegateNames).GetFields(BindingFlags.Public | BindingFlags.Static |
                                                                          BindingFlags.FlattenHierarchy);
            foreach (FieldInfo info in fieldInfos)
            {
                if (info != null && info.IsLiteral && !info.IsInitOnly && info.FieldType == typeof(string))
                {
                    string constValue = info.GetValue(null).ToString();
                    AllDelegateNames.Add(constValue);
                }
            }
        }
        
        /// <summary>
        /// 当演员位置改变
        /// </summary>
        public const string OnPosChange = "OnPosChange";
    }
    
    public delegate void OnActorChange(Actor pActor);
    
    public class ActorDelegate
    {
        private Actor actor;
        private Dictionary<string, OnActorChange> delegateDict = new Dictionary<string, OnActorChange>();

        public ActorDelegate(Actor pActor)
        {
            actor = pActor;
            foreach (string delegateName in ActorDelegateNames.AllDelegateNames)
            {
                delegateDict.Add(delegateName,null);
            }
        }

        public void Clear()
        {
            delegateDict.Clear();
            delegateDict = null;
        }

        /// <summary>
        /// 执行委托
        /// </summary>
        /// <param name="pDelegateName"></param>
        /// <param name="pActor"></param>
        public void ExecuteDelegate(string pDelegateName)
        {
            if (!delegateDict.ContainsKey(pDelegateName))
            {
                ActorLocate.Log.LogError("执行演员委托失败，没有对应委托名",pDelegateName);
                return;
            }
            delegateDict[pDelegateName]?.Invoke(actor);
        }

        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="pDelegateName"></param>
        /// <param name="pDelegate"></param>
        public void Register(string pDelegateName, OnActorChange pDelegate)
        {
            if (delegateDict.ContainsKey(pDelegateName))
            {
                ActorLocate.Log.LogError("注册演员委托失败，没有对应委托名",pDelegateName);
                return;
            }
            delegateDict[pDelegateName] += pDelegate;
        }

        /// <summary>
        /// 清除委托
        /// </summary>
        /// <param name="pDelegateName"></param>
        /// <param name="pDelegate"></param>
        public void Remove(string pDelegateName, OnActorChange pDelegate)
        {
            if (delegateDict.ContainsKey(pDelegateName))
            {
                ActorLocate.Log.LogError("移除演员委托失败，没有对应委托名",pDelegateName);
                return;
            }
            delegateDict[pDelegateName] -= pDelegate;
        }
    }
}