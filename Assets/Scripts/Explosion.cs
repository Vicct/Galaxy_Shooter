using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private CameraScript _cameraShake;
    void Start()
    {
        _cameraShake = GameObject.Find("MainCamera").GetComponent<CameraScript>();
        _cameraShake.Shake();
        Destroy(this.gameObject, 3.0f);
    }
}
