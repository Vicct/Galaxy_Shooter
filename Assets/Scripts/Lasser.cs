using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasser : MonoBehaviour
{
    private float _laserSpeed = 8f; 

    void Start()
    { 
    }

    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y >= 8.0f)
        {     
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

}
