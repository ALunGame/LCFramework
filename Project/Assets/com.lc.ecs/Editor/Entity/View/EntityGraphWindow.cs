using LCECS.Core;
using LCNode;
using LCNode.View;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LCToolkit;
using UnityEditor.UIElements;

namespace LCECS.EntityGraph
{
    [CustomGraphWindow(typeof(EntityGraph))]
    public class EntityGraphWindow : BaseGraphWindow
    {
        protected override void BuildToolbar(ToolbarView toolbar)
        {
            base.BuildToolbar(toolbar);
            if (Application.isPlaying)
            {
                CreateRunningTimeToolbar(toolbar);
            }
        }

        private void CreateRunningTimeToolbar(ToolbarView toolbar)
        {
            EntityGraph graph = Graph as EntityGraph;
            //查看所有节点
            ToolbarButton btnSelEntity = new ToolbarButton()
            {
                text = graph.RunningTimeEntity == null ? "选择实体" : graph.RunningTimeEntity.Go.name,
                tooltip = "选择实体"
            };
            btnSelEntity.clicked += () =>
            {
                SelRunningTimeEntity(btnSelEntity);
            };
            toolbar.AddButtonToLeft(btnSelEntity);

        }

        private void SelRunningTimeEntity(ToolbarButton btnSelEntity)
        {
            EntityGraphAsset graphAsset = GraphAsset as EntityGraphAsset;
            int entityId = graphAsset.entityId;

            List<string> selStrs = new List<string>();
            List<Entity> entities = new List<Entity>();
            foreach (var item in ECSLocate.ECS.GetAllEntitys())
            {
                if (item.Value.Id == entityId)
                {
                    entities.Add(item.Value);
                    selStrs.Add(item.Value.Go.name);
                }
            }

            MiscHelper.Menu(selStrs, (int x) =>
            {
                EntityGraph graph = Graph as EntityGraph;
                graph.RunningTimeEntity = entities[x];
                btnSelEntity.text = graph.RunningTimeEntity.Go.name;
            });
        }
    }
}