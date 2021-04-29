using System;
using UnityEngine;

namespace LCUI
{
    public enum UIShowMode
    {
        DoNothing,                  //啥事都不做
        HideOther,                  //关闭其他界面
        NoNeedBack,                 //不加入导航
        NoNeedBack_HideOther, 	    //不加入导航，同时显示时隐藏其他界面
    }

    public enum UILayer
    {
        Base,
        First,
        Second,
        Three,
        Top,
        Dynamic,
    }

    [Serializable]
    public class UIPanelInfo
    {
        public UIPanelId PanelId;
        public UILayer Layer;
        public RectTransform PanelRect;
    }

    //界面Id
    public enum UIPanelId
    {
        Login,
        Fight,
        FightUpWorld,           //战斗飘字
    }
}
