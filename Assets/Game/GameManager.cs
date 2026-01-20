using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Camera cam;

    [SerializeField] private float percentScreenToScroll;
    private float WidthCamSize => HeightCamSize * cam.aspect;
    private float HalfWidthCamSize => WidthCamSize * .5f;
    private float HeightCamSize => cam.orthographicSize * 2;

    void Awake()
    {
        if (!cam)
            cam = Camera.main;

        SpriteRenderer _spriteRend = player.transform.GetComponentInChildren<SpriteRenderer>();
        CreateSiblingPlayer(_spriteRend, WidthCamSize);
        CreateSiblingPlayer(_spriteRend, -WidthCamSize);
    }

    private void Update()
    {
        WrapAroundScreen();
    }

    private void CreateSiblingPlayer(Component _comp, float _posX)
    {
        GameObject _other = Instantiate(_comp.gameObject, player.transform);
        _other.transform.localPosition = new(_posX, 0, 0);
    }
    public void WrapAroundScreen()
    {
        if (IsInScreenHorizontal()) return;

        Vector2 _newPosPlayer = player.transform.position;
        if (player.transform.position.x > HalfWidthCamSize + cam.transform.position.x)
            _newPosPlayer.x = cam.transform.position.x - HalfWidthCamSize;
        else if (player.transform.position.x < -HalfWidthCamSize + cam.transform.position.x)
            _newPosPlayer.x = cam.transform.position.x + HalfWidthCamSize;

        player.transform.position = _newPosPlayer;
    }
    private bool IsInScreenHorizontal()
    {
        return !(player.transform.position.x > HalfWidthCamSize + cam.transform.position.x || player.transform.position.x < -HalfWidthCamSize + cam.transform.position.x);
    }


}
