using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasser : MonoBehaviour
{
    private float _laserSpeed = 8f; 
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
        if (transform.position.y >= 8.0f)
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

        if (transform.position.y < -8.0f)
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

 /*   void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = Other.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
        }
    }
*/
}
