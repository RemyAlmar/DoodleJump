using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedLerp = 1f;
    [SerializeField] private float speedMove = 1f;

    private InputController input;
    private float horizontalMovement = 0f;
    private float snapEpsilon = 0.01f;
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
        //Debug.Log($"Speed => {horizontalMovement * speedMove}");
        Debug.Log($"Direction => {CurrentDir}");
    }

    public void Move()
    {
        float _target = input.Player.Move.ReadValue<float>();
        float _rate = 1f - Mathf.Exp(-speedLerp * Time.deltaTime);
        float _nextMove = Mathf.Lerp(horizontalMovement, _target, _rate);
        horizontalMovement = Mathf.Abs(horizontalMovement - _target) < snapEpsilon ? _target : _nextMove;
    }
}
