using LCConfig;
using LCJson;
using LCNode;
using LCNode.View;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCMap
{
    [CustomNodeView(typeof(Map_ActorNode))]
    public class Map_ActorNodeView : BaseNodeView
    {
        private static List<ActorCnf> actorCnfs = null;
        private static Dictionary<int, MapInfo> mapCnfs = null;

        private Button btnAddActor;
        private Button btnClearActor;
        private Dictionary<int, Button[]> btnActorList = new Dictionary<int, Button[]>();

        public Map_ActorNodeView()
        {
            if (actorCnfs == null)
            {
                actorCnfs = ConfigSetting.GetConfigAssets<ActorCnf>();
            }
            if (mapCnfs == null)
            {
                mapCnfs = new Dictionary<int, MapInfo>();
                List<ED_MapCom> maps = MapSetting.GetAllMaps();
                for (int i = 0; i < maps.Count; i++)
                {
                    string filePath = MapSetting.GetMapModelSavePath(maps[i].mapId.ToString());
                    string jsonStr = IOHelper.ReadText(filePath);
                    MapInfo mapModel = JsonMapper.ToObject<MapInfo>(jsonStr);
                    mapCnfs.Add(maps[i].mapId, mapModel);
                }
            }

            btnAddActor = new Button();
            btnAddActor.text = "添加演员";
            controlsContainer.Add(btnAddActor);

            btnClearActor = new Button();
            btnClearActor.text = "清空演员";
            outputContainer.Add(btnClearActor);
        }

        protected override void OnInitialized()
        {
            DrawActors();
            btnAddActor.clicked += OnClickAddActor;
            btnClearActor.clicked += OnClickClearActor;
        }

        protected override void OnDrawerValuesChange()
        {
            DrawActors();
        }

        private void DrawActors()
        {
            Map_ActorNode node = Model as Map_ActorNode;
            foreach (var item in btnActorList)
            {
                RemoveActorItemBtn(item.Key);
            }
            btnActorList.Clear();
            for (int i = 0; i < node.actorIds.Count; i++)
            {
                int actorId = node.actorIds[i];
                DrawActorItem(i,actorId);
            }
        }

        private void DrawActorItem(int index,int actorId)
        {
            Button selActorBtn;
            Button delActorBtn;
            if (!btnActorList.ContainsKey(actorId))
            {
                selActorBtn = new Button();
                selActorBtn.text = GetActorName(actorId);
                selActorBtn.clicked += () =>
                {
                    OnClickSelActor(index,actorId, selActorBtn);
                };
                inputContainer.Add(selActorBtn);

                delActorBtn = new Button();
                delActorBtn.text = "删除";
                delActorBtn.clicked += () =>
                {
                    OnClickRemoveActor(actorId);
                };
                outputContainer.Add(delActorBtn);

                btnActorList.Add(actorId, new Button[2]);
                btnActorList[actorId][0] = selActorBtn;
                btnActorList[actorId][1] = delActorBtn;
            }
            else
            {
                Button[] btns = btnActorList[actorId];
                btns[0].text = GetActorName(actorId);
            }
        }

        private void RemoveActorItemBtn(int actorId)
        {
            if (!btnActorList.ContainsKey(actorId))
            {
                return;
            }
            Button[] buttons = btnActorList[actorId];
            inputContainer.Remove(buttons[0]);
            outputContainer.Remove(buttons[1]);
        }

        private void OnClickRemoveActor(int actorId)
        {
            Map_ActorNode node = Model as Map_ActorNode;
            for (int i = 0; i < node.actorIds.Count; i++)
            {
                if (node.actorIds[i] == actorId)
                {
                    node.actorIds.RemoveAt(i);
                }
            }
            DrawActors();
        }

        private void OnClickSelActor(int index,int actorId, Button selActorBtn)
        {
            Map_ActorNode node = Model as Map_ActorNode;
            List<int> actors = GetCurrActors();
            for (int i = 0; i < actors.Count; i++)
            {
                if (actors[i] == actorId)
                {
                    actors.RemoveAt(i);
                }
            }
            List<string> actorNames = new List<string>();
            for (int i = 0; i < actors.Count; i++)
            {
                actorNames.Add(GetActorName(actors[i]));
            }

            MiscHelper.Menu(actorNames, (int x) =>
            {
                int newActorId = actors[x];
                if (newActorId != actorId)
                {
                    node.actorIds[index] = newActorId;
                    selActorBtn.text = GetActorName(newActorId);
                }
            });
        }

        private void OnClickAddActor()
        {
            Map_ActorNode node = Model as Map_ActorNode;

            int leftActorId = -1;
            List<int> actors = GetCurrActors();
            for (int i = 0; i < actors.Count; i++)
            {
                int actorId = actors[i];
                if (!node.actorIds.Contains(actorId))
                {
                    leftActorId = actorId;
                    break;
                }
            }
            if (leftActorId == -1)
            {
                Debug.LogError($"添加演员出错，所有演员添加完了");
                return;
            }
            node.actorIds.Add(leftActorId);
            DrawActors();
        }

        private void OnClickClearActor()
        {
            Map_ActorNode node = Model as Map_ActorNode;
            node.actorIds.Clear();
            DrawActors();
        }

        private bool CheckActorIdIsSafe(int actorId)
        {
            Map_ActorNode node = Model as Map_ActorNode;
            List<int> actors = GetCurrActors();
            return actors.Contains(actorId);
        }

        private List<int> GetCurrActors()
        {
            Map_ActorNode node = Model as Map_ActorNode;
            List<int> actors = new List<int>();
            if (node.mapId == MapNodeId.所有地图)
            {
                for (int i = 0; i < actorCnfs.Count; i++)
                {
                    ActorCnf actorCnf = actorCnfs[i];
                    actors.Add(actorCnf.id);
                }
            }
            else
            {
                MapInfo mapModel = mapCnfs[(int)node.mapId];
                actors.Add(mapModel.mainActor.id);
                for (int i = 0; i < mapModel.areas.Count; i++)
                {
                    AreaInfo area = mapModel.areas[i];
                    for (int j = 0; j < area.actors.Count; j++)
                    {
                        int tId = area.actors[j].id;
                        if (!actors.Contains(tId))
                        {
                            actors.Add(tId);
                        }
                    }
                }
            }
            return actors;
        }

        private string GetActorName(int actorId)
        {
            for (int i = 0; i < actorCnfs.Count; i++)
            {
                ActorCnf actorCnf = actorCnfs[i];
                if (actorCnf.id == actorId)
                {
                    return actorCnf.name;
                }
            }
            return "Unknown";
        }
    } 
}
