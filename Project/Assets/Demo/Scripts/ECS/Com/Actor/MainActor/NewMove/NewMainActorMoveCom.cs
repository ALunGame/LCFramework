using System;
using LCECS.Core;
using LCMap;
using LCToolkit;
using LCToolkit.FSM;
using UnityEditor;
using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public class NewMainActorMoveCom : BaseCom
    {
        
        //一段跳最大距离
        [NonSerialized]
        public float JumpDis = 0.5f;
        
        //可以二段跳最小距离
        [NonSerialized]
        public float JumpSecondCanDis = 0.5f;			   
        //二段跳最大距离
        [NonSerialized]
        public float JumpSecondDis = 2.5f;            
        //二段跳跳跃速度
        [NonSerialized]
        public float JumpSecondSpeed = 18;
        //蹬墙跳水平速度
        [NonSerialized]
        public float JumpWallSpeed = 4;


        [NonSerialized]
        public Vector2 Speed;
        [NonSerialized]
        public float MaxRunSpeed = 9;
        
        [NonSerialized]
        public float JumpSpeed = 10.5f;  //最大跳跃速度
        
        public Vector2 Pos { get; private set; }
        public ActorDir CurrDir { get; private set; }

        private TransCom transCom { get; set; }
        private Actor actor { get; set; }
        
        public MainActorCollider Collider { get; private set; }
        public MainActorInput Input { get; private set; }
        
        public bool IsGround {get; private set;}

        [NonSerialized]
        private Fsm fsm;
        [NonSerialized]
        private BaseMainActorMoveState[] states = 
        {
            new MainActorRunState(),
            new MainActorClimbState(),
            new MainActorJumpState(),
            new MainActorFullState(),
        };
        
        [NonSerialized]
        private int totalJumpStep = 2;
        public int CurrJumpStep { get; private set; }

        protected override void OnAwake(Entity pEntity)
        {
            Actor actor = pEntity as Actor;
            transCom = actor.Trans;
            this.actor = actor;
            
            Input = new MainActorInput();
            Input.KeyCodeMode();
            
            fsm = Fsm.Create(actor,states);
            fsm.Start(MainActorMoveStateDef.Run);
            
            BindGoCom bindGoCom = pEntity.GetCom<BindGoCom>();
            if (bindGoCom != null)
            {
                bindGoCom.RegGoChange(OnBindGoChange);
            }
            ActorDisplayCom displayCom = pEntity.GetCom<ActorDisplayCom>();
            if (displayCom != null)
            {
                displayCom.RegStateChange((stateName) =>
                {
                    OnDisplayGoChange(displayCom);
                });
            }
        }
        
        private void OnBindGoChange(GameObject pGo)
        {
            UpdateCollider();
        }

        private void OnDisplayGoChange(ActorDisplayCom pDisplayCom)
        {
            UpdateCollider();
        }

        private void UpdateCollider()
        {
            Collider = new MainActorCollider(this,actor);
            if (actor.DisplayCom == null || actor.DisplayCom.BodyCollider == null)
            {
                return;
            }
            
            Collider.SetRect(actor.DisplayCom.BodyCollider);
            Pos = transCom.Pos;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public void Update(float pDeltaTime)
        {
            if (Collider == null || !Collider.IsLegal)
            {
                return;
            }
            
            //状态
            if (Speed.y <= 0)
            {
                IsGround = Collider.CheckGround();
            }
            else
            {
                IsGround = false;
            }
            
            //输入
            Input.Update(pDeltaTime);
            SetDir((ActorDir)Input.MoveX);

            //状态机
            fsm.Update(pDeltaTime,Time.realtimeSinceStartup);
            if (Speed.x != 0)
            {
                SetDir(Speed.x > 0 ? ActorDir.Right : ActorDir.Left);
            }
        }

        public void FixeUpdate(float pDeltaTime)
        {
            //位置
            Collider.TryMovePosX(Speed.x*pDeltaTime);
            Collider.TryMovePosY(Speed.y*pDeltaTime);
        }

        public override void OnDrawGizmosSelected()
        {
            if (fsm.CurrentState!=null)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;

                string showStr = $@"
State:{fsm.CurrentState.GetType().Name}
Speed:{Speed}
Ground:{IsGround}
JumpStep:{CurrJumpStep}
";
                Handles.Label(Pos.ToVector3() + Vector3.up * 2, showStr, style);
            }
        }

        #region 位置方向

        public void SetPos(Vector2 pNewPos)
        {
            Pos = pNewPos;
            actor.SetPos(pNewPos);
        }

        public void SetDir(ActorDir pNewDir)
        {
            if (pNewDir == ActorDir.None)
            {
                return;
            }
            
            CurrDir = pNewDir;

            if (CurrDir == ActorDir.Left)
            {
                actor.SetDir(DirType.Left);
            }
            else
            {
                actor.SetDir(DirType.Right);
            }
        }
        
        #endregion

        #region UpdateSpeed

        /// <summary>
        /// 通过 MoveX 更新速度
        /// </summary>
        public void UpdateSpeedX(float pDeltaTime)
        {
            //水平速度计算
            float maxRunSpeed = MaxRunSpeed;
            float mult = MainActorConst.GroundMult;
            Vector2 currSpeed = Speed;

            float newSpeedX = currSpeed.x;
            //超过最大速度，并且持续向该方向移动，减速到最大速度
            if (Math.Abs(currSpeed.x) > maxRunSpeed && Math.Sign(currSpeed.x) == Input.MoveX)
            {
                newSpeedX = Mathf.MoveTowards(currSpeed.x, maxRunSpeed * Input.MoveX, MainActorConst.RunReduce * mult * pDeltaTime);
            }
            else
            {
                newSpeedX = Mathf.MoveTowards(currSpeed.x, maxRunSpeed * Input.MoveX, MainActorConst.RunAccel * mult * pDeltaTime);
            }
            Speed.x = newSpeedX;
        }

        /// <summary>
        /// 更新下落速度
        /// </summary>
        /// <param name="pFullMult">下落阻力</param>
        public void UpdateFullSpeedY(float pDeltaTime, float pFullMult = 1)
        {
            Speed.y = Mathf.MoveTowards(Speed.y, MainActorConst.MaxFall, MainActorConst.Gravity * pFullMult * pDeltaTime);
        }

        #endregion

        #region Jump
        
        public void Jump()
        {
            if (CurrJumpStep >= totalJumpStep)
                return;
            CurrJumpStep++;

            Input.ClearJump();
            
            //地面起跳添加一个额外速度
            if (IsGround)
            {
                Speed.x += Input.MoveX * MainActorConst.GroundJumpAddSpeed;
            }
            Speed.y = JumpSpeed;
            
            float targetDis = CurrJumpStep <= 0 ? JumpDis : JumpSecondDis;
            fsm.ChangeState(MainActorMoveStateDef.Jump,new MainActorJumpStateContext(targetDis));
        }
        
        public void WallJump(ActorDir pDir)
        {
            CurrJumpStep = 0;
            
            Input.ClearJump();

            Speed.x = (int)pDir * MainActorConst.WallJumpHSpeed;
            Speed.y = JumpSpeed;
            
            float targetDis = CurrJumpStep <= 0 ? JumpDis : JumpSecondDis;
            fsm.ChangeState(MainActorMoveStateDef.Jump,new MainActorJumpStateContext(targetDis));
        }

        public void ClearJumpStep()
        {
            CurrJumpStep = 0;
        }
        
        #endregion

        #region Climb

        public void Climb()
        {
            Speed.x = 0;
            fsm.ChangeState(MainActorMoveStateDef.Climb);
        }

        #endregion
    }
}