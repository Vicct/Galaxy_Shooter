using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allien_Laser : MonoBehaviour
{
    private float _enemyLaserSpeed = 8f;
    private Vector3 _laserPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _laserPosition = GetComponent<Transform>().position;
        transform.position += _laserPosition.normalized * _enemyLaserSpeed * Time.deltaTime; 
        if (transform.position.y < -6.0f | transform.position.x > 8.0f)
        {     
            Destroy(this.gameObject);
        }
    }

}
