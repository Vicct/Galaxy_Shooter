using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private UIManager _uiManager; 
    private Player _player;
    private Animator _EnemyExplodeAnim;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _explodeAudio;
    private AudioSource _audioSource;



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
        _audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        Movement();

        if(Time.time > _canFire && this.gameObject != null)
        {
            _fireRate = UnityEngine.Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab,new Vector3(transform.position.x - 0.19f,transform.position.y + 0.1f,0), Quaternion.identity);
            Lasser[] lassers = enemyLaser.GetComponentsInChildren<Lasser>();
            for( int i = 0; i < lassers.Length; i++ )
            {
                lassers[i].AssignEnemyLaser();
                _audioSource.clip = _laserAudio;
                _audioSource.Play();
            }
        }
       
    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y < -5.0f & _player != null)
        {
            float randomX = UnityEngine.Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX,7,0);
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        
        if(Other.tag == "Player")
        {
            //Player player = Other.transform.GetComponent<Player>();
            if (_player != null)
            {
                _player.Damage();  
            }
            
            _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.clip = _explodeAudio;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            
            Destroy(this.gameObject, 2.8f);
        }
        if(Other.tag == "Laser")
        {
            if(_player != null)
                {
                    //player.UpdateScore(); //it does not work - Object reference not set to an instance of an object.
                    _player.UpdateScore(10); //Question about the reference to an object.
                }
            
            _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.clip = _explodeAudio;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.3f);
            Destroy(Other.gameObject);
        }
    }
}
