using System;
using Demo;
using Demo.Com;
using LCECS;
using LCECS.Core;
using LCECS.Data;
using Scenes.MoveTest;
using UnityEngine;

namespace Scenes.Move
{
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum PlayState
    {
        Normal,
        Jump,
        Climb,
        Dash,
        Fall,
    }
    
    public partial class PlayerMove : MonoBehaviour
    {
        private PlayerMoveCollider _moveCollider;
        private RaycastHit2D[] HorizontalBox;
        private Rigidbody2D rig;
        private PlayerInputCom input;
        [SerializeField] private BoxCollider2D _collider2D;
        
        [Header("当前速度")]
        public Vector3 Velocity;
        [Header("当前方向")]
        [SerializeField] private DirType playDir;
        [Header("当前状态")]
        [SerializeField] private PlayState playState;
        [Header("移动速度")]
        public float MoveSpeed;
        
        private float moveH;         //横向位移减速时的速度
        private int introDir;        //横向位移减速时的方向
        private bool isCanControl = true;      //是否允许控制
        private bool isMove = true;            //是否允许左右移动

        private void Awake()
        {
            input = GetComponent<PlayerInputCom>();
            rig = GetComponent<Rigidbody2D>();
            _moveCollider = new PlayerMoveCollider();
            _moveCollider.Init(_collider2D,"Map");
            //InitEntity();
        }

        private void Update()
        {
            _moveCollider.UpdateRaycastHitInfos();
            CheckDir();
            if(Velocity.x >= MoveSpeed)
            {
                CheckFixedHorizontalMove();
            }
            if(Velocity.y >6)
            {
                CheckFixedUpMove();
            }
            //碰撞点位debug
            if (HorizontalBox != null && HorizontalBox.Length > 0 && HorizontalBox[0])
            {
                Debug.DrawLine(_moveCollider.Center, HorizontalBox[0].point, Color.yellow);
            }
            if (input.DashKeyDown && dashCount > 0)
            {
                Dash();
            }
            switch (playState)
            {
                case PlayState.Normal:
                    Normal();
                    break;
                case PlayState.Climb:
                    Climb();
                    break;
                case PlayState.Fall:
                    Fall();
                    break;
                case PlayState.Dash:
                    CheckDashJump();
                    break;
            }
            //UpdateEntityPos();
        }

        private void FixedUpdate()
        {
            if(CoyotetimeFram > 0)
            {
                CoyotetimeFram--;
            }
            HorizontalMove();
            rig.MovePosition(transform.position + Velocity * Time.fixedDeltaTime);
        }

        private void OnDrawGizmos()
        {
            if (_moveCollider == null)
            {
                return;
            }
            _moveCollider.DrawRaycastGizmos();
        }

        #region Entity

        private EntityWorkData _entityWorkData;
        private TransCom _transCom;

        private void InitEntity()
        {
            _entityWorkData = ECSLocate.Player.GetPlayerWorkData();
            _transCom = _entityWorkData.MEntity.GetCom<TransCom>();
        }

        private void UpdateEntityPos()
        {
            _transCom.SetPos(rig.position,true);
        }

        #endregion

