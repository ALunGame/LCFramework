using LCNode.Model;
using UnityEditor.Experimental.GraphView;

namespace LCNode.View
{
    public partial class BasePortView
    {
        public BasePortView(BasePort port, IEdgeConnectorListener connectorListener) : this(
            orientation: port.orientation == BasePort.Orientation.Horizontal ? Orientation.Horizontal : Orientation.Vertical,
            direction: port.direction == BasePort.Direction.Input ? Direction.Input : Direction.Output,
            capacity: port.capacity == BasePort.Capacity.Single ? Capacity.Single : Capacity.Multi,
            port.type, connectorListener)
        {

        }
    }
}
