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
    private Rigidbody2D rb;
    private float horizontalMovement = 0f;
    private readonly float snapEpsilon = 0.01f;
    private int CurrentDir => (int)input.Player.Move.ReadValue<float>();
    void Awake()
    {
        input ??= new();
        rb = GetComponent<Rigidbody2D>();
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
        //Debug.Log($"Speed => {horizontalMovement * speedMove}");
        Debug.Log($"Direction => {CurrentDir}");
        if (CheckDown())
            Jump(forceJump);
    }

    public void Move()
    {
        float _target = input.Player.Move.ReadValue<float>();
        float _rate = 1f - Mathf.Exp(-speedLerp * Time.deltaTime);
        float _nextMove = Mathf.Lerp(horizontalMovement, _target, _rate);
        horizontalMovement = Mathf.Abs(horizontalMovement - _target) < snapEpsilon ? _target : _nextMove;
        rb.linearVelocityX = horizontalMovement * speedMove;
    }

    public void Jump(float _force)
    {
        Debug.Log("Jump");
        rb.linearVelocityY = 0f;
        rb.AddForceY(_force);
    }
    public bool CheckDown()
    {
        if (rb.linearVelocityY >= 0f) return false;

        RaycastHit2D _hit = Physics2D.BoxCast(rb.position + checkOffset, checkSize, 0, Vector2.zero, 0, layerPlatform);
        return _hit;
    }

    private void OnDrawGizmos()
    {
        if (rb)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(rb.position + checkOffset, checkSize);
        }
    }
}
