using UnityEngine.EventSystems;

namespace LCToolkit
{
    public enum UnityEventType
    {
        Up,
        Down,
        Click,
        LongClick,

        BeginDrag,
        Drag,
        EndDrag,
    }

    public class UnityEventInfo
    {
        public UnityEventType EventType;
        public PointerEventData EventData;

        public UnityEventInfo(UnityEventType EventType, PointerEventData EventData)
        {
            this.EventType = EventType;
            this.EventData = EventData;
        }
    }
}