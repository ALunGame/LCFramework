namespace LCECS.Server.Layer
{
    public interface IRequestServer
    {
        void Init();
        void PushRequest(int entityId, RequestId reqId);
    }
}
