using Demo.Com;
using Demo.Config;
using Demo.Info;
using LCECS;
using LCECS.Core;
using LCECS.Data;
using LCHelp;
using LCSkill;
using LCUI;
using UnityEngine;

namespace Demo
{
    public class GameStart : MonoBehaviour
    {
        public bool DrawGizmos = false;
        public GameObject PlayerStart;

        public bool LastReqStop = false;
        private Entity PlayerEntity;

        private void Awake()
        {
            TempConfig.Init();
        }

        private void Start()
        {
            //创建玩家实体
            GameObject entityGo = null;
            ECSLocate.Player.CreatePlayerEntity(1001, ref entityGo);
            entityGo.transform.position = PlayerStart.transform.position;
            PlayerEntity = ECSLocate.Player.GetPlayerEntity();

            //创建全局系统
            ECSLocate.ECS.CreateEntity(1003, new GameObject("MapSystem"));
            ECSLocate.ECS.CreateEntity(1002, new GameObject("CameraSystem"));
            ECSLocate.ECS.CreateEntity(1004, new GameObject("EffectSystem"));
        }

        private void Update()
        {
            //时间线
            TimeLineHelp.Update();
            ComputeVelocity();
            OnClickNormalAttack();
        }

        protected void ComputeVelocity()
        {

            #region 点击加速
            // Vector2 move=Vector2.zero;
            // if (Input.GetKeyDown(KeyCode.A))
            // {
            //     move.x = 1;
            // }
            // if (Input.GetButtonDown("Jump"))
            // {
            //     move.y = 1;
            // }

            // ParamData paramData = ECSLocate.Player.GetReqParam(EntityReqId.PlayerMove);
            // paramData.SetVect2(move);
            // ECSLocate.Player.PushPlayerReq(EntityReqId.PlayerMove);

            #endregion

            #region 按键速度

            Vector2 move = Vector2.zero;
            bool dash = false;

            move.x = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
            {
                move.y = 1;
            }

            if (Input.GetMouseButtonDown(0))
            {
                dash = true;
            }

            //没有移动请求
            if (move == Vector2.zero && dash == false)
            {
                //上一次也是没有移动请求
                if (LastReqStop)
                {
                    return;
                }
                ParamData paramData = ECSLocate.Player.GetReqParam(1);
                paramData.SetVect2(move);
                paramData.SetBool(dash);
                ECSLocate.Player.PushPlayerReq(1);
                LastReqStop = true;
            }
            else
            {
                LastReqStop = false;
                ParamData paramData = ECSLocate.Player.GetReqParam(1);
                paramData.SetVect2(move);
                paramData.SetBool(dash);
                ECSLocate.Player.PushPlayerReq(1);
            }

            #endregion
        }

        //普通攻击
        private void OnClickNormalAttack()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ParamData paramData = ECSLocate.Player.GetReqParam(2);
                paramData.SetBool(true);
                ECSLocate.Player.PushPlayerReq(2);
            }
        }


        private void OnDrawGizmos()
        {
            if (DrawGizmos == false)
            {
                return;
            }
            if (!Application.isPlaying)
            {
                return;
            }
            PlayerSensor playerInfo = ECSLayerLocate.Info.GetSensor<PlayerSensor>(SensorType.Player);

            //碰撞检测
            DrawPlayerColliderLine();
            
            //地图检测
            DrawMapCheckBox(playerInfo.GetPlayerPos());

            //背景检测
            DrawBgCheckBox(playerInfo.GetPlayerPos());
        }

        //显示碰撞射线
        private void DrawPlayerColliderLine()
        {
            PlayerPhysicsCom playerPhysicsCom = PlayerEntity.GetCom<PlayerPhysicsCom>();
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(playerPhysicsCom.Rig2D.position + playerPhysicsCom.BottomCheckPoint,playerPhysicsCom.CollisionRadius);
            Gizmos.DrawSphere(playerPhysicsCom.Rig2D.position + playerPhysicsCom.RightCheckPoint,playerPhysicsCom.CollisionRadius);
            Gizmos.DrawSphere(playerPhysicsCom.Rig2D.position + playerPhysicsCom.LeftCheckPoint,playerPhysicsCom.CollisionRadius);
            Gizmos.color = Color.white;
        }
        
        private void DrawMapCheckBox(Vector3 playerPos)
        {
            Vector3 pos = new Vector3(playerPos.x - ((float)TempConfig.MapSizeX / 2), playerPos.y - ((float)TempConfig.MapSizeY / 2));
            Vector3 size = new Vector3(TempConfig.MapSizeX, TempConfig.MapSizeY);
            Rect mapCheck = new Rect(pos, size);
            EDGizmos.DrawRect(mapCheck, Color.green);
        }

        private void DrawBgCheckBox(Vector3 playerPos)
        {
            Vector3 pos = new Vector3(playerPos.x - 45, -10);
            Vector3 size = new Vector3(90, 20);
            Rect bgCheck = new Rect(pos, size);
            EDGizmos.DrawRect(bgCheck, Color.blue);
        }

    }
}
