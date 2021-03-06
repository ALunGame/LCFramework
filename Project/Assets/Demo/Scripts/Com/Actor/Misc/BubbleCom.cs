using Demo.UI;
using LCECS.Core;
using LCLoad;
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

        protected override void OnInit(GameObject go)
        {
            ActorObj actorObj = go.GetComponent<ActorObj>();
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj);
        }

        private void OnDisplayGoChange(ActorObj actorObj)
        {
            bubbleRoot = actorObj.GetStateGo().transform.Find("BubbleRoot");
            if (bubbleRoot == null)
            {
                bubbleRoot = actorObj.GetStateGo().transform;
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

            GameObject bubbleAssetGo = LoadHelper.LoadPrefab("BubbleDialogCom");
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