        /// <summary>
        /// 横向移动
        /// </summary>
        void HorizontalMove()
        {
            if (isCanMove())
            {
                //减速速阶段
                if ((Velocity.x > 0 && input.MoveDir == -1) || (Velocity.x < 0 && input.MoveDir == 1) || input.MoveDir == 0 ||
                    (IsGround() && input.v < 0) || Mathf.Abs(Velocity.x) > MoveSpeed)
                {
                    introDir = Velocity.x > 0 ? 1 : -1;
                    moveH = Mathf.Abs(Velocity.x);
                    
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
                    Velocity.x = moveH * introDir;
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
                            Velocity.x += MoveSpeed / 6;
                        }
                        else
                        {
                            Velocity.x += MoveSpeed / 15f;
                        }
                        if (Velocity.x > MoveSpeed)
                            Velocity.x = MoveSpeed;
                    }
                    else if (input.MoveDir == -1)
                    {
                        if (IsGround())
                        {
                            Velocity.x -= MoveSpeed / 6;
                        }
                        else
                        {
                            Velocity.x -= MoveSpeed / 12f;
                        }
                        if (Velocity.x < -MoveSpeed)
                            Velocity.x = -MoveSpeed;
                    }
                }
  
            }
        }
                
        bool isCanMove()
        {
            return playState != PlayState.Dash && playState != PlayState.Climb && isCanControl && isMove;
        }
        
        //角色面朝方向
        DirType lastDir;
        void CheckDir()
        {
            if (playState == PlayState.Climb || playState == PlayState.Dash)
                return;
            lastDir = playDir;
            if (input.MoveDir > 0)
            {
                playDir = DirType.Right;
            }
            else if (input.MoveDir < 0)
            {
                playDir = DirType.Left;
            }
            if(lastDir != playDir)
            {
                transform.localScale = new Vector3(GetDirInt, 1, 1);
            }
        }

        /// <summary>
        /// 在地面上
        /// </summary>
        /// <returns></returns>
        public bool IsGround()
        {
            return _moveCollider.IsGround;
        }

        /// <summary>
        /// 改变移动状态
        /// </summary>
        /// <param name="pState"></param>
        public void ChangeState(PlayState pState)
        {
            playState = pState;
        }
        
        //玩家朝向的int值（1为right， -1为left）
        public int GetDirInt
        {
            get{
                return playDir == DirType.Right ? 1 : -1;
            }
        }
        
        /// <summary>
        /// 检测并修正水平方向的位移
        /// </summary>
        bool FixHorizon;
        void CheckFixedHorizontalMove()
        {
            if (FixHorizon)
                return;
            HorizontalBox = playDir == DirType.Right ? _moveCollider.RightBox : _moveCollider.LeftBox;
            if (HorizontalBox.Length == 1)
            {
                var pointDis = HorizontalBox[0].point.y - Center.y;
                if (pointDis > 0.34f)
                {
                    var offsetPos = Mathf.Ceil(rig.position.y);
                    rig.position = new Vector2(rig.position.x, offsetPos - 0.22f);
                }
                else if (pointDis < -0.42f)
                {
                    var offsetPos = Mathf.Ceil(rig.position.y);
                    rig.position = new Vector2(rig.position.x, offsetPos + 0.035f);
                }
                FixHorizon = true;
            }
        }
        
        /// <summary>
        /// 检测并修正垂直方向的位移
        /// </summary>
        bool CheckFixedUpMove()
        {
            if (_moveCollider.UpBox.Length == 1)
            {
                var pointDis = _moveCollider.UpBox[0].point.x - Center.y;
                if (pointDis > 0.34f)
                {
                    var offsetPos = Mathf.Floor(rig.position.x);
                    rig.position = new Vector2(offsetPos + 0.48f, rig.position.y);
                    return true;
                }
                else if (pointDis < -0.34f)
                {
                    var offsetPos = Mathf.Floor(rig.position.x);
                    rig.position =  new Vector2(offsetPos + 0.52f, rig.position.y);
                    return true;
                }
                else
                {
                    Velocity.y = 0;
                    ChangeState(PlayState.Fall);
                    return false;
                }
            }
            return true;
        }
        
        bool BoxCheckCanClimb()
        {
            if (_moveCollider.RightBox.Length > 0)
            {
                HorizontalBox = _moveCollider.RightBox;
            }
            else if (_moveCollider.LeftBox.Length > 0)
            {
                HorizontalBox = _moveCollider.LeftBox;
            }
            return _moveCollider.RightBox.Length > 0 || _moveCollider.LeftBox.Length > 0;
        }
        
        /// <summary>
        /// 检测是周围是否有墙壁，既是否可以爬墙。
        /// </summary>
        /// <returns></returns>
        bool BoxCheckCanClimbDash()
        {
            _moveCollider.BoxCheckCanClimbDash();
            if (_moveCollider.RightBox.Length > 0)
            {
                HorizontalBox = _moveCollider.RightBox;
            }
            else if (_moveCollider.LeftBox.Length > 0)
            {
                HorizontalBox = _moveCollider.LeftBox;
            }
            return _moveCollider.RightBox.Length > 0 || _moveCollider.LeftBox.Length > 0;
        }
        
        public Vector2 Center
        {
            get
            {
                return _moveCollider.Center;
            }
        }

        public Vector2 Size
        {
            get
            {
                return _moveCollider.Size;
            }
        }
    }
}