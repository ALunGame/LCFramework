using System;
using System.Collections.Generic;
using Demo.Config;
using Demo.UserData;
using LCToolkit;
using LCUI;
using TMPro;
using UnityEngine;

namespace Demo.UI
{
    public class MainUI_EventPanelModel : UIModel
    {
    }
    
    public class MainUI_EventPanel : UIPanel<MainUI_EventPanelModel>
    {
        private UIComGlue<TextMeshProUGUI> titleTMP = new UIComGlue<TextMeshProUGUI>("Center/EventInfo/Title");
        private UIComGlue<Transform> eventInfoTrans = new UIComGlue<Transform>("Center/EventInfo");

        private UICacheGlue eventInfoCache = new UICacheGlue("Center/EventInfo/Prefab/InfoItem",
            "Center/EventInfo/Infos/Viewport/Content", true);

        private UIUserDataChangeGlue eventUserDataChange = new UIUserDataChangeGlue();


        public override void OnAwake()
        {
            titleTMP.Com.text = "当前事件";
        }

        public override void OnShow()
        {
            base.OnShow();
            RefreshEvents();
            eventUserDataChange.Register(EventData.Instance,RefreshEvents);
        }
        
        private void RefreshEvents()
        {
            eventInfoCache.RecycleAll();

            for (int i = 0; i < EventData.Instance.CurrEventIds.Count; i++)
            {
                int eventId = EventData.Instance.CurrEventIds[i];
                if (LCConfig.Config.EventCnf.TryGetValue(eventId,out EventCnf eventCnf))
                {
                    GameObject eventGo = eventInfoCache.Take();
                    TextUtil.SetText(eventGo.transform,"Title", eventCnf.name);
                    TextUtil.SetText(eventGo.transform,"Des", eventCnf.des);
                }
                else
                {
                    GameLocate.Log.LogError("事件出错，没有对应配置",eventId);
                }
            }
        }
        
    }
}