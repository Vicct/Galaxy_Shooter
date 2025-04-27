using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private float _missileSpeed = 10.0f; 
    private float _distanceToEnemy;
    private float _distanceToUfo;
    [SerializeField]
    private float _attackRange = 8f;
    private Vector3 _attackDirection;

    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0f,0f,90f);
    }
    
    void Update()
    {
        MoveUp ();
        KillEnemies();
    }

    void MoveUp ()
    {
       
        transform.Translate(Vector3.right * _missileSpeed * Time.deltaTime);
        if (transform.position.y >= 8.0f)
        {     
            Destroy(this.gameObject);
        }
    }

    void KillEnemies()
    {
        GameObject _enemy = GameObject.Find("Enemy(Clone)");
        GameObject _ufo = GameObject.Find("Alien Saucer(Clone)");
        if(_enemy != null)
        {
            _distanceToEnemy = Vector3.Distance(_enemy.transform.position, this.transform.position);
            if (_distanceToEnemy <= _attackRange)
            {
                _attackDirection = this.transform.position - _enemy.transform.position;
                _attackDirection = _attackDirection.normalized;
                this.transform.position -= _attackDirection * Time.deltaTime * (_missileSpeed * 2);
            }
        }
        if(_ufo != null)
        {
            _distanceToUfo = Vector3.Distance(_ufo.transform.position, this.transform.position);     
            if(_distanceToUfo <= _attackRange)
            {
                _attackDirection = this.transform.position - _ufo.transform.position;
                _attackDirection = _attackDirection.normalized;
                this.transform.position -= _attackDirection * Time.deltaTime * (_missileSpeed * 2);                
            }
        }
    }
}
