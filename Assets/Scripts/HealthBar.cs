using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _scaleTransform, _target;

    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.position = _target.position + Vector3.up * 2f;
        transform.rotation = _cameraTransform.rotation;
    }

    public void Setup(Transform target)
    {
        _target = target;
    }

    public void SetHealth(int health, int maxHealth)
    {
        float xScale = (float) health / maxHealth;
        xScale = Mathf.Clamp01(xScale);
        _scaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
