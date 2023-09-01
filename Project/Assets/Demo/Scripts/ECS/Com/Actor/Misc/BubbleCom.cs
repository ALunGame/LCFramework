using Demo.UI;
using LCECS.Core;
using LCMap;
using LCToolkit;
using LCUI;
using UnityEngine;

namespace Demo.Com
{
    public class BubbleCom : BaseCom
    {
        private BubbleDialogCom bubbleCom;
        private Transform bubbleRoot;

        protected override void OnAwake(Entity pEntity)
        {
            ActorDisplayCom displayCom = pEntity.GetCom<ActorDisplayCom>();
            if (displayCom != null)
            {
                displayCom.RegStateChange((stateName) =>
                {
                    OnDisplayGoChange(displayCom.StateGo);
                });
            }
        }

        private void OnDisplayGoChange(GameObject pStateGo)
        {
            bubbleRoot = pStateGo.transform.Find("BubbleRoot");
            if (bubbleRoot == null)
            {
                bubbleRoot = pStateGo.transform;
            }
            if (bubbleCom != null)
            {
                bubbleCom.transform.SetParent(bubbleRoot);
                bubbleCom.transform.ResetNoScale();
            }
        }

        public BubbleDialogCom GetBubbleCom()
        {
            if (bubbleCom != null)
            {
                return bubbleCom;
            }

            GameObject bubbleAssetGo = IAFramework.GameContext.Asset.LoadPrefab("BubbleDialogCom");
            GameObject bubbleTrans   = GameObject.Instantiate(bubbleAssetGo);
            bubbleTrans.GetComponent<Canvas>().worldCamera = Camera.main;
            bubbleTrans.transform.SetParent(bubbleRoot);
            bubbleTrans.transform.ResetNoScale();

            bubbleCom = new BubbleDialogCom();
            UIPanelCreater.CreateUIPanelTrans(bubbleCom, bubbleTrans.transform);
            return bubbleCom;
        }
    }
}