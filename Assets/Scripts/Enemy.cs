using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private UIManager _uimanager;
    
    private Player _player;

    private Animator _EnemyExplodeAnim;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The player is Null");
        }
        _EnemyExplodeAnim = GetComponent<Animator>();

        if (_EnemyExplodeAnim == null)
        {
            Debug.LogError("The Enemy Explode is null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.0f & _player != null)
        {
            float randomX = UnityEngine.Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX,7,0);
        }
    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        
        if(Other.tag == "Player")
            {
                //Player player = Other.transform.GetComponent<Player>();
                if (_player != null)
                {
                    _player.Damage();  
                }
                
                _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
                _speed = 1.0f;
                Destroy(this.gameObject, 2.3f);
            }
        if(Other.tag == "Laser")
            {
                if(_player != null)
                    {
                        //player.UpdateScore(); //it does not work - Object reference not set to an instance of an object.
                        _player.UpdateScore(10); //Question about the reference to an object.
                    }
                
                _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
                _speed = 1.0f;
                Destroy(this.gameObject, 2.3f);
                Destroy(Other.gameObject);
            }
    }
}
