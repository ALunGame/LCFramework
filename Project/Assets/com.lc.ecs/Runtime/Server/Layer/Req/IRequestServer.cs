using LCECS.Data;

namespace LCECS.Server.Layer
{
    public interface IRequestServer
    {
        void Init();
        void PushRequest(string uid, RequestData requestData);
    }
}
