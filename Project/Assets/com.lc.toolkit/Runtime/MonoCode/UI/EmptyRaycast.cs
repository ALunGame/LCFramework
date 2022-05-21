using UnityEngine.UI;
using UnityEngine;

namespace LCToolkit
{
    /// <summary>
    /// �յ��¼�������
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyRaycast : MaskableGraphic
    {
        protected EmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    } 
}
