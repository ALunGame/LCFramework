using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;

namespace LCSkill.BulletGraph
{
    [CustomNodeView(typeof(Bullet_HitAddBuffNode))]
    public class Bullet_HitAddBuffNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Bullet_HitAddBuffNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Buff";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Bullet_HitAddBuffNode node = Model as Bullet_HitAddBuffNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
            BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + node.addBuff.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}