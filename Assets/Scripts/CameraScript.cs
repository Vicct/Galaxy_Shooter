using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float _shakeDuration = 0.5f; // Duration of the shake effect
    public float _shakeMagnitude = 0.1f; // Magnitude of the positional shake
    public float _shakeSpeed = 1.0f; // Speed of the shake effect
    private Vector3 _originalPosition;
    private float _currentShakeDuration = 0f;
    private float _currentShakeMagnitude = 0f;
    private float _currentShakeSpeed = 0f;
    

    void Start()
    {
        _originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (_currentShakeDuration > 0)
        {
            transform.localPosition = _originalPosition + Random.insideUnitSphere * _currentShakeMagnitude;
            _currentShakeDuration -= Time.deltaTime * _currentShakeSpeed;
        }
        else
        {
            _currentShakeDuration = 0f;
            transform.localPosition = _originalPosition;
        }
        
    }

    public void Shake()
    {
        _currentShakeDuration = _shakeDuration;
        _currentShakeMagnitude = _shakeMagnitude;
        _currentShakeSpeed = _shakeSpeed;
    }





}
