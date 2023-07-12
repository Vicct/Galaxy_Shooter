using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOscript : MonoBehaviour
{
    [SerializeField]
    public float _speed = 0.001f;
    private UIManager _uiManager; 
    private Player _player;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _explodeAudio;
    private AudioSource _audioSource;
    private Vector3 _enemyInitialPosition;
    private float _circleRadius;
    private float _angle = 0f;
    private Vector3 _originOffset;
    private int _randomAttackDirection;
    private float _attackDirection;
    private Lasser _ufoLaser;
    [SerializeField]
    private GameObject _explosionPrefab;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The player is Null");
        }

        _audioSource = GetComponent<AudioSource>();

        _enemyInitialPosition = transform.localPosition; //enemy initial position
        _originOffset = new Vector3(UnityEngine.Random.Range(5,-5),8,0);
        _circleRadius = UnityEngine.Random.Range(3f,10f);
        _randomAttackDirection = UnityEngine.Random.Range(1,3);
        if (_randomAttackDirection == 1)
        {
            _attackDirection = -1;
        }
        else if (_randomAttackDirection == 2)
        {
            _attackDirection = 1;
        }
        //_ufoLaser = GameObject.Find("Lasser").GetComponent<Lasser>();

    }

    void Update()
    {
        LaserFire();
        EnemyCircularMovement();
    }

    void LaserFire()
    {
        if(Time.time > _canFire && this.gameObject != null)
        {
            _fireRate = UnityEngine.Random.Range(2.0f, 4.0f);
            _canFire = Time.time + _fireRate;
            GameObject _ufoShot = Instantiate(_laserPrefab,new Vector3(transform.position.x,transform.position.y - 1.0f,0), Quaternion.identity);
            Lasser[] _ufoLaser = _ufoShot.GetComponentsInChildren<Lasser>();
            for( int i = 0; i < _ufoLaser.Length; i++ )
            {
                _ufoLaser[i].AssignEnemyLaser();
                _audioSource.clip = _laserAudio;
                _audioSource.Play();
            }
        }
    }

    void EnemyCircularMovement()
    {        
        _angle += _attackDirection * Time.deltaTime;

        float x = Mathf.Cos(_angle) * _circleRadius + _originOffset.x;
        float y = Mathf.Sin(_angle) * _circleRadius + _originOffset.y;
        transform.position = new Vector3(x, y , 0f);
        Vector3 newPosition = _enemyInitialPosition + transform.position;
        transform.Translate(newPosition * _speed);     
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        
        if(Other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();  
            }
            
            //_EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0.0f;
            _audioSource.clip = _explodeAudio;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
        }
        else if (Other.tag == "Laser")
        {
            if(_player != null)
                {
                    //player.UpdateScore(); //it does not work - Object reference not set to an instance of an object.
                    _player.UpdateScore(20); //Question about the reference to an object.
                }
            
            //_EnemyExplodeAnim.SetTrigger("OnEnemyDeath");
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _speed = 0.0f;
            _audioSource.clip = _explodeAudio;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject);
            Destroy(Other.gameObject);
        }

    }

    public void BombDestroy()
    {
        if(_player != null)
            {
                _player.UpdateScore(20); 
            }
        
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _speed = 0.0f;
        _audioSource.clip = _explodeAudio;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject);
    }
}
