using System.Collections.Generic;
using LCSkill.Help;
using LCSkill.Timeline;
using LCToolkit;
using LCToolkit.Element;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Common
{
    [TimlineClipElement(typeof(RaycastClip))]
    public class RaycastClip_Element : Clip_Element<RaycastClip>
    {
        private GameObject preViewRaycastGo = null;
        
        protected override void OnInit()
        {
            // Model.TitleName = Model.effetGo.ObjName;
            // Model.effetGo.OnObjectAssetChange += OnEffectChange;
            // Model.OnEndFrameChange += OnEndFrameChange;
            //
            // RootElement.CreateGUIFunc(OnGui);
        }

        public override void OnClear()
        {
            // Model.effetGo.OnObjectAssetChange -= OnEffectChange;
            // Model.OnEndFrameChange -= OnEndFrameChange;
            // if (preViewRaycastGo != null)
            // {
            //     GameObject.DestroyImmediate(preViewRaycastGo);
            // }
        }

        private void OnEffectChange()
        {
            // Model.TitleName = Model.effetGo.ObjName;
            // if (preViewRaycastGo != null)
            // {
            //     GameObject.DestroyImmediate(preViewRaycastGo);
            // }
        }
        
        private void OnEndFrameChange(int pValue)
        {
        }
        
        private void OnGui()
        {
        }
        
        protected override void OnPlaying(float pPlayTime)
        {
            UpdateRaycastGo();
        }

        protected override void OnExitPlaying()
        {
            if (preViewRaycastGo != null)
            {
                GameObject.DestroyImmediate(preViewRaycastGo);
                preViewRaycastGo = null;
            }
        }

        private void UpdateRaycastGo()
        {
            //创建
            if (preViewRaycastGo == null)
                preViewRaycastGo = new GameObject("Raycast");

            // Model.followPath.SetRootGo(window.PreviewGo);
            // Model.followPath.UpdateObj();
            // GameObject followGo = Model.followPath.Go;
            // if (followGo != null)
            // {
            //     preViewRaycastGo.transform.SetParent(followGo.transform);
            //     preViewRaycastGo.transform.localPosition = Model.followOffset;
            // }
            
            //绘制
            SkillShapeDrawCom drawCom = preViewRaycastGo.transform.GetOrAddCom<SkillShapeDrawCom>();
            drawCom.DrawShape = Model.areaShape;
            drawCom.DrawShape.Translate(window.PreviewGo.transform.position.ToVector2() + Model.areaPos);
        }
    }
}