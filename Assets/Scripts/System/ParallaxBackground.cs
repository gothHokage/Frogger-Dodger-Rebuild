// ParallaxBackground.cs
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public SpriteRenderer spriteRenderer;
        [Range(0f, 1f)] public float parallaxFactor;
    }

    [SerializeField] private ParallaxLayer[] _layers;
    [SerializeField] private Transform _player;

    private Vector3 _lastPlayerPos;

    private void Start()
    {
        if (_player == null)
            _player = GameObject.FindWithTag("Player")?.transform;

        _lastPlayerPos = _player.position;
    }

    private void LateUpdate()
    {
        if (_player == null) return;

        float deltaX = _player.position.x - _lastPlayerPos.x;
        float deltaY = _player.position.y - _lastPlayerPos.y;

        foreach (var layer in _layers)
        {
            if (layer.spriteRenderer == null) continue;
            layer.spriteRenderer.transform.position += new Vector3(
                deltaX * layer.parallaxFactor,
                deltaY * layer.parallaxFactor,
                0f);
        }

        _lastPlayerPos = _player.position;
    }
}