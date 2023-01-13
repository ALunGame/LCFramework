using System.Collections.Generic;
using Config;
using Demo.Com;
using DG.Tweening;
using LCMap;
using UnityEngine;

namespace Demo
{
    /// <summary>
    /// 采集物品交互
    /// </summary>
    public class CollectInteractive : ActorInteractive
    {
        public static Dictionary<int, int> CollectWeapon = new Dictionary<int, int>()
        {
            {401,10},
        };


        private int _type = (int)InteractiveType.Collect;
        public override int Type { get => _type;}

        protected override InteractiveState OnExecute(Actor executeActor, params object[] pParams)
        {
            int itemId = (int)pParams[0];
            
            //1，产出物品
            actor.GetCom(out OutputItemCom outputItemCom);
            ItemInfo outputItem = outputItemCom.GetOutputInfo(itemId);
            
            //2，采集物品
            executeActor.GetCom(out BagCom exbagCom);
            exbagCom.AddItem(outputItem.itemId, outputItem.itemCnt);
            
            //3,武器表现
            if (CollectWeapon.ContainsKey(itemId))
            {
                int weaponId = CollectWeapon[itemId];
                WeaponCom weaponCom = executeActor.GetCom<WeaponCom>();
                weaponCom.UseWeapon(weaponId, () =>
                {
                    //4,抖动
                    actor.GetStateGo().transform.DOComplete(false);
                    actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);
                    Success();
                });
                return InteractiveState.Executing; 
            }

            //4,抖动
            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);

            return InteractiveState.Success;
        }
    }
}
