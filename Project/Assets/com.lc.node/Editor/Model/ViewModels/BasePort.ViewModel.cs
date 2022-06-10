using System;
using System.Collections.Generic;
using System.Linq;
using LCToolkit.ViewModel;
using LCJson;

namespace LCNode.Model
{
    public partial class BasePort : ViewModel, IGraphElement
    {
        #region Fields
        [NonSerialized] BaseNode owner;
        [NonSerialized] List<BaseConnection> connections;
        [NonSerialized] Func<BaseConnection, BaseConnection, int> comparer;

        public event Action<BaseConnection> onConnected;
        public event Action<BaseConnection> onDisconnected;
        public event Action onSorted;
        #endregion

        #region Properties
        public BaseNode Owner { get { return owner; } }
        public IReadOnlyCollection<BaseConnection> Connections { get { return connections; } }
        #endregion

        internal void Enable(BaseNode node)
        {
            owner = node;
            switch (orientation)
            {
                case Orientation.Horizontal:
                    connections = new List<BaseConnection>();
                    comparer = HorizontalComparer;
                    break;
                case Orientation.Vertical:
                    connections = new List<BaseConnection>();
                    comparer = VerticalComparer;
                    break;
            }
            OnEnabled();
            RefreshIndex();
        }

        #region API
        public void ConnectTo(BaseConnection connection)
        {
            connections.Add(connection);
            Resort();
            onConnected?.Invoke(connection);
            RefreshIndex();
        }

        public void DisconnectTo(BaseConnection connection)
        {
            connections.Remove(connection);
            Resort();
            onDisconnected?.Invoke(connection);
            RefreshIndex();
        }

        /// <summary> 强制重新排序 </summary>
        public void Resort()
        {
            connections.QuickSort(comparer);
            onSorted?.Invoke();
            RefreshIndex();
        }

        public void RefreshIndex()
        {
            if (!setIndex)
                return;
            if (connections == null || connections.Count <= 0)
                return;
            if (direction == Direction.Input)
                RefreshInIndex();
            else if (direction == Direction.Output)
                RefreshOutIndex();
        }

        private void RefreshOutIndex()
        {
            for (int i = 0; i < connections.Count; i++)
            {
                BaseConnection connect = connections[i];
                BaseNode node = connect.ToNode;
                if (node != null)
                {
                    node.inIndex = i;
                    if (node.OnTitleChanged != null)
                    {
                        node?.OnTitleChanged(node.Title);
                    }
                }
            }
        }

        private void RefreshInIndex()
        {
            for (int i = 0; i < connections.Count; i++)
            {
                BaseConnection connect = connections[i];
                BaseNode node = connect.FromNode;
                if (node != null)
                {
                    node.outIndex = i;
                    if (node.OnTitleChanged != null)
                    {
                        node?.OnTitleChanged(node.Title);
                    }
                }
            }
        }


        #endregion

        #region Overrides
        protected virtual void OnEnabled() { }
        #endregion

        #region Static
        private int VerticalComparer(BaseConnection x, BaseConnection y)
        {
            // 若需要重新排序的是input接口，则根据FromNode排序
            // 若需要重新排序的是output接口，则根据ToNode排序
            var nodeX = direction == Direction.Input ? x.FromNode : x.ToNode;
            var nodeY = direction == Direction.Input ? y.FromNode : y.ToNode;

            // 则使用x坐标比较排序
            // 遵循从左到右
            if (nodeX.Position.x < nodeY.Position.x)
                return -1;
            if (nodeX.Position.x > nodeY.Position.x)
                return 1;

            // 若节点的x坐标相同，则使用y坐标比较排序
            // 遵循从上到下
            if (nodeX.Position.y < nodeY.Position.y)
                return -1;
            if (nodeX.Position.y > nodeY.Position.y)
                return 1;

            return 0;
        }

        private int HorizontalComparer(BaseConnection x, BaseConnection y)
        {
            // 若需要重新排序的是input接口，则根据FromNode排序
            // 若需要重新排序的是output接口，则根据ToNode排序
            var nodeX = direction == Direction.Input ? x.FromNode : x.ToNode;
            var nodeY = direction == Direction.Input ? y.FromNode : y.ToNode;

            // 则使用y坐标比较排序
            // 遵循从上到下
            if (nodeX.Position.y < nodeY.Position.y)
                return -1;
            if (nodeX.Position.y > nodeY.Position.y)
                return 1;

            // 若节点的y坐标相同，则使用x坐标比较排序
            // 遵循从左到右
            if (nodeX.Position.x < nodeY.Position.x)
                return -1;
            if (nodeX.Position.x > nodeY.Position.x)
                return 1;

            return 0;
        }
        #endregion
    }
}
