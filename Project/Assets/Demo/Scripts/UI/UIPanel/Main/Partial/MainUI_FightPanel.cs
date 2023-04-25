using Demo.Com;
using Demo.Com.MainActor;
using LCECS;
using LCMap;
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

        private UIUpdateGlue updateGlue = new UIUpdateGlue();

        private MainActorInputCom inputCom;

        public override void OnAwake()
        {
            inputCom = ECSLocate.ECS.GetWorld().GetCom<MainActorInputCom>();
            // InitMoveBtn();
            // InitMoveInputCheckFunc();
            //
            // InitJumpBtn();
            // InitJumpInputCheckFunc();
            //
            // InitMoveLegalFunc();
            //
            // updateGlue.SetFunc(Update);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                skill_1_Btn.Click();
            }
        }
        

        #region 移动按钮事件

        private bool leftMoveDown;
        private bool leftMoveUp;
        private bool leftMoveHode;
        
        private bool rightMoveDown;
        private bool rightMoveUp;
        private bool rightMoveHode;

        private float moveValue = 0;
        
        private void InitMoveBtn()
        {
            leftMoveBtn.Com.SetDown((data) =>
            {
                GameLocate.Log.Log("leftMoveBtn.SetDown");
                moveValue = -1;
                leftMoveDown = true;
                leftMoveUp   = false;
                leftMoveHode = false;
            });
            leftMoveBtn.Com.SetUp((data) =>
            {
                GameLocate.Log.Log("leftMoveBtn.SetUp");
                moveValue = 0;
                leftMoveHode = false;
                leftMoveDown = false;
                leftMoveUp   = true;
            });
            leftMoveBtn.Com.SetHold((data) =>
            {
                GameLocate.Log.Log("leftMoveBtn.SetHold");
                moveValue = -1;
                leftMoveHode = true;
            });
            
            rightMoveBtn.Com.SetDown((data) =>
            {
                GameLocate.Log.Log("rightMoveBtn.SetDown");
                moveValue = 1;
                rightMoveDown   = true;
                rightMoveUp     = false;
                rightMoveHode   = false;
            });
            rightMoveBtn.Com.SetUp((data) =>
            {
                GameLocate.Log.Log("rightMoveBtn.SetUp");
                moveValue = 0;
                rightMoveHode = false;
                rightMoveDown = false;
                rightMoveUp   = true;
            });
            rightMoveBtn.Com.SetHold((data) =>
            {
                GameLocate.Log.Log("rightMoveBtn.SetHold");
                moveValue = 1;
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

            inputCom.GetHorizontalInput = () =>
            {
#if UNITY_EDITOR
                float value = Input.GetAxisRaw("Horizontal");
                if (value != 0)
                {
                    return value;
                }
#endif
                return moveValue;
            };
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

        #region 移动合法检测

        private void InitMoveLegalFunc()
        {
            inputCom.LeftMoveKey.CheckLegal = CheckCanMove;
            
            inputCom.RightMoveKey.CheckLegal = CheckCanMove;

            inputCom.Jump.CheckLegal = CheckCanMove;
        }

        private MainActorMoveCom mainActorMoveCom;
        private bool CheckCanMove()
        {
            Actor playerActor = LCMap.MapLocate.Map.PlayerActor;
            if (playerActor == null || !playerActor.isActive)
            {
                return false;
            }

            if (!ActorMediator.CheckCanRequest(playerActor, ActorRequestType.Move))
            {
                if (mainActorMoveCom == null)
                {
                    mainActorMoveCom = playerActor.GetCom<MainActorMoveCom>();
                }
                moveValue = 0;
                mainActorMoveCom.ClearHorizontalVelocity();
                mainActorMoveCom.ClearVerticalVelocity();
                return false;
            }

            if (playerActor.CurrRequestId != (int)ActorRequestType.Move)
            {
                ActorLocate.ActorRequest.Request(playerActor,(int)ActorRequestType.Move,null);
            }
            return true;
        }
        

        #endregion
    }
}