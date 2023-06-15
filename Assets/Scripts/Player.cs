using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

   [SerializeField]
   private float _speed = 3.5f;
   private float _speedMultiplier = 2.0f;
   [SerializeField]
   private GameObject _laserPrefab; 
   [SerializeField]
   private float _fireRate = 0.5f;
   private float _canFire = -1f;
   [SerializeField]
   private int _lives = 3;
   private SpawnManager _spawnManager;
   [SerializeField]
   private GameObject _tripleShootPrefab;
   private bool _istripeShootActive = false;
   private bool _isShieldActive = false;
   [SerializeField]
   private GameObject _shielVisualizer;
   [SerializeField]
   private int _score;
   private UIManager _uiManager;
   [SerializeField]
   private GameObject _leftDamage;
   [SerializeField]
   private GameObject _rightDamage;
   [SerializeField]
   private AudioClip _laserAudio;
   [SerializeField]
   private AudioClip _explodeAudio;
   private AudioSource _audioSource;
   private int _countLaser = 0;
   [SerializeField]
   private GameObject _explosionPrefab;
 

    void Start()
    {
        transform.position = new Vector3(0,-3,0);
        _rightDamage.SetActive(false);
        _leftDamage.SetActive(false);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        //_spawnManager = FindObjectOfType<SpawnManager>(); 
        if(_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is null");
        }

        //_uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _uiManager = FindObjectOfType<UIManager>(); //What is the difference between those lines?
        if(_uiManager == null)
        {
            Debug.Log("The UI Manager is null");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("The audiosource on the player is null");
        }
        else
        {
            _audioSource.clip = _laserAudio;
        }
        
    }
    void Update()
    {
        Movements ();     
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) 
        {
            ShootLaser();
        }
    }
    void Movements ()
    {
        float _HorizontalInput = Input.GetAxis("Horizontal");       
        float _VerticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(_HorizontalInput, _VerticalInput, 0) * _speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,-9.6f,9.6f), transform.position.y,0);
       if (transform.position.y >= 5.0f)
       {
          transform.position = new Vector3(transform.position.x, 5.0f, 0);
       }
       else if (transform.position.y <= -5.0f)
       {
          transform.position = new Vector3(transform.position.x,-5.0f, 0);
       }
    }
    void ShootLaser()
    {
      _canFire = Time.time + _fireRate;
      
        if (_istripeShootActive == true)
        {
            Instantiate(_tripleShootPrefab, _tripleShootPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
        }

        else
        {
            Instantiate(_laserPrefab, _laserPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, 0) , Quaternion.identity); 
        }
        _audioSource.clip = _laserAudio;
        _audioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldActive == true)
            {
                return;
            }

        _lives --;
        _uiManager.UpdateLives(_lives);
        if(_lives == 2)
            {
                _rightDamage.SetActive(true);
            }
        else if(_lives == 1)
            {
                _leftDamage.SetActive(true);
            }

        if (_lives == 0)
        {         
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject,0.3f);
        }
        
    }
    public void TripleshotisActive()
    {
        _istripeShootActive = true;
        StartCoroutine(Tripleshotpowerdownroutine());
    }

    IEnumerator Tripleshotpowerdownroutine()
    {
        yield return new WaitForSeconds(5.0f);      
        _istripeShootActive = false;
    }

    public void SpeedBoostActive()
    {
       //_isSpeedBoostActive = true;
       _speed *=_speedMultiplier;
       StartCoroutine(SpeedBoostPowerDownCoroutine());
    }

    IEnumerator SpeedBoostPowerDownCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /=_speedMultiplier;
        //_isSpeedBoostActive = false;
    }

    public void ShieldisActive()
    {
        _isShieldActive = true;
        _shielVisualizer.SetActive(true);
        StartCoroutine(ShieldPowerDownCoroutine());
    }

    IEnumerator ShieldPowerDownCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        _shielVisualizer.SetActive(false);
        _isShieldActive = false;
    }

    public void UpdateScore(int points)
    {
        _score += points;
        _uiManager.UpdateScorex(_score);
    }
    //Create a method to add 10 to the score
    //Communicate to the UIManager to update the score

    private void OnTriggerEnter2D(Collider2D Other)
    { 
        if(Other.tag == "Laser")
        {
           _countLaser = _countLaser + 1;
           //Debug.LogError(_countLaser);
           Destroy(Other.gameObject);
    
            if (_countLaser < 2)
            {
                //Debug.LogError(_countLaser);
            }
            else if (_countLaser == 2)
            {
                Debug.LogError("The count is " + _countLaser);
                _audioSource.clip = _explodeAudio;
                _audioSource.Play();
                Damage();
                _countLaser = 0;
            }
        }

    }








}
