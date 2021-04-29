using LCHelp;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.Json;

namespace LCSkill
{
    public class SkillEditorHelp:MonoBehaviour
    {
        public Rect SelfSkillRect = new Rect();
        public Rect OtherSkillRect = new Rect();

        private void OnDrawGizmos()
        {
            EDGizmos.DrawRect(SelfSkillRect, Color.green);
            EDGizmos.DrawRect(OtherSkillRect, Color.red);
        }
    }

    public class SkillEditorWindow: EditorWindow
    {
        private const string JsonPath = "/Resources/Config/SkillJson.txt";

        private static SkillList Json = new SkillList();
        private static Animator SelAnim = null;
        private static GameObject SelGo = null;
        private static SkillEditorHelp SelGoHelp = null;
        private static List<AnimationClip> RunningClip = new List<AnimationClip>();

        public static void OpenWindow(Animator anim)
        {
            SelGo = null;
            SelAnim = null;
            RunningClip.Clear();

            SkillEditorWindow window = GetWindow<SkillEditorWindow>("技能编辑器");
            window.minSize = new Vector2(800, 600);

            SelGo = anim.gameObject;
            SelAnim = anim;
            SelGoHelp = SelGo.transform.parent.gameObject.AddComponent<SkillEditorHelp>();

            if (EDTool.CheckFileInPath(Application.dataPath + JsonPath))
            {
                string dataJson = EDTool.ReadText(Application.dataPath + JsonPath);
                Json = JsonMapper.ToObject<SkillList>(dataJson);
            }
        }

        //技能开始播放时间
        private float SkillPlayTime = 0;
        private bool IsPlaying = false;
        private Dictionary<string, bool> ImpactInfoTog = new Dictionary<string, bool>();

        private void OnEnable()
        {
            ImpactInfoTog.Add("Data1", false);
            ImpactInfoTog.Add("Data2", false);

            ImpactInfoTog.Add("Anim1", false);
            ImpactInfoTog.Add("Anim2", false);

            ImpactInfoTog.Add("Effect1", false);
            ImpactInfoTog.Add("Effect2", false);

            ImpactInfoTog.Add("Audio1", false);
            ImpactInfoTog.Add("Audio2", false);
        }

        private void OnGUI()
        {
            EDLayout.CreateHorizontal("", 800, 600, () =>
            {
                ShowLeft(200, 600);
                ShowRight(600, 600);
            });
        }

        private void Update()
        {
            SliderAnim();
            DrawSkillArea();
        }

        private void OnDestroy()
        {
            Save();
            Json = new SkillList();
            LCSkillLocate.ReLoadData = true;

            if (SelGoHelp != null)
            {
                DestroyImmediate(SelGoHelp);
            }
        }

        #region 技能播放
        private void SliderAnim()
        {
            if (SelGo == null)
                return;
            AnimationClip clip = GetAnimClipByTime(SkillPlayTime, out float leftTime,true);
            if (clip == null)
                return;
            clip.SampleAnimation(SelGo, leftTime);
        }

