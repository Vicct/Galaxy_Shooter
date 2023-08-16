using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float _enemyLaserSpeed = 8f;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemyLaserSpeed * Time.deltaTime);
        if (transform.position.y < -6.0f  || transform.position.x > 10.0f || transform.position.x < -10.0)
        {     
            Destroy(this.gameObject);
        }
    }
}
