using LCToolkit;
using LCUI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Demo.UI
{
    public enum EmojiType
    {
        None,
    }

    public class BubbleDialogComModel : UIModel
    {
        /// <summary>
        /// 对话表情
        /// </summary>
        public BindableValue<EmojiType> emoji = new BindableValue<EmojiType>();

        /// <summary>
        /// 对话人名字
        /// </summary>
        public BindableValue<string> name = new BindableValue<string>();

        /// <summary>
        /// 对话内容
        /// </summary>
        public BindableValue<string> content = new BindableValue<string>();

        /// <summary>
        /// 正在播放动画
        /// </summary>
        public BindableValue<bool> isPlaying = new BindableValue<bool>();

        /// <summary>
        /// 动画
        /// </summary>
        public Tween contentTween;
    }

    public class BubbleDialogCom : UIPanel<BubbleDialogComModel>
    {
        private float preWorldShowTime = 0.02f;
        private UIComGlue<Transform> emojiTrans = new UIComGlue<Transform>("Box/Center/Emoji");
        private UIComGlue<Transform> dialogTrans = new UIComGlue<Transform>("Box/Center/Dailog");

        public override void OnShow()
        {
            BindModel.emoji.RegisterValueChangedEvent(RefreshEmoji);
            BindModel.content.RegisterValueChangedEvent(RefreshContent);
            BindModel.name.RegisterValueChangedEvent(RefreshTalkName);
            BindModel.isPlaying.RegisterValueChangedEvent(OnPlayingChange);
        }

        public override void OnHide()
        {
            TweenUtil.Clear(BindModel.contentTween);
            BindModel.contentTween = null;
        }

        private void RefreshEmoji(EmojiType emojiType)
        {
            if (emojiType == EmojiType.None)
            {
                emojiTrans.Com.gameObject.SetActive(false);
                BindModel.isPlaying.Value = false;
                return;
            }
            BindModel.isPlaying.Value = false;
        }

        private void RefreshContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                dialogTrans.Com.gameObject.SetActive(false);
                BindModel.isPlaying.Value = false;
                return;
            }
            BindModel.isPlaying.Value = true;
            Text textCom = dialogTrans.Com.Find("Box/MsgBox/TalkMsg/Content").GetComponent<Text>();
            textCom.text = "";
            BindModel.contentTween = textCom.DOText(content, content.Length * preWorldShowTime, true).SetEase(Ease.Linear);
            BindModel.contentTween.OnUpdate(() =>
            {
                RebuildLayout();
            });
            BindModel.contentTween.OnComplete(() =>
            {
                TweenUtil.DoDelayFunc(() =>
                {
                    RebuildLayout();
                });
                BindModel.isPlaying.Value = false;
            });
        }

        private void RebuildLayout()
        {
            dialogTrans.Com.Find("Box/MsgBox/TalkMsg").GetComponent<RectTransform>().RebuildLayout();
            dialogTrans.Com.Find("Box/TalkName").GetComponent<RectTransform>().RebuildLayout();
            dialogTrans.Com.Find("Box/TalkName").GetComponent<RectTransform>().RebuildLayout();
            dialogTrans.Com.Find("Box").GetComponent<RectTransform>().RebuildLayout();
        }

        private void RefreshTalkName(string name)
        {
            Text nameTextCom = dialogTrans.Com.Find("Box/TalkName/Name").GetComponent<Text>();
            if (string.IsNullOrEmpty(name))
            {
                nameTextCom.text = "?????";
            }
            else
            {
                nameTextCom.text = name;
            }
        }

        private void OnPlayingChange(bool isPlaying)
        {
            if (!isPlaying)
            {
                if (BindModel.contentTween != null)
                {
                    TweenUtil.Clear(BindModel.contentTween);
                    Text textCom = dialogTrans.Com.Find("Box/MsgBox/TalkMsg/Content").GetComponent<Text>();
                    textCom.text = BindModel.content.Value;
                    BindModel.contentTween = null;
                }
            }
        }
    } 
}
