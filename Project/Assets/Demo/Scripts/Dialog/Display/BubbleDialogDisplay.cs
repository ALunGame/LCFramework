using LCDialog;
using LCECS.Core;
using LCMap;
using System.Collections.Generic;
using LCECS;
using Demo.Com;
using Demo.UI;
using LCUI;

namespace Demo.Dialog
{
    public class BubbleDialogDisplay : IDialogDisplayServer
    {
        private Dictionary<string, List<BubbleDialogCom>> bubbleComDict = new Dictionary<string, List<BubbleDialogCom>>();

        public void OnCreateDialog(DialogObj dialog, List<string> actorUids)
        {
            
        }

        public void OnPlayDialog(DialogObj dialog, DialogStepModel stepModel)
        {
            //隐藏当前的
            if (bubbleComDict.ContainsKey(dialog.Uid))
            {
                List<BubbleDialogCom> coms = bubbleComDict[dialog.Uid];
                for (int i = 0; i < coms.Count; i++)
                {
                    coms[i].Hide();
                }
                bubbleComDict.Remove(dialog.Uid);
            }

            //展示新的
            List<ActorObj> actors = new List<ActorObj>();
            if (stepModel.speakers != null && stepModel.speakers.Count > 0)
            {
                for (int i = 0; i < stepModel.speakers.Count; i++)
                {
                    int actorId = stepModel.speakers[i];
                    actors.AddRange(MapLocate.Map.GetActors(actorId));
                }
            }
            else
            {
                if (stepModel.speakerType == SpeakerType.Sponsor)
                {
                    actors.Add(dialog.Sponsor);
                }
                else if (stepModel.speakerType == SpeakerType.Target)
                {
                    actors.AddRange(dialog.Targets);
                }
            }

            List<BubbleDialogCom> bubbleComs = new List<BubbleDialogCom>();
            bool hasPlayer = false;
            for (int i = 0; i < actors.Count; i++)
            {
                ActorObj actor = actors[i];

                Entity entity = ECSLocate.ECS.GetEntity(actor.Uid);
                BubbleCom bubbleCom = entity.GetCom<BubbleCom>();
                BubbleDialogCom bubbleDialogCom = bubbleCom.GetBubbleCom();

                BubbleDialogComModel bubbleDialogComModel = bubbleDialogCom.BindModel;
                bubbleDialogComModel.content.Value = stepModel.content;
                bubbleDialogCom.Show();
                bubbleComs.Add(bubbleDialogCom);

                if (actor.Equals(MapLocate.Map.PlayerActor))
                    hasPlayer = true;
            }
            bubbleComDict.Add(dialog.Uid, bubbleComs);

            if (hasPlayer || (stepModel.disposes != null && stepModel.disposes.Count > 0))
            {
                BubbleDialogChoosePanelModel model = UILocate.UI.GetPanelModel<BubbleDialogChoosePanelModel>(UIPanelId.BubbleDialogChoosePanel);
                model.dialogUid.Value = dialog.Uid;
                model.stepModel.Value = stepModel;
                model.skipCallBack = (uid) =>
                {
                    List<BubbleDialogCom> coms = bubbleComDict[uid];
                    for (int i = 0; i < coms.Count; i++)
                    {
                        if (!coms[i].BindModel.isPlaying.Value)
                        {
                            return false;
                        }
                        coms[i].BindModel.isPlaying.Value = false;
                    }
                    return true;
                };
                UILocate.UI.Show(UIPanelId.BubbleDialogChoosePanel);
            }
        }

        public void OnClickDispose(DialogObj dialog, int disposeId)
        {
            
        }

        public void OnCloseDialog(DialogObj dialog)
        {
            List<BubbleDialogCom> coms = bubbleComDict[dialog.Uid];
            for (int i = 0; i < coms.Count; i++)
            {
                coms[i].Hide();
            }
            bubbleComDict.Remove(dialog.Uid);
            BubbleDialogChoosePanelModel model = UILocate.UI.GetPanelModel<BubbleDialogChoosePanelModel>(UIPanelId.BubbleDialogChoosePanel);
            if (model.dialogUid.Value == dialog.Uid)
            {
                UILocate.UI.Hide(UIPanelId.BubbleDialogChoosePanel);
            }
        }
    }
}