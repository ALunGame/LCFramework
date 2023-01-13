using System.Collections;
using UnityEngine;
using LCUI;
using LCToolkit;
using LCDialog;
using UnityEngine.UI;
using System;

namespace Demo.UI
{
    public class BubbleDialogChoosePanelModel : UIModel
    {
        public BindableValue<string> dialogUid = new BindableValue<string>();
        public BindableValue<DialogStepModel> stepModel = new BindableValue<DialogStepModel>();

        public Func<string,bool> skipCallBack = null;
    }
    
    public class BubbleDialogChoosePanel : UIPanel<BubbleDialogChoosePanelModel>
    {
        private UIComGlue<Transform> chooseTrans = new UIComGlue<Transform>("Center/Choose");
        private UICacheGlue chooseItem = new UICacheGlue("Center/Choose/Prefab/DisposeBtn", "Center/Choose/ChooseRoot/List", true, true);

        private UIComGlue<Transform> skipTrans = new UIComGlue<Transform>("Center/Skip");

        public override void OnAwake()
        {
            BtnUtil.SetClick(skipTrans.Com, "SkipBg", () =>
            {
                if (BindModel.skipCallBack != null)
                {
                    Func<string, bool> func = BindModel.skipCallBack;
                    BindModel.skipCallBack = null;
                    if (!func(BindModel.dialogUid.Value))
                    {
                        DialogLocate.Dialog.PlayNext(BindModel.dialogUid.Value);
                    }
                }
                else
                {
                    DialogLocate.Dialog.PlayNext(BindModel.dialogUid.Value);
                }
            });
        }

        public override void OnShow()
        {
            BindModel.dialogUid.RegisterValueChangedEvent(OnDialogUidChange);
            BindModel.stepModel.RegisterValueChangedEvent(OnDialogStepChange);
        }

        public override void OnHide()
        {
            BindModel.skipCallBack = null;
            Debug.LogError("OnHide>>>>");
        }

        private void OnDialogUidChange(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                UILocate.UI.Hide(UIPanelDef.BubbleDialogChoosePanel);
                return;
            }
        }

        private void OnDialogStepChange(DialogStepModel stepModel)
        {
            chooseTrans.Com.gameObject.SetActive(false);
            skipTrans.Com.gameObject.SetActive(false);
            //没有选项就是跳过
            if (stepModel.disposes == null || stepModel.disposes.Count <= 0)
            {
                skipTrans.Com.gameObject.SetActive(true);
            }
            else
            {
                chooseItem.RecycleAll();
                for (int i = 0; i < stepModel.disposes.Count; i++)
                {
                    CreateDisposeItem(stepModel.disposes[i]);
                }
                chooseTrans.Com.gameObject.SetActive(true);
            }
        }

        private void CreateDisposeItem(DialogDisposeModel disposeModel)
        {
            GameObject go = chooseItem.Take();
            go.transform.Find("Content").GetComponent<Text>().text = disposeModel.content;
            BtnUtil.SetClick(go.transform, "", () =>
            {
                DialogLocate.Dialog.ClickDispose(BindModel.dialogUid.Value, disposeModel.id);
            });
        }
    }
}