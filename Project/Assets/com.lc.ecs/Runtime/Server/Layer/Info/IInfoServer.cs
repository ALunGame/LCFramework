using LCECS.Core;
using LCECS.Data;
using LCECS.Layer.Info;

namespace LCECS.Server.Layer
{
    public interface IInfoServer
    {
        void Init();

        T GetSensor<T>(SensorType key) where T : ISensor;

        void AddEntityWorkData(int entityId, EntityWorkData data);

        EntityWorkData GetEntityWorkData(int entityId);

        void RemoveEntityWorkData(int entityId);
    }
}
