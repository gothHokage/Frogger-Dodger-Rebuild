using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        G.CameraShake = this;
    }

    public void Shake(float force)
    {
        _impulseSource.GenerateImpulse(force);
    }
}