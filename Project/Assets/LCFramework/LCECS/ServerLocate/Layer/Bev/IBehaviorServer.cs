using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace LCECS.Server.Layer
{
    public interface IBehaviorServer
    {
        void Init();

        Node GetBevNode(int bevId);

        void PushBev(EntityWorkData workData);

        void Execute();
    }
}
