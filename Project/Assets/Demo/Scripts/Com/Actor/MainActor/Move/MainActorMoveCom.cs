using System;
using LCECS;
using LCECS.Core;
using LCMap;
using LCToolkit;
using Scenes.MoveTest;
using UnityEngine;

namespace Demo.Com.MainActor
{
    public enum MainActorMoveState
    {
        Normal,
        Jump,
        Climb,
        Dash,
        Fall,
    }
    
    public partial class MainActorMoveCom : BaseCom
    {
        [NonSerialized] private Rigidbody2D rig2D;
        [NonSerialized] private BoxCollider2D collider2D;
        [NonSerialized] private MainActorMoveCollider moveCollider;
        [NonSerialized] private MainActorMoveAnim moveAnim;
        [NonSerialized] private MainActorMoveHelper monoHelper;
        
        [NonSerialized] private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
        }
        
        [NonSerialized] private DirType currDir;
        [NonSerialized] private MainActorMoveState moveState;
        public MainActorMoveState MoveState
        {
            get { return moveState; }
        }
        
        [NonSerialized] private float moveH;         //横向位移减速时的速度
        [NonSerialized] private int introDir;        //横向位移减速时的方向
        [NonSerialized] private bool isCanControl = true;      //是否允许控制
        [NonSerialized] private bool isMove = true;            //是否允许左右移动
        [NonSerialized] private RaycastHit2D[] HorizontalBox;
        
        [NonSerialized] private Actor actor;
        [NonSerialized] private TransCom trans;
        [NonSerialized] private BindGoCom bindGo;
        [NonSerialized] private ActorDisplayCom displayCom;
        [NonSerialized] private MainActorInputCom input;
        [NonSerialized] private AnimCom animCom;
        
        
        [NonSerialized] private bool banMove;

        protected override void OnAwake(Entity pEntity)
        {
            input = ECSLocate.ECS.GetWorld().GetCom<MainActorInputCom>();
            actor = pEntity as Actor;
            trans = pEntity.GetCom<TransCom>();
            bindGo = pEntity.GetCom<BindGoCom>();
            bindGo.RegGoChange(OnBindGoChange);
            animCom = pEntity.GetCom<AnimCom>();

            displayCom = pEntity.GetCom<ActorDisplayCom>();
            displayCom.RegStateChange(OnStateChange);

            moveAnim = new MainActorMoveAnim(this, animCom);
        }

        private void OnBindGoChange(GameObject pGo)
        {
            rig2D = pGo.GetComponent<Rigidbody2D>();
            monoHelper = pGo.transform.GetOrAddCom<MainActorMoveHelper>();
        }
        
        private void OnStateChange(string pStateName)
        {
            collider2D = displayCom.BodyCollider;
            moveCollider = new MainActorMoveCollider();
            moveCollider.Init(collider2D,"Map");
        }

        public void Update()
        {
            if (banMove)
                return;
            
            //更新碰撞
            moveCollider.UpdateRaycastHitInfos();
            
            //更新方向
            UpdateDir();
            
            //更新动画
            moveAnim.ExecutePlayAnim();
            
            //修复位置
            if(velocity.x >= MoveSpeed)
            {
                CheckFixedHorizontalMove();
            }
            if(velocity.y >6)
            {
                CheckFixedUpMove();
            }
            
            //碰撞点位debug
            if (HorizontalBox != null && HorizontalBox.Length > 0 && HorizontalBox[0])
            {
                Debug.DrawLine(moveCollider.Center, HorizontalBox[0].point, Color.yellow);
            }
            
            //冲刺
            if (input.DashKeyDown && dashCount > 0)
            {
                Dash();
            }
            switch (moveState)
            {
                case MainActorMoveState.Normal:
                    Normal();
                    break;
                case MainActorMoveState.Climb:
                    Climb();
                    break;
                case MainActorMoveState.Fall:
                    Fall();
                    break;
                case MainActorMoveState.Dash:
                    CheckDashJump();
                    break;
            }
            trans.SetPos(rig2D.position,true);
        }

        public void FixedUpdate()
        {
            if (banMove)
                return;
            
            if(CoyotetimeFram > 0)
            {
                CoyotetimeFram--;
            }
            HorizontalMove();
            rig2D.MovePosition(rig2D.position + velocity * Time.fixedDeltaTime);
        }

        #region 移动

