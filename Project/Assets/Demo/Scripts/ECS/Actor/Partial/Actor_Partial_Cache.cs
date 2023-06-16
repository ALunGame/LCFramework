using Demo;
using Demo.Com;
using Demo.Com.Move;
using Demo.Life;

namespace LCMap
{
    public partial class Actor
    {
        private BagCom bagCom;
        /// <summary>
        /// 背包组件
        /// </summary>
        public BagCom BagCom
        {
            get
            {
                if (bagCom == null)
                {
                    bagCom = GetOrCreateCom<BagCom>();
                }

                return bagCom;
            }
        }
        
        
        private ActorLifeCom lifeCom;
        /// <summary>
        /// 生活组件
        /// </summary>
        public ActorLifeCom LifeCom
        {
            get
            {
                if (lifeCom == null)
                {
                    lifeCom = GetOrCreateCom<ActorLifeCom>();
                }

                return lifeCom;
            }
        }
        
        private ActorMoveCom moveCom;
        /// <summary>
        /// 移动组件
        /// </summary>
        public ActorMoveCom MoveCom
        {
            get
            {
                if (moveCom == null)
                {
                    moveCom = GetOrCreateCom<ActorMoveCom>();
                }

                return moveCom;
            }
        }
        
        private AnimCom animCom;
        /// <summary>
        /// 动画组件
        /// </summary>
        public AnimCom AnimCom
        {
            get
            {
                if (animCom == null)
                {
                    animCom = GetOrCreateCom<AnimCom>();
                }

                return animCom;
            }
        }
        
        
    }
}