using System.Collections.Generic;
using UnityEngine;

public class PlatformPooling : MonoBehaviour
{
    public static PlatformPooling Instance;

    public List<GameObject> platformPrefabs;
    public List<PlatformHandler> bufferPlatformRest = new();
    public List<PlatformHandler> bufferPlatformUsed = new();

    public Transform parentRest;
    public Transform parentUsed;

    [Tooltip("Percent from width screen to apply to platform width"), Range(1, 50)] public float MinWidthPercent;
    [Tooltip("Percent from width screen to apply to platform width"), Range(1, 50)] public float MaxWidthPercent;

    [Tooltip("Percent from height screen to apply to platform height"), Range(1, 20)] public float MinHeightPercent;
    [Tooltip("Percent from height screen to apply to platform height"), Range(1, 20)] public float MaxHeightPercent;
    public float MinPlatformWidth => GameManager.Instance.WidthCamSize * (MinWidthPercent * .01f);
    public float MaxPlatformWidth => GameManager.Instance.WidthCamSize * (MaxWidthPercent * .01f);

    public float MaxPlatformHeight => GameManager.Instance.HeightCamSize * (MinHeightPercent * .01f);
    public float MinPlatformHeight => GameManager.Instance.HeightCamSize * (MaxHeightPercent * .01f);

    public void Init()
    {
        if (Instance == null)
            Instance = this;

        parentRest = new GameObject("Platform Rest").transform;
        parentRest.SetParent(transform);
        parentUsed = new GameObject("Platform Used").transform;
        parentUsed.SetParent(transform);
    }
    public void Start()
    {
        CreatePlatform();
        CreatePlatform();
        CreatePlatform();
    }

    private void CreatePlatform()
    {
        if (platformPrefabs == null) return;

        GameObject _platform = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Count)], parentRest);
        _platform.TryGetComponent(out PlatformHandler _platformHandler);

        if (_platformHandler)
        {
            DisablePlatform(_platformHandler);
            Vector2 _size = new(Random.Range(MinPlatformWidth, MaxPlatformWidth), Random.Range(MinPlatformHeight, MaxPlatformHeight));
            _platformHandler.SetSize(_size);
        }
        else
            Destroy(_platform);
    }

    public void GetPlatform(Vector2 _pos)
    {
        if (bufferPlatformRest.Count > 0)
        {
            PlatformHandler _platform = bufferPlatformRest[^1];
            _platform.transform.position = _pos;
            EnablePlatform(_platform);
        }
    }

    public void EnablePlatform(PlatformHandler _platform)
    {
        if (bufferPlatformRest.Contains(_platform))
            bufferPlatformRest.Remove(_platform);

        if (!bufferPlatformUsed.Contains(_platform))
            bufferPlatformUsed.Add(_platform);

        _platform.Enable();
        _platform.transform.SetParent(parentUsed);
    }

    public void DisablePlatform(PlatformHandler _platform)
    {
        if (bufferPlatformUsed.Contains(_platform))
            bufferPlatformUsed.Remove(_platform);

        if (!bufferPlatformRest.Contains(_platform))
            bufferPlatformRest.Add(_platform);

        _platform.Disable();
        _platform.transform.SetParent(parentRest);
    }
}
