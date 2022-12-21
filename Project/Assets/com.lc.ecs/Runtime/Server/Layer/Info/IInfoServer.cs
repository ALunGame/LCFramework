using LCECS.Core;
using LCECS.Data;
using LCECS.Layer.Info;

namespace LCECS.Server.Layer
{
    public interface IInfoServer
    {
        void Init();

        T GetSensor<T>(SensorType key) where T : ISensor;

        void AddEntityWorkData(string uid, EntityWorkData data);

        EntityWorkData GetEntityWorkData(string uid);

        void RemoveEntityWorkData(string uid);
    }
}
