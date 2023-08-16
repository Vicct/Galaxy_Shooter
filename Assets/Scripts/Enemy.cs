using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float _speed = 4.0f;
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
    [SerializeField]
    private GameObject _enemyShield;
    private float _shieldEnable = 0;
    private float _shieldRate = 2.0f;
    private bool _isEnemyShieldActive;
    //Ram variables
    private float _distance;
    private Vector3 _direction;
    private float _attackRange = 2.0f; 
    private float _ramSpeedMultiplier = 1.2f;
    //Health destroy
    private GameObject _healthPowerUp;
    private float _originOffset = 1.0f;
    private float _raycastMaxDistance = 5.0f;
    private float _distanceToLaser;
    private float _defenseRange = 3.0f;
    private Vector3 _avoidDirection;


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
        _enemyShield.SetActive(false);
    }

    void Update()
    {
        //LaserFire();
        EnemyVerticalMovement();
        EnemyShield();
        EnemyRam();
        HealthDestroy();
        SmartShoot();
        AvoidLaser();
    }

    void LaserFire()
    {
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

    void EnemyVerticalMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5.0f & _player != null )
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
                _audioSource.clip = _explodeAudio;
                _audioSource.Play();  
            }
            
            if (_isEnemyShieldActive == true)
            {
                return;
            }
            else
            {
                _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
                _speed = 0.0f;
                _audioSource.clip = _explodeAudio;
                _audioSource.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }
        else if (Other.tag == "Laser")
        {
            if(_isEnemyShieldActive == true)
            {
                Destroy(Other.gameObject);
                return;
            }
            if(_player != null)
                {
                    _player.UpdateScore(10); 
                }
            _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            _audioSource.clip = _explodeAudio;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.3f);
            Destroy(Other.gameObject);
        }
        else if (Other.gameObject.name == "Missile(Clone)")
        {
            if(_player != null)
            {
                _player.UpdateScore(10); 
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

    public void BombDestroy()
    {
        if(_player != null)
            {
                _player.UpdateScore(10); 
            }
        
        _EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
        _speed = 0.0f;
        _audioSource.clip = _explodeAudio;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 1.3f);
    }

    void EnemyShield()
    {
        if(Time.time > _shieldEnable && this.gameObject != null)
        {
            _shieldRate = UnityEngine.Random.Range(1.0f, 3.0f);
            _shieldEnable = Time.time + _shieldRate;
            _enemyShield.SetActive(true);
            StartCoroutine(ShieldActive());
            _isEnemyShieldActive = true;
        }
    }
    IEnumerator ShieldActive()
    {
        yield return new WaitForSeconds(4.0f);
        _enemyShield.SetActive(false);
        _isEnemyShieldActive = false;
    }

    void EnemyRam()
    {
        if(_player != null)
        {
            _distance = Vector3.Distance(_player.transform.position, this.transform.position);

            if (_distance <= _attackRange)
            {
                _direction = this.transform.position - _player.transform.position;
                _direction = _direction.normalized;
                this.transform.position -= _direction * Time.deltaTime * (_speed * _ramSpeedMultiplier);
            }
        }
    }

    void SmartShoot()
    {
        if(_player !=null)
        {
            Vector3 _playerPosition = _player.transform.position;
            _direction = this.transform.position - _player.transform.position;
            bool _playerDetectedInFront = Vector3.Dot(transform.up, _direction) > 0.0f;
            if(Time.time > _canFire && this.gameObject != null)
            {
                _fireRate = UnityEngine.Random.Range(3.0f, 7.0f);     
                _canFire = Time.time + _fireRate;

                if (_playerDetectedInFront)
                {

                    GameObject enemyLaser = Instantiate(_enemyLaserPrefab,new Vector3(transform.position.x - 0.185f,transform.position.y - 0.05f,0), Quaternion.identity);
                    Lasser[] lassers = enemyLaser.GetComponentsInChildren<Lasser>();
                    for( int i = 0; i < lassers.Length; i++ )
                    {
                        lassers[i].AssignEnemyLaser();
                        _audioSource.clip = _laserAudio;        
                        _audioSource.Play();
                    }

                }
                else
                {
                    GameObject enemyLaser = Instantiate(_enemyLaserPrefab,new Vector3(transform.position.x - 0.17f,transform.position.y + 1.75f,0), Quaternion.identity);
                    enemyLaser.GetComponentsInChildren<Lasser>();
                    _audioSource.clip = _laserAudio;
                    _audioSource.Play();
                }
            }
        }
    }

    void HealthDestroy()
    {
        _healthPowerUp = GameObject.Find("Health_Powerup(Clone)");
        if(_healthPowerUp != null && this.gameObject != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - _originOffset), Vector2.down, _raycastMaxDistance);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - _originOffset), Vector2.down*_raycastMaxDistance, Color.red);
            
            if (hit.collider != null && hit.collider.gameObject.name == _healthPowerUp.name && Time.time > _canFire)
            {
                _fireRate = UnityEngine.Random.Range(1.0f, 3.0f);     
                _canFire = Time.time + _fireRate;
                
                Debug.Log("hit  :" + hit.collider.gameObject.name + "_healthPowerUp  :"  + _healthPowerUp.name);
                Debug.Log("Time.time > _canFire  :" + _canFire);

                GameObject enemyLaser = Instantiate(_enemyLaserPrefab,new Vector3(transform.position.x - 0.185f,transform.position.y - 0.05f,0), Quaternion.identity);
                Lasser[] lassers = enemyLaser.GetComponentsInChildren<Lasser>();
                for( int i = 0; i < lassers.Length; i++ )
                {
                    lassers[i].AssignEnemyLaser();
                    _audioSource.clip = _laserAudio;        
                    _audioSource.Play();
                }
            }

            RaycastHit2D hitback = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + _originOffset), Vector2.up, _raycastMaxDistance);
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + _originOffset), Vector2.up*_raycastMaxDistance, Color.blue);
            
            if (hitback.collider != null && hitback.collider.gameObject == _healthPowerUp && Time.time > _canFire)
            {
                _fireRate = UnityEngine.Random.Range(1.0f, 3.0f);     
                _canFire = Time.time + _fireRate;
                
                Debug.Log("hitback  :" + hitback.collider.gameObject.name);
                Debug.Log("Time.time > _canFire  :" + _canFire);

                GameObject enemyLaser = Instantiate(_enemyLaserPrefab,new Vector3(transform.position.x - 0.17f,transform.position.y + 1.75f,0), Quaternion.identity);
                enemyLaser.GetComponentsInChildren<Lasser>();
                _audioSource.clip = _laserAudio;
                _audioSource.Play();
                
            }
        }
    }

    void AvoidLaser()
    {
        GameObject _laser = GameObject.Find("Lasser(Clone)");
        if(_laser != null)
        {
            _distanceToLaser = Vector3.Distance(_laser.transform.position, this.transform.position);

            if (_distanceToLaser <= _defenseRange)
            {
                _avoidDirection = this.transform.position - _laser.transform.position;
                _avoidDirection = _avoidDirection.normalized;
                this.transform.position += _avoidDirection * Time.deltaTime * (_speed * 3);
            }
        }
    }
}

