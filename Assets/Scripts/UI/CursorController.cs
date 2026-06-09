using UnityEngine;
using UnityEngine.InputSystem;

public class CursorController : MonoBehaviour
{
    [SerializeField] private float _autoAimRadius = 5f;
    [SerializeField] private LayerMask _enemyLayer;

    private Camera _cam;
    public Transform CurrentTarget { get; private set; }

    private void Start()
    {
        _cam = Camera.main;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (G.InputManager.CurrentDevice is Gamepad)
            UpdateGamepadCursor();
        else
            UpdateMouseCursor();
    }

    private void UpdateMouseCursor()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 worldPos = _cam.ScreenToWorldPoint(mouseScreen);
        worldPos.z = 0f;
        transform.position = worldPos;

        CurrentTarget = null;
    }

    private void UpdateGamepadCursor()
    {
        var colliders = Physics2D.OverlapCircleAll(
            transform.parent.position,
            _autoAimRadius,
            _enemyLayer
        );

        Transform nearest = null;
        float minDist = float.MaxValue;

        foreach (var col in colliders)
        {
            float dist = Vector2.Distance(transform.parent.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = col.transform;
            }
        }

        CurrentTarget = nearest;
        
        Vector3 targetPos = nearest != null
            ? nearest.position
            : transform.parent.position + Vector3.right;

        targetPos.z = 0f;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 15f);
    }

    private void OnDrawGizmosSelected()
    {
        if (transform.parent == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, _autoAimRadius);
    }
}