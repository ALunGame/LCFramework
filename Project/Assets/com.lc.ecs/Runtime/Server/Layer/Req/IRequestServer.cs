namespace LCECS.Server.Layer
{
    public interface IRequestServer
    {
        void Init();
        int GetRequestWeight(int reqId);
        void PushRequest(int entityId, int reqId);
    }
}
