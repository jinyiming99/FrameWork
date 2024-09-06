using UnityEngine;
public class SceneAnchor: MonoBehaviour
{
    public enum SceneAnchorType
    {
        Start,
        Position,
        End,
    };

    [SerializeField] private SceneAnchorType _type = SceneAnchorType.Position;
    public SceneAnchorType Type => _type;
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position,0.5f);
    }
}
