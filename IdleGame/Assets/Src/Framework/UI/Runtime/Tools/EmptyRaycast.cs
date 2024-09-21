using UnityEngine;
using UnityEngine.UI;

//用作ui的空白点击区接收raycast
//不使用image，直接挂载此组件和button即可
//原理是只接收点击，不渲染，可以减少叠加造成的drawcall
[RequireComponent(typeof(CanvasRenderer))]
public class EmptyRaycast : MaskableGraphic
{
    protected EmptyRaycast()
    {
        useLegacyMeshGeneration = false;
    }

    protected override void OnPopulateMesh(VertexHelper v)
    {
        v.Clear();
    }
}
