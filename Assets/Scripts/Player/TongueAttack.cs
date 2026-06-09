using UnityEngine;
using UnityEngine.InputSystem;

public class TongueAttack : MonoBehaviour
{
    [Header("Tongue")]
    [SerializeField] private float _tongueRange = 8f;
    [SerializeField] private float _catchRadius = 0.4f;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("References")]
    [SerializeField] private Transform _tongueOrigin;
    [SerializeField] private CursorController _cursor;

    [Header("Tongue Visual")]
    [SerializeField] private float _tongueSpeed = 20f;
    [SerializeField] private Color _tongueColor = new Color(1f, 0.3f, 0.3f);
    [SerializeField] private float _tongueWidth = 0.1f;
    [SerializeField] private Sprite _tongueSprite;

    private enum TongueState { Idle, Extending, Retracting }
    private TongueState _state = TongueState.Idle;

    private GameObject _tongueObj;
    private SpriteRenderer _tongueSR;
    private Transform _tongueTR;

    private Vector2 _shootOrigin;
    private Vector2 _shootTarget;
    private float _currentLength;
    private float _maxLength;

    // пойманный враг
    private Transform _caughtEnemy;
    private Collider2D _caughtCollider;

    private void Awake()
    {
        _tongueObj = new GameObject("TongueVisual");
        _tongueObj.transform.SetParent(transform);
        _tongueSR = _tongueObj.AddComponent<SpriteRenderer>();
        _tongueTR = _tongueObj.transform;

        if (_tongueSprite == null)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            _tongueSprite = Sprite.Create(tex,
                new Rect(0, 0, 1, 1),
                new Vector2(0f, 0.5f),
                1f);
        }

        _tongueSR.sprite = _tongueSprite;
        _tongueSR.color = _tongueColor;
        _tongueSR.sortingOrder = 5;
        _tongueObj.SetActive(false);
    }

    private void Update()
    {
        if (G.InputManager.ShootPressed && _state == TongueState.Idle)
        {
            StartShoot();
            G.InputManager.ConsumeShoot();
        }

        UpdateTongue();
    }

    private void StartShoot()
    {
        if (_cursor == null) return;

        _shootOrigin = _tongueOrigin != null
            ? _tongueOrigin.position
            : transform.position;

        if (G.InputManager.CurrentDevice is Gamepad && _cursor.CurrentTarget != null)
        {
            _shootTarget = _cursor.CurrentTarget.position;
            _caughtCollider = _cursor.CurrentTarget.GetComponent<Collider2D>();
        }
        else
        {
            Vector2 dir = ((Vector2)_cursor.transform.position - _shootOrigin).normalized;
            var hit = Physics2D.CircleCast(_shootOrigin, _catchRadius, dir, _tongueRange, _enemyLayer);

            _shootTarget = hit.collider != null
                ? hit.point
                : _shootOrigin + dir * _tongueRange;

            _caughtCollider = hit.collider;
        }

        _caughtEnemy = null;
        _maxLength = Vector2.Distance(_shootOrigin, _shootTarget);
        _currentLength = 0f;
        _state = TongueState.Extending;
        _tongueObj.SetActive(true);
    }

    private void UpdateTongue()
    {
        if (_state == TongueState.Idle) return;

        Vector2 origin = _tongueOrigin != null
            ? _tongueOrigin.position
            : transform.position;

        if (_state == TongueState.Extending)
        {
            _currentLength += _tongueSpeed * Time.deltaTime;

            if (_currentLength >= _maxLength)
            {
                _currentLength = _maxLength;
                
                if (_caughtCollider != null)
                {
                    _caughtEnemy = _caughtCollider.transform;
                    
                    if (_caughtEnemy.TryGetComponent<Rigidbody2D>(out var rb))
                        rb.simulated = false;
                }

                _state = TongueState.Retracting;
            }
        }
        else if (_state == TongueState.Retracting)
        {
            _currentLength -= _tongueSpeed * 1.6f * Time.deltaTime;
            
            if (_caughtEnemy != null)
            {
                Vector2 dir = (_shootTarget - _shootOrigin).normalized;
                Vector2 tip = origin + dir * _currentLength;
                _caughtEnemy.position = new Vector3(tip.x, tip.y, _caughtEnemy.position.z);
            }

            if (_currentLength <= 0f)
            {
                _currentLength = 0f;
                _state = TongueState.Idle;
                _tongueObj.SetActive(false);
                
                if (_caughtEnemy != null)
                {
                    if (_caughtCollider.TryGetComponent<IPoolable>(out var poolable))
                    {
                        if (string.IsNullOrEmpty(poolable.PoolKey))
                        {
                            Destroy(_caughtEnemy.gameObject);
                        }

                        else
                        {
                            G.ObjectPool.Return(poolable.PoolKey, _caughtEnemy.gameObject);
                        }
                        
                    }

                    OnEatInsect();
                    _caughtEnemy = null;
                    _caughtCollider = null;
                }
                return;
            }
        }

        ApplyTongueVisual(origin);
    }

    private void ApplyTongueVisual(Vector2 origin)
    {
        Vector2 dir = (_shootTarget - _shootOrigin).normalized;
        Vector2 tip = origin + dir * _currentLength;
        Vector2 center = (origin + tip) * 0.5f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        _tongueTR.position = new Vector3(center.x, center.y, 0f);
        _tongueTR.rotation = Quaternion.Euler(0f, 0f, angle);
        _tongueTR.localScale = new Vector3(_currentLength, _tongueWidth, 1f);
    }

    private void OnEatInsect()
    {
        Debug.Log("Насекомое съедено!");
    }

    private void OnDrawGizmosSelected()
    {
        if (_tongueOrigin == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_tongueOrigin.position, _catchRadius);
    }
}