        /// <summary>
        /// 横向移动
        /// </summary>
        private void HorizontalMove()
        {
            if (isCanMove())
            {
                //减速速阶段
                if ((velocity.x > 0 && input.MoveDir == -1) || (velocity.x < 0 && input.MoveDir == 1) || input.MoveDir == 0 ||
                    (IsGround() && input.v < 0) || Mathf.Abs(velocity.x) > MoveSpeed)
                {
                    introDir = velocity.x > 0 ? 1 : -1;
                    moveH = Mathf.Abs(velocity.x);
                    
                    //地面上三帧减速到0
                    if(IsGround())
                    {
                        moveH -= MoveSpeed / 3;
                    }
                    else
                    {
                        moveH -= MoveSpeed / 6;
                    }
                    if (moveH < 0.01f)
                    {
                        moveH = 0;
                    }
                    velocity.x = moveH * introDir;
                }
                else
                {
                    //蹲下不允许移动
                    if (IsGround() && input.v < 0)
                        return;

                    //右
                    if (input.MoveDir == 1)
                    {
                        //地面上6帧到达满速
                        if (IsGround())
                        {
                            velocity.x += MoveSpeed / 6;
                        }
                        else
                        {
                            velocity.x += MoveSpeed / 15f;
                        }
                        if (velocity.x > MoveSpeed)
                            velocity.x = MoveSpeed;
                    }
                    else if (input.MoveDir == -1)
                    {
                        if (IsGround())
                        {
                            velocity.x -= MoveSpeed / 6;
                        }
                        else
                        {
                            velocity.x -= MoveSpeed / 12f;
                        }
                        if (velocity.x < -MoveSpeed)
                            velocity.x = -MoveSpeed;
                    }
                }
  
            }
        }

        #endregion

        #region 方向
        
        [NonSerialized] private DirType lastDir;
        
        private void UpdateDir()
        {
            if (moveState == MainActorMoveState.Climb || moveState == MainActorMoveState.Dash)
                return;
            lastDir = currDir;
            if (input.MoveDir > 0)
            {
                currDir = DirType.Right;
            }
            else if (input.MoveDir < 0)
            {
                currDir = DirType.Left;
            }
            if(lastDir != currDir)
            {
                actor.SetDir(currDir);
            }
        }
        

        #endregion

        #region 修复方向

        /// <summary>
        /// 检测并修正水平方向的位移
        /// </summary>
        private bool FixHorizon;
        private void CheckFixedHorizontalMove()
        {
            if (FixHorizon)
                return;
            HorizontalBox = currDir == DirType.Right ? moveCollider.RightBox : moveCollider.LeftBox;
            if (HorizontalBox.Length == 1)
            {
                var pointDis = HorizontalBox[0].point.y - Center.y;
                if (pointDis > 0.34f)
                {
                    var offsetPos = Mathf.Ceil(rig2D.position.y);
                    rig2D.position = new Vector2(rig2D.position.x, offsetPos - 0.22f);
                }
                else if (pointDis < -0.42f)
                {
                    var offsetPos = Mathf.Ceil(rig2D.position.y);
                    rig2D.position = new Vector2(rig2D.position.x, offsetPos + 0.035f);
                }
                FixHorizon = true;
            }
        }
        
        /// <summary>
        /// 检测并修正垂直方向的位移
        /// </summary>
        private bool CheckFixedUpMove()
        {
            if (moveCollider.UpBox.Length == 1)
            {
                var pointDis = moveCollider.UpBox[0].point.x - Center.x;
                if (pointDis > 0.34f)
                {
                    var offsetPos = Mathf.Floor(rig2D.position.x);
                    rig2D.position = new Vector2(offsetPos + 0.48f, rig2D.position.y);
                    return true;
                }
                else if (pointDis < -0.34f)
                {
                    var offsetPos = Mathf.Floor(rig2D.position.x);
                    rig2D.position =  new Vector2(offsetPos + 0.52f, rig2D.position.y);
                    return true;
                }
                else
                {
                    velocity.y = 0;
                    ChangeState(MainActorMoveState.Fall);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Check

        /// <summary>
        /// 可以移动
        /// </summary>
        /// <returns></returns>
        private bool isCanMove()
        {
            return moveState != MainActorMoveState.Dash && moveState != MainActorMoveState.Climb && isCanControl && isMove;
        }
        
        /// <summary>
        /// 在地面上
        /// </summary>
        /// <returns></returns>
        public bool IsGround()
        {
            return moveCollider.IsGround;
        }

        #endregion

        #region Set

        private void ChangeState(MainActorMoveState pState)
        {
            moveState = pState;
        }

        public void BanMove(bool pBanMove, string pBanReason = "")
        {
            banMove = pBanMove;
            if (banMove)
            {
                GameLocate.Log.Log("禁止移动》》》",pBanReason);
            }
        }

        #endregion

        #region Get

        public int GetDirInt
        {
            get{
                return currDir == DirType.Right ? 1 : -1;
            }
        }
        
        public Vector2 Center
        {
            get
            {
                return moveCollider.Center;
            }
        }

        public Vector2 Size
        {
            get
            {
                return moveCollider.Size;
            }
        }

        #endregion
    }
}