        private AnimationClip GetAnimClipByTime(float playTime, out float animLeftTime,bool isSelf)
        {
            animLeftTime = playTime;
            if (SelSkill == null)
            {
                return null;
            }

            for (int i = 0; i < SelSkill.ImpactList.Count; i++)
            {
                SkillImpact impact      = SelSkill.ImpactList[i];
                SkillSelfImpactInfo info    = null;
                SkillOtherImpactInfo other = null;
                if (isSelf)
                {
                    if (impact.SelfInfo != null)
                    {
                        info = impact.SelfInfo;
                    }
                }
                else
                {
                    if (impact.OtherInfo != null)
                    {
                        other = impact.OtherInfo;
                    }
                }
                if (info != null && info.Anim!=null)
                {
                    AnimationClip clip  = GetAnimatorClipByName(SelAnim, info.Anim.AnimName);
                    if (clip!=null)
                    {
                        float animStartTime = (float)impact.Time;
                        float animEndTime   = (float)impact.Time + clip.length;
                        if (playTime >= animStartTime && playTime <= animEndTime)
                        {
                            animLeftTime = playTime - animStartTime;
                            return clip;
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region 技能范围显示

        //绘制技能作用范围
        private void DrawSkillArea()
        {
            if (SelImpact==null)
            {
                return;
            }

            //自身效果
            if (SelImpact.OtherInfo!=null)
            {
                SkillOtherImpactInfo impactInfo = SelImpact.OtherInfo;
                SelGoHelp.OtherSkillRect.position = (Vector3)LCConvert.StrChangeToObject(impactInfo.Pos, typeof(Vector3).FullName);
                SelGoHelp.OtherSkillRect.size = (Vector3)LCConvert.StrChangeToObject(impactInfo.Area, typeof(Vector3).FullName);
            }
        }

        #endregion

        #region View
        private SkillJson SelSkill;
        private SkillImpact SelImpact;

        #region 左方
        private Vector2 LeftPos = Vector2.zero;
        private void ShowLeft(float width, float height)
        {
            EDLayout.CreateScrollView(ref LeftPos, "", width, height, () =>
            {
                for (int i = 0; i < Json.List.Count; i++)
                {
                    SkillJson skill = Json.List[i];
                    if (skill.DecStr==SelGo.transform.parent.name)
                    {
                        EDColor.DrawColorArea(SelSkill != null && SelSkill.Id == skill.Id ? Color.green : Color.white, () =>
                        {
                            EDButton.CreateBtn(string.Format("{0} ({1})", skill.Id, skill.DecStr), 180, 25, () =>
                            {
                                SelSkillChange(skill);
                            });
                        });
                    }
                }

                //添加技能
                EDButton.CreateBtn("新建技能", 180, 30, () =>
                {
                    EDPopPanel.PopWindow("输入技能Id>>>>>>", (x) =>
                    {
                        if (CheckContainSkill(x))
                        {
                            Debug.LogError("技能Id重复>>>>>>>>>>>>" + x);
                            return;
                        }
                        int skillId = int.Parse(x);
                        SkillJson json = new SkillJson
                        {
                            Id = skillId,
                            DecStr = SelGo.transform.parent.name,
                        };
                        Json.List.Add(json);
                    });
                });

                //重载数据
                EDButton.CreateBtn("重载数据", 180, 30, () =>
                {
                    Save();
                    LCSkillLocate.ReLoadData = true;
                });
            });
        }
        #endregion

        #region 右方

        private bool ShowFilter = false;
        private bool ShowExData = false;
        private Vector2 Pos01 = Vector2.zero;

        private void ShowRight(float width, float height)
        {
            if (SelSkill==null)
            {
                return;
            }
            EDLayout.CreateVertical("box", width, height, () =>
            {
                //基础信息
                ShowSkillInfo(width - 10, 70);

                EDLayout.CreateScrollView(ref Pos01, "", width-10, height - 100, (float wd,float he) =>
                {
                    //效果列表
                    float hei = SelSkill.ImpactList.Count <= 0 ? 25 : SelSkill.ImpactList.Count * 25;
                    ShowSkillImpactLine(wd - 50, hei);

                    //效果
                    ShowSkillImpact(wd-10, he- hei);
                });
                
            });
        }

        private void ShowSkillInfo(float width,float height)
        {
            if (SelSkill==null)
            {
                return;
            }
            SelSkill.ContinueTime = GetSkillContinueTime(SelSkill);
            EDLayout.CreateVertical("", width, height, () =>
            {
                EDLayout.CreateHorizontal("box", width, 30, () =>
                {
                    EditorGUILayout.LabelField("Id：" + SelSkill.Id, GUILayout.Width(140), GUILayout.Height(25));
                    EditorGUILayout.LabelField("持续时间：" + SelSkill.ContinueTime, GUILayout.Width(140), GUILayout.Height(25));

                    EDButton.CreateBtn("删除此技能", 100, 25, () =>
                    {
                        Json.List.Remove(SelSkill);
                        SelSkill = null;
                        SelImpact = null;
                    });

                    EDButton.CreateBtn("添加技能效果", 100, 25, () =>
                    {
                        EDPopPanel.PopWindow("输入效果名>>>>>>", (x) =>
                        {
                            if (CheckContainImpactName(x,SelSkill))
                            {
                                Debug.LogError("效果名重复>>>>>>>>>>>>" + x);
                                return;
                            }
                            SkillImpact impact = new SkillImpact
                            {
                                Name = x
                            };
                            SelSkill.ImpactList.Add(impact);
                            SetSkillImpactStartTime(SelSkill, SelSkill.ImpactList.Count - 1);
                        });
                    });
                });

                EDLayout.CreateHorizontal("box", width, 30, () =>
                {
                    SkillPlayTime = EditorGUILayout.Slider("技能时间线",SkillPlayTime,0,(float)SelSkill.ContinueTime,GUILayout.Width(width), GUILayout.Height(25));
                });

                EDLayout.CreateHorizontal("box", width, 30, () =>
                {
                    if (string.IsNullOrEmpty(SelSkill.DecStr)&&SelGo!=null)
                    {
                        SelSkill.DecStr = SelGo.transform.parent.name;
                    }
                    EditorGUILayout.LabelField(SelSkill.DecStr, GUILayout.Width(width), GUILayout.Height(25));
                });
            });
        }

        //显示技能效果线
        private void ShowSkillImpactLine(float width, float height)
        {
            width = width - 10;
            float windowMinWidth = 30;

            EDLayout.CreateVertical("box", width, height, () =>
            {
                for (int i = 0; i < SelSkill.ImpactList.Count; i++)
                {
                    SkillImpact impact = SelSkill.ImpactList[i];

                    float leftWidth  = impact.Time <= 0 ? 0 : (float)(impact.Time/SelSkill.ContinueTime)* width;
                    float btnWidth   = impact.ContinueTime <= 0? windowMinWidth : (float)(impact.ContinueTime / SelSkill.ContinueTime) * width;

                    float itemHeight = height / SelSkill.ImpactList.Count;

                    EDLayout.CreateHorizontal("box", width, itemHeight, () =>
                    {
                        if (leftWidth>0)
                        {
                            EDLayout.CreateHorizontal("", leftWidth, itemHeight, () => {
                                EditorGUILayout.Space();
                            });
                        }

                        EDColor.DrawColorArea(SelImpact != null && SelImpact.Name == impact.Name ? Color.green : Color.white, () =>
                        {
                            EDButton.CreateBtn(impact.Name, btnWidth, itemHeight, () => {
                                SelImpact = impact;
                            });
                        });
                    });
                }
            });
        }

        private void ShowSkillImpact(float width, float height)
        {
            if (SelImpact==null)
            {
                return;
            }

            int testWidth = 200;
            EDLayout.CreateVertical("", width-10, height, (float wid01, float hei01) =>
            {
                EDLayout.CreateHorizontal("box", wid01, 25, (float wid02, float hei02)=> {
                    EditorGUILayout.LabelField(SelImpact.Name, GUILayout.Width(testWidth), GUILayout.Height(25));
                    EDButton.CreateBtn("删除此效果", testWidth, 25, () =>
                    {
                        for (int i = 0; i < SelSkill.ImpactList.Count; i++)
                        {
                            SkillImpact impact = SelSkill.ImpactList[i];
                            if (impact.Name==SelImpact.Name)
                            {
                                SelSkill.ImpactList.RemoveAt(i);
                                SelImpact = null;
                                return;
                            }
                        }
                    });
                });
                
                //时间信息
                EDLayout.CreateHorizontal("box", wid01, 25, (float wid02, float hei02) =>
                {
                    //作用时间
                    object startTimeValue = SelImpact.Time;
                    EDTypeField.CreateTypeField("开始时间:", ref startTimeValue, typeof(double), testWidth, 25);
                    double tmpStartTime = double.Parse(startTimeValue.ToString());
                    if (tmpStartTime != SelImpact.Time)
                    {
                        if (tmpStartTime<=0)
                        {
                            tmpStartTime = 0;
                        }
                        SelImpact.Time = tmpStartTime;
                    }

                    //持续时间
                    object continueTimeValue = SelImpact.ContinueTime;
                    EDTypeField.CreateTypeField("持续时间:", ref continueTimeValue, typeof(double), testWidth, 25);
                    double tmpTime = double.Parse(continueTimeValue.ToString());
                    if (tmpTime!= SelImpact.ContinueTime)
                    {
                        if (tmpTime <= 0)
                        {
                            tmpTime = 0;
                        }
                        SelImpact.ContinueTime = tmpTime;
                    }
                });

                //打断设置
                EDLayout.CreateHorizontal("box", wid01, 25, (float wid02, float hei02) =>
                {
                    //是否可被打断
                    SelImpact.CanBreak = EditorGUILayout.Toggle("作用是否可被打断：", SelImpact.CanBreak, GUILayout.Width(testWidth), GUILayout.Height(35));

                    if (SelImpact.CanBreak)
                    {
                        SelImpact.BreakExit = EditorGUILayout.Toggle("打断后技能结束：", SelImpact.BreakExit, GUILayout.Width(testWidth), GUILayout.Height(35));
                    }
                });


                EDLayout.CreateHorizontal("", wid01, height-50, () =>
                {
                    ShowSkillSelfImpactInfo(wid01 / 2, height - 50);
                    ShowSkillOtherImpactInfo(wid01 / 2, height - 50);
                });
            });
        }

        private void ShowSkillSelfImpactInfo(float width, float height)
        {
            EDLayout.CreateVertical("box", width-10, height, () =>
            {
                if (SelImpact.SelfInfo == null)
                {
                    EDButton.CreateBtn("创建自身作用效果", width - 10, 30, () =>
                    {
                        SelImpact.SelfInfo = new SkillSelfImpactInfo();
                    });
                    return;
                }
                EditorGUILayout.LabelField("自身作用效果", GUILayout.Width(width - 10), GUILayout.Height(25));
                ShowSelfSkillImpactInfo(SelImpact.SelfInfo, width-10, height-30,1);
            });
        }

        private void ShowSkillOtherImpactInfo(float width, float height)
        {
            EDLayout.CreateVertical("box", width-10, height, () =>
            {
                if (SelImpact.OtherInfo == null)
                {
                    EDButton.CreateBtn("创建他人作用效果", width - 10, 30, () =>
                    {
                        SelImpact.OtherInfo = new SkillOtherImpactInfo();
                    });
                    return;
                }

                float itemWidth = width - 10;

                EditorGUILayout.LabelField("他人作用效果", GUILayout.Width(width - 10), GUILayout.Height(25));
                //作用位置
                object data = LCConvert.StrChangeToObject(SelImpact.OtherInfo.Pos, typeof(Vector3).FullName);
                EDTypeField.CreateTypeField("作用位置：", ref data, typeof(Vector3), width-10, 35);
                SelImpact.OtherInfo.Pos = LCExtension.ToString(data, typeof(Vector3).FullName);

                //作用范围
                object areaData = LCConvert.StrChangeToObject(SelImpact.OtherInfo.Area, typeof(Vector3).FullName);
                EDTypeField.CreateTypeField("作用范围：", ref areaData, typeof(Vector3), width - 10, 35);
                SelImpact.OtherInfo.Area = LCExtension.ToString(areaData, typeof(Vector3).FullName);

                //作用时间
                EditorGUILayout.LabelField("数据作用时间：", GUILayout.Width(width - 10), GUILayout.Height(25));
                SelImpact.OtherInfo.Time = EditorGUILayout.Slider((float)SelImpact.OtherInfo.Time, 0, (float)SelImpact.ContinueTime, GUILayout.Width(itemWidth), GUILayout.Height(25));

                //僵直时间
                SelImpact.OtherInfo.StopTime = EditorGUILayout.DoubleField("僵直时间:", SelImpact.OtherInfo.StopTime, GUILayout.Width(itemWidth), GUILayout.Height(25));

                SelImpact.OtherInfo.DataType = (SkillDataImpactType)EditorGUILayout.EnumPopup("数据作用类型:", SelImpact.OtherInfo.DataType, GUILayout.Width(itemWidth), GUILayout.Height(25));
                if (SelImpact.OtherInfo.DataType != SkillDataImpactType.Once)
                {
                    SelImpact.OtherInfo.ContinueTime = EditorGUILayout.DoubleField("数据作用持续时间:", SelImpact.OtherInfo.ContinueTime, GUILayout.Width(itemWidth), GUILayout.Height(25));
                }

                if (SelImpact.OtherInfo.DataType == SkillDataImpactType.Gap)
                {
                    SelImpact.OtherInfo.GapTime = EditorGUILayout.DoubleField("数据在持续时间内间隔多久作用一次:", SelImpact.OtherInfo.GapTime, GUILayout.Width(itemWidth), GUILayout.Height(25));
                }

                ShowSkillFilter(SelImpact.OtherInfo.Filter, width-10, 100);

                ShowOtherSkillImpactInfo(SelImpact.OtherInfo, width-10, height-200, 2);
            });
        }

        //显示他人技能作用筛选
        private void ShowSkillFilter(SkillFilter filter,float width, float height)
        {
            EDLayout.CreateVertical("box", width, height, () =>
            {
                //筛选目标
                filter.TargetType = (SkillTargetType)EditorGUILayout.EnumPopup("筛选目标：", filter.TargetType, GUILayout.Width(width-10), GUILayout.Height(25));
                //筛选规则
                filter.Filter = (SkillFilterRule)EditorGUILayout.EnumPopup("筛选规则：", filter.Filter, GUILayout.Width(width - 10), GUILayout.Height(25));
                //筛选属性
                filter.DataName = EditorGUILayout.TextField("筛选规则属性名：", filter.DataName, GUILayout.Width(width - 10), GUILayout.Height(25));
                //筛选数量
                filter.TargetCnt  = EditorGUILayout.IntField("筛选数量：", filter.TargetCnt, GUILayout.Width(width - 10), GUILayout.Height(25));
            });
        }

        //显示技能作用数据
        private void ShowSelfSkillImpactInfo(SkillSelfImpactInfo info,float width, float height,int index)
        {
            float itemWidth = width - 10;

            ImpactInfoTog["Data" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Data" + index], "作用数据");
            if (ImpactInfoTog["Data" + index])
            {
                //Data
                EDLayout.CreateVertical("box", width, 300, () =>
                {
                    if (info.Data == null)
                    {
                        EDButton.CreateBtn("创建效果数据", 100, 25, () =>
                        {
                            info.Data = new SkillSelfData();
                        });
                    }
                    else
                    {
                        EditorGUILayout.LabelField("数据作用时间：", GUILayout.Width(itemWidth), GUILayout.Height(25));
                        info.Data.Time = EditorGUILayout.Slider((float)info.Data.Time, 0, (float)SelImpact.ContinueTime, GUILayout.Width(itemWidth), GUILayout.Height(25));

                        info.Data.DataType = (SkillDataImpactType)EditorGUILayout.EnumPopup("数据作用类型:", info.Data.DataType, GUILayout.Width(itemWidth), GUILayout.Height(25));
                        if (info.Data.DataType != SkillDataImpactType.Once)
                        {
                            info.Data.ContinueTime = EditorGUILayout.DoubleField("数据作用持续时间:", info.Data.Time, GUILayout.Width(itemWidth), GUILayout.Height(25));
                        }

                        if (info.Data.DataType == SkillDataImpactType.Gap)
                        {
                            info.Data.GapTime = EditorGUILayout.DoubleField("数据在持续时间内间隔多久作用一次:", info.Data.GapTime, GUILayout.Width(itemWidth), GUILayout.Height(25));
                        }

                        ShowSkillDataOperateList(info.Data.DataOperates, itemWidth, 200);
                    }
                });
            }

            ImpactInfoTog["Anim" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Anim" + index], "作用动画");
            if (ImpactInfoTog["Anim" + index])
            {
                //Anim
                EDLayout.CreateVertical("box", width, 50, () =>
                {
                    if (info.Anim == null)
                    {
                        EDButton.CreateBtn("创建效果动画", 100, 25, () =>
                        {
                            info.Anim = new SkillAnim();
                        });
                    }
                    else
                    {
                        EditorGUILayout.LabelField("动画播放时间：", GUILayout.Width(itemWidth), GUILayout.Height(25));
                        info.Anim.Time = EditorGUILayout.Slider((float)info.Anim.Time, 0, (float)SelImpact.ContinueTime, GUILayout.Width(itemWidth), GUILayout.Height(25));

                        EDButton.CreateBtn("效果持续时间设置为动画时间", itemWidth, 25, () =>
                        {

                            SelImpact.ContinueTime = LCUtil.GetAnimClipTime(SelAnim, info.Anim.AnimName);
                        });
                        
                        EDButton.CreateBtn(info.Anim.AnimName, itemWidth, 25, () =>
                        {
                            List<string> names = GetAnimatorClipNames(SelAnim);
                            EDPopMenu.CreatePopMenu(names, (string clipName) =>
                            {
                                info.Anim.AnimName = clipName;
                                float animTime = GetAnimatorClipTime(SelAnim, clipName);
                                if (SelImpact.ContinueTime < animTime)
                                {
                                    SelImpact.ContinueTime = animTime;
                                }
                            });
                        });
                    }
                });
            }

            ImpactInfoTog["Effect" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Effect" + index], "作用特效");
            if (ImpactInfoTog["Effect" + index])
            {
                //Effect
                EDLayout.CreateVertical("box", width, 100, () =>
                {
                    if (info.Effect == null)
                    {
                        EDButton.CreateBtn("创建效果特效", 100, 25, () =>
                        {
                            info.Effect = new SkillEffect();
                        });
                    }
                    else
                    {
                        EditorGUILayout.LabelField("特效播放时间：", GUILayout.Width(itemWidth), GUILayout.Height(25));
                        info.Effect.Time = EditorGUILayout.Slider((float)info.Effect.Time, 0, (float)SelImpact.ContinueTime, GUILayout.Width(itemWidth), GUILayout.Height(25));

                        //特效Id
                        object hideValue = info.Effect.HideTime;
                        EDTypeField.CreateTypeField("特效隐藏时间:", ref hideValue, typeof(double), itemWidth, 25);
                        info.Effect.HideTime = double.Parse(hideValue.ToString());

                        //特效Id
                        object idValue = info.Effect.EffectId;
                        EDTypeField.CreateTypeField("特效Id:", ref idValue, typeof(int), itemWidth, 25);
                        info.Effect.EffectId = int.Parse(idValue.ToString());

                        //特效位置
                        object value = LCConvert.StrChangeToObject(info.Effect.Pos, typeof(Vector3).FullName);
                        EDTypeField.CreateTypeField("特效位置:", ref value, typeof(Vector3), itemWidth, 25);
                        info.Effect.Pos = LCExtension.ToString(value, typeof(Vector3).FullName);
                    }
                });
            }

            ImpactInfoTog["Audio" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Audio" + index], "作用音效");
            if (ImpactInfoTog["Audio" + index])
            {
                //Audio
                EDLayout.CreateVertical("box", width, 50, () =>
                {
                    if (info.Audio == null)
                    {
                        EDButton.CreateBtn("创建效果音效", 100, 25, () =>
                        {
                            info.Audio = new SkillAudio();
                        });
                    }
                    else
                    {
                        EditorGUILayout.LabelField("音效播放时间：", GUILayout.Width(itemWidth), GUILayout.Height(25));
                        info.Audio.Time = EditorGUILayout.Slider((float)info.Audio.Time, 0, (float)SelImpact.ContinueTime, GUILayout.Width(itemWidth), GUILayout.Height(25));

                        object idValue = info.Audio.AudioId;
                        EDTypeField.CreateTypeField("音效Id:", ref idValue, typeof(int), itemWidth, 25);
                        info.Audio.AudioId = int.Parse(idValue.ToString());
                    }
                });
            }

        }

        private void ShowOtherSkillImpactInfo(SkillOtherImpactInfo info, float width, float height, int index)
        {
            float itemWidth = width - 10;

            ImpactInfoTog["Data" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Data" + index], "作用数据");
            if (ImpactInfoTog["Data" + index])
            {
                //Data
                EDLayout.CreateVertical("box", width, 300, () =>
                {
                    if (info.Data == null)
                    {
                        EDButton.CreateBtn("创建效果数据", 100, 25, () =>
                        {
                            info.Data = new SkillOtherData();
                        });
                    }
                    else
                    {
                        ShowSkillDataOperateList(info.Data.DataOperates, itemWidth, 200);
                    }
                });
            }

            ImpactInfoTog["Anim" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Anim" + index], "作用动画");
            if (ImpactInfoTog["Anim" + index])
            {
                //Anim
                EDLayout.CreateVertical("box", width, 50, () =>
                {
                    if (info.Anim == null)
                    {
                        EDButton.CreateBtn("创建效果动画", 100, 25, () =>
                        {
                            info.Anim = new SkillAnim();
                        });
                    }
                    else
                    {
                        EDButton.CreateBtn(info.Anim.AnimName, itemWidth, 25, () =>
                        {
                            List<string> names = GetAnimatorClipNames(SelAnim);
                            EDPopMenu.CreatePopMenu(names, (string clipName) =>
                            {
                                info.Anim.AnimName = clipName;
                                float animTime = GetAnimatorClipTime(SelAnim, clipName);
                                if (SelImpact.ContinueTime < animTime)
                                {
                                    SelImpact.ContinueTime = animTime;
                                }
                            });
                        });
                    }
                });
            }

            ImpactInfoTog["Effect" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Effect" + index], "作用特效");
            if (ImpactInfoTog["Effect" + index])
            {
                //Effect
                EDLayout.CreateVertical("box", width, 100, () =>
                {
                    if (info.Effect == null)
                    {
                        EDButton.CreateBtn("创建效果特效", 100, 25, () =>
                        {
                            info.Effect = new SkillEffect();
                        });
                    }
                    else
                    {
                        //特效Id
                        object hideValue = info.Effect.HideTime;
                        EDTypeField.CreateTypeField("特效隐藏时间:", ref hideValue, typeof(double), itemWidth, 25);
                        info.Effect.HideTime = double.Parse(hideValue.ToString());

                        //特效Id
                        object idValue = info.Effect.EffectId;
                        EDTypeField.CreateTypeField("特效Id:", ref idValue, typeof(int), itemWidth, 25);
                        info.Effect.EffectId = int.Parse(idValue.ToString());

                        //特效位置
                        object value = LCConvert.StrChangeToObject(info.Effect.Pos, typeof(Vector3).FullName);
                        EDTypeField.CreateTypeField("特效位置:", ref value, typeof(Vector3), itemWidth, 25);
                        info.Effect.Pos = LCExtension.ToString(value, typeof(Vector3).FullName);
                    }
                });
            }

            ImpactInfoTog["Audio" + index] = EditorGUILayout.Foldout(ImpactInfoTog["Audio" + index], "作用音效");
            if (ImpactInfoTog["Audio" + index])
            {
                //Audio
                EDLayout.CreateVertical("box", width, 50, () =>
                {
                    if (info.Audio == null)
                    {
                        EDButton.CreateBtn("创建效果音效", 100, 25, () =>
                        {
                            info.Audio = new SkillAudio();
                        });
                    }
                    else
                    {
                        object idValue = info.Audio.AudioId;
                        EDTypeField.CreateTypeField("音效Id:", ref idValue, typeof(int), itemWidth, 25);
                        info.Audio.AudioId = int.Parse(idValue.ToString());
                    }
                });
            }
        }

        private Vector2 DataOperatePos = Vector2.zero;
        private void ShowSkillDataOperateList(List<SkillDataOperate> operates, float width, float height)
        {
            float itemWidth = width - 60;
            EDLayout.CreateScrollView(ref DataOperatePos, "box", width, height, () =>
            {
                EDButton.CreateBtn("添加操作数据", itemWidth, 30, () =>
                {
                    operates.Add(new SkillDataOperate());
                });
                for (int i = 0; i < operates.Count; i++)
                {
                    EDLayout.CreateVertical("GroupBox", width - 40, 70, () =>
                    {
                        SkillDataOperate info = operates[i];
                        EDLayout.CreateHorizontal("", width - 50, 40, () =>
                        {
                            EDLayout.CreateVertical("", itemWidth / 2, 40, () =>
                            {
                                EditorGUILayout.LabelField("属性:", GUILayout.Width(itemWidth / 2), GUILayout.Height(20));
                                info.Name = EditorGUILayout.TextField(info.Name, GUILayout.Width(itemWidth / 2), GUILayout.Height(20));
                            });

                            EDLayout.CreateVertical("", itemWidth / 2, 40, () =>
                            {
                                EditorGUILayout.LabelField("值:", GUILayout.Width(itemWidth / 2), GUILayout.Height(20));
                                info.Data = EditorGUILayout.FloatField((float)info.Data, GUILayout.Width(itemWidth / 2), GUILayout.Height(20));
                            });
                        });

                        EDLayout.CreateHorizontal("", width - 50, 40, () =>
                        {
                            EDLayout.CreateVertical("", itemWidth / 2, 40, () =>
                            {
                                EditorGUILayout.LabelField("公式:", GUILayout.Width(itemWidth / 2), GUILayout.Height(20));
                                info.MathType = (SkillDataMathType)EditorGUILayout.EnumPopup(info.MathType, GUILayout.Width(itemWidth / 2), GUILayout.Height(25));

                            });
                            EDButton.CreateBtn("删除", itemWidth / 2, 40, () =>
                            {
                                operates.RemoveAt(i);
                            });
                        });
                    });
                }
            });

            
            
        }

        #endregion

        #endregion

        #region Data

        private void SelSkillChange(SkillJson skill)
        {
            if (skill == null)
            {
                SelSkill = null;
                return;
            }
            SelSkill = skill;
            SelImpact = null;
        }

        private bool CheckContainSkill(string id)
        {
            int skillId = int.Parse(id);
            for (int i = 0; i < Json.List.Count; i++)
            {
                if (Json.List[i].Id == skillId)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckContainImpactName(string name, SkillJson skill)
        {
            for (int i = 0; i < skill.ImpactList.Count; i++)
            {
                if (skill.ImpactList[i].Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        //计算技能持续时间
        private double GetSkillContinueTime(SkillJson skill)
        {
            double resTime = 0;
            for (int i = 0; i < skill.ImpactList.Count; i++)
            {
                SkillImpact impact = skill.ImpactList[i];
                double tempTime = impact.Time + impact.ContinueTime;
                if (tempTime >= resTime)
                {
                    resTime = tempTime;
                }
            }
            return resTime;
        }

        //计算作用开始时间
        private void SetSkillImpactStartTime(SkillJson skill,int index)
        {
            double offTime = 0;
            for (int i = 0; i < skill.ImpactList.Count; i++)
            {
                SkillImpact impact = skill.ImpactList[i];
                if (i<index)
                {
                    offTime += impact.ContinueTime;
                }
            }
            skill.ImpactList[index].Time = offTime;
        }

        //保存实体Json数据
        private void Save()
        {
            if (Json.List.Count == 0)
            {
                return;
            }
            string jsonData = JsonMapper.ToJson(Json);
            EDTool.WriteText(jsonData, Application.dataPath + JsonPath);
            AssetDatabase.Refresh();
        }

        #endregion

        #region Anim

        public AnimationClip GetAnimatorClipByName(Animator animator,string name)
        {
            if (animator == null)
            {
                return null;
            }
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == name)
                {
                    return clip;
                }
            }
            return null;
        }

        public List<string> GetAnimatorClipNames(Animator animator)
        {
            if (animator==null)
            {
                return new List<string>() { "Damage" };
            }
            List<string> names = new List<string>();
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                names.Add(clip.name);
            }
            return names;
        }

        public float GetAnimatorClipTime(Animator animator, string name)
        {
            if (animator==null)
            {
                return 0;
            }
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == name)
                {
                    return clip.length;
                }
            }
            return 0;
        }

        public bool CheckAnimatorHasClip(Animator animator, string name)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (name == clip.name)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
