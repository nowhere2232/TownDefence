using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float _minSize = 3f;
    [SerializeField] private float _maxSize = 10f;
    [SerializeField] private float _speed = 1f;

    private Camera _camera;

    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        var mouse = Mouse.current;
        if (mouse == null)
            return;

        float scroll = mouse.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) >  0.01f)
        {
            float size = _camera.orthographicSize - scroll * _speed * 0.01f;
            _camera.orthographicSize = Mathf.Clamp(size, _minSize, _maxSize);
        }
    }
}
