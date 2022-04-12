using LCECS.Data;

namespace LCECS.Server.Layer
{
    public interface IDecisionServer
    {
        void Init();

        void AddDecisionEntity(int decId, EntityWorkData workData);

        void RemoveDecisionEntity(int decId, int entityId);

        void Execute();
    }
}
