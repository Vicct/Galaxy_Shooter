using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasser : MonoBehaviour
{
    private float _laserSpeed = 13f; 
    private bool _isEnemyLaser = false;

    void Start()
    { 
    }

    void Update()
    {
        if ( _isEnemyLaser == true)
        {
            MoveDown();
        }
        else
        {
           MoveUp();
        }
    }

    void MoveUp ()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);
        if (transform.position.y >= 8.0f || transform.position.x > 7.0f || transform.position.x < -7.0)
        {     
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown ()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        if (transform.position.y < -8.0f || transform.position.x > 7.0f || transform.position.x < -7.0)
        {     
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

}
