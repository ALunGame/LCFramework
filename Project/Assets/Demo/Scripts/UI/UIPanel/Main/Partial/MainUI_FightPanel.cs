using Demo.Com;
using Demo.Mediator;
using LCECS;
using LCToolkit;
using LCUI;
using UnityEngine;

namespace Demo.UI
{
    public class MainUI_FightPanelModel : UIModel
    {
    }
    
    public class MainUI_FightPanel : UIPanel<MainUI_FightPanelModel>
    {
        private UIComGlue<BaseButton> leftMoveBtn = new UIComGlue<BaseButton>("Left/MoveBtn/LeftMove");
        private UIComGlue<BaseButton> rightMoveBtn = new UIComGlue<BaseButton>("Left/MoveBtn/RightMove");

        private UIBtnGlue skill_1_Btn = new UIBtnGlue("Right/SkillBtn/Skill_1", () =>
        {
            ActorMediator.ReleaseSkill(LCMap.MapLocate.Map.PlayerActor, 100101);
        });
        private UIBtnGlue skill_2_Btn = new UIBtnGlue("Right/SkillBtn/Skill_2", () =>
        {
            ActorMediator.ReleaseSkill(LCMap.MapLocate.Map.PlayerActor, 100102);
        });

        private MainActorInputCom inputCom;

        public override void OnAwake()
        {
            inputCom = ECSLocate.ECS.GetWorld().GetCom<MainActorInputCom>();
            InitMoveBtn();
            InitMoveInputCheckFunc();

            InitJumpBtn();
            InitJumpInputCheckFunc();
        }


        #region 移动按钮事件

        private bool leftMoveDown;
        private bool leftMoveUp;
        private bool leftMoveHode;
        
        private bool rightMoveDown;
        private bool rightMoveUp;
        private bool rightMoveHode;
        
        private void InitMoveBtn()
        {
            leftMoveBtn.Com.SetDown((data) =>
            {
                inputCom.h = -1;
                leftMoveDown = true;
                leftMoveUp   = false;
                leftMoveHode = false;
            });
            leftMoveBtn.Com.SetUp((data) =>
            {
                inputCom.h = 0;
                leftMoveHode = false;
                leftMoveDown = false;
                leftMoveUp   = true;
            });
            leftMoveBtn.Com.SetHold((data) =>
            {
                inputCom.h = -1;
                leftMoveHode = true;
            });
            
            rightMoveBtn.Com.SetDown((data) =>
            {
                inputCom.h = 1;
                rightMoveDown   = true;
                rightMoveUp     = false;
                rightMoveHode   = false;
            });
            rightMoveBtn.Com.SetUp((data) =>
            {
                inputCom.h = 0;
                rightMoveHode = false;
                rightMoveDown = false;
                rightMoveUp   = true;
            });
            rightMoveBtn.Com.SetHold((data) =>
            {
                inputCom.h = 1;
                rightMoveHode = true;
            });
        }

        private void InitMoveInputCheckFunc()
        {
            #region 左移动按钮

            //左
            inputCom.LeftMoveKey.CheckKey = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.A))
                {
                    return true;
                }
#endif
                if (leftMoveHode)
                {
                    return true;
                }
                return false;
            };
            inputCom.LeftMoveKey.CheckKeyDown = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.A))
                {
                    return true;
                }
#endif
                if (leftMoveDown)
                {
                    return true;
                }
                return false;
            };
            inputCom.LeftMoveKey.CheckKeyUp = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKeyUp(KeyCode.A))
                {
                    return true;
                }
#endif
                if (leftMoveUp)
                {
                    return true;
                }
                return false;
            };

            #endregion

            #region 右移动按钮

            //右
            inputCom.RightMoveKey.CheckKey = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.D))
                {
                    return true;
                }
#endif
                if (rightMoveHode)
                {
                    return true;
                }
                return false;
            };
            inputCom.RightMoveKey.CheckKeyDown = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.D))
                {
                    return true;
                }
#endif
                if (rightMoveDown)
                {
                    return true;
                }
                return false;
            };
            inputCom.RightMoveKey.CheckKeyUp = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKeyUp(KeyCode.D))
                {
                    return true;
                }
#endif
                if (rightMoveUp)
                {
                    return true;
                }
                return false;
            };

            #endregion
        }
        

        #endregion

        #region 跳跃按钮事件

        private void InitJumpBtn()
        {
            
        }

        private void InitJumpInputCheckFunc()
        {
            inputCom.Jump.CheckKey = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKey(KeyCode.Space))
                {
                    return true;
                }
#endif
                return false;
            };
            
            inputCom.Jump.CheckKeyDown = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    return true;
                }
#endif
                return false;
            };
            
            inputCom.Jump.CheckKeyUp = () =>
            {
#if UNITY_EDITOR
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    return true;
                }
#endif
                return false;
            };
        }
        

        #endregion
    }
}