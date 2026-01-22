using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private float speedLerp = 1f;
    [SerializeField] private float speedMove = 1f;
    [SerializeField] private float forceJump = 1f;

    [Header("Checker")]
    [SerializeField] private Vector2 checkOffset = Vector2.down;
    [SerializeField] private Vector2 checkSize = new(1, .2f);
    [SerializeField] private LayerMask layerPlatform;

    private InputController input;
    private float horizontalMovement = 0f;

    private Vector2 velocity = Vector2.zero;
    private readonly float snapEpsilon = 0.01f;
    private int CurrentDir => (int)input.Player.Move.ReadValue<float>();
    void Awake()
    {
        input ??= new();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckDown();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
        velocity += Physics2D.gravity * Time.deltaTime;
    }
    private bool CheckDown()
    {
        if (velocity.y >= 0f) return false;

        RaycastHit2D _hit = Physics2D.BoxCast(transform.position + (Vector3)checkOffset, checkSize, 0, Vector2.zero, 0, layerPlatform);
        if (_hit)
        {
            _hit.transform.gameObject.TryGetComponent(out IJumpable _jumper);
            _jumper?.OnAnyOtherLand(this);
        }

        return _hit;
    }

    public void Move()
    {
        float _target = input.Player.Move.ReadValue<float>();
        float _rate = 1f - Mathf.Exp(-speedLerp * Time.deltaTime);
        float _nextMove = Mathf.Lerp(horizontalMovement, _target, _rate);
        horizontalMovement = Mathf.Abs(horizontalMovement - _target) < snapEpsilon ? _target : _nextMove;
        velocity.x = horizontalMovement * speedMove;
    }

    public void Jump(float _force = 1)
    {
        velocity.y = 0f;
        velocity.y += _force * forceJump;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position + (Vector3)checkOffset, checkSize);
    }
}
