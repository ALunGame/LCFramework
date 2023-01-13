using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace LCECS.Core
{
    public class TransData
    {
        public bool setPos = false;
        public Vector3 pos;

        public bool setRoate = false;
        public Vector3 roate;

        public bool setScale = false;
        public Vector3 scale;
    }

    public class TransCom : BaseCom
    {
        public Vector3 InitPos { get; private set; }
        public Vector3 InitRoate { get; private set; }
        public Vector3 InitScale { get; private set; }

        public Vector3 Pos { get; private set; }
        public Vector3 Roate { get; private set; }
        public Vector3 Scale { get; private set; }

        [NonSerialized]
        private Transform trans;

        [NonSerialized]
        private TransData transData = new TransData();

        protected override void OnAwake(Entity pEntity)
        {
            BindGoCom bindGoCom = pEntity.GetCom<BindGoCom>();
            if (bindGoCom != null)
            {
                bindGoCom.RegGoChange(OnBindGoChange);
            }
        }

        private void OnBindGoChange(GameObject pGo)
        {
            trans = pGo.transform;
            pGo.transform.position = Pos;
            pGo.transform.localEulerAngles = Roate;
            pGo.transform.localScale = Scale;

            InitPos = Pos;
            InitRoate = Roate;
            InitScale = Scale;
        }

        public void SetPos(Vector3 pPos,bool pJustData = false)
        {
            Pos = pPos;
            if (trans != null && !pJustData)
            {
                trans.position = Pos;
            }
        }

        public void SetRoate(Vector3 pRoate,bool pJustData = false)
        {
            Roate = pRoate;
            if (trans != null && !pJustData)
                trans.localEulerAngles = Roate;
        }

        public void SetScale(Vector3 pScale,bool pJustData = false)
        {
            Scale = pScale;
            if (trans != null && !pJustData)
                trans.localScale = Scale;
        }

        public void UpdateTrans()
        {
            Pos = trans.position;
            Roate = trans.localEulerAngles;
            Scale = trans.localScale;
        }

        /// <summary>
        /// 等待系统执行位置设置
        /// </summary>
        /// <param name="pPos"></param>
        public void WaitSetPos(Vector3 pPos)
        {
            transData.setPos = true;
            transData.pos = pPos;
        }

        /// <summary>
        /// 获得等待的设置位置
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public bool HasWaitPos(out Vector3 pPos,bool pClear = true)
        {
            if (!transData.setPos)
            {
                pPos = Vector3.zero;
                return false;
            }
            if (pClear)
                transData.setPos = false;
            pPos = transData.pos;
            return true;
        }

        /// <summary>
        /// 等待系统执行旋转设置
        /// </summary>
        /// <param name="pPos"></param>
        public void WaitSetRoate(Vector3 pRoate)
        {
            transData.setRoate = true;
            transData.roate = pRoate;
        }

        /// <summary>
        /// 获得等待的设置旋转
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public bool HasWaitRoate(out Vector3 pRoate, bool pClear = true)
        {
            if (!transData.setRoate)
            {
                pRoate = Vector3.zero;
                return false;
            }
            if (pClear)
                transData.setRoate = false;
            pRoate = transData.roate;
            return true;
        }

        /// <summary>
        /// 等待系统执行缩放设置
        /// </summary>
        /// <param name="pPos"></param>
        public void WaitSetScale(Vector3 pScale)
        {
            transData.setScale = true;
            transData.scale = pScale;
        }

        /// <summary>
        /// 获得等待的设置缩放
        /// </summary>
        /// <param name="pPos"></param>
        /// <returns></returns>
        public bool HasWaitScale(out Vector3 pScale, bool pClear = true)
        {
            if (!transData.setScale)
            {
                pScale = Vector3.zero;
                return false;
            }
            if (pClear)
                transData.setScale = false;
            pScale = transData.scale;
            return true;
        }

        public void UpdateWaitTransData()
        {
            if (transData.setPos)
                SetPos(transData.pos);
            if (transData.setRoate)
                SetRoate(transData.roate);
            if (transData.setScale)
                SetScale(transData.scale);
            ClearWaitTransData();
        }

        public void ClearWaitTransData()
        {
            transData.setPos = false;
            transData.setRoate = false;
            transData.setScale = false;
        }
    }
}
