using Unity.VisualScripting;
using UnityEngine;
public class PlatformHandler : MonoBehaviour, IJumpable
{
    [SerializeField] private Transform children;
    [SerializeField] private Vector2 size = Vector2.one;
    [SerializeField] private float jumpForceMultiplier = 10;

    private BoxCollider2D coll;

    public BoxCollider2D Coll { get { if (!coll) coll = GetComponent<BoxCollider2D>(); return coll; } }


    public void OnAnyOtherLand(PlayerController _pc)
    {
        _pc.Jump(jumpForceMultiplier);
    }

    private void OnValidate()
    {
        if (children != null)
        {
            children.localScale = size;
            Coll.size = size.Abs();
        }
    }
}
