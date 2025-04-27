using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

   //[SerializeField]
   private float _speed;
   private float _speedPower = 4.0f;
   private float _speedMultiplier = 3.0f;   
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
   private GameObject _shielVisualizer, _shielVisualizerMedium, _shielVisualizerHigh;
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
   private int _shieldCount = 0;
   private float _timeCoroutine = 5.0f;
   private int _activeCoroutine = 1;
   private int _bullets = 15;
   private int _maxBullets;
   //Thurster variables
   [SerializeField]
   private float _powerupThrustersWaitTimeLimit = 3.0f;
   [SerializeField]
   private float _thrusterChargeLevelMax = 10.0f;
   [SerializeField]
   private float _thrusterChargeLevel;
   [SerializeField]
   private float _changeDecreaseThrusterChargeBy = 1.5f;
   [SerializeField]
   private float _changeIncreaseThrusterChargeBy = 0.01f;
   [SerializeField]
   private bool _canUseThrusters = true;
   [SerializeField]
   private bool _thrustersInUse = false;
   private CameraScript _cameraShake;
   [SerializeField]
   private GameObject _shielCryptoEffectVisualizer;


   [SerializeField]
   private GameObject _missile; 
   [SerializeField]
   private int _missiles = 5;
   [SerializeField]
   private AudioClip __missileAudio;
   private AudioSource _missileAudioSource;
   private bool isMissileTrue = false;


    void Start()
    {
        transform.position = new Vector3(0,-3,0);
        _rightDamage.SetActive(false);
        _leftDamage.SetActive(false);
        _shielVisualizer.SetActive(false);
        _shielVisualizerMedium.SetActive(false);
        _shielVisualizerHigh.SetActive(false);
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
        _cameraShake = GameObject.Find("MainCamera").GetComponent<CameraScript>();

        _maxBullets = _bullets;
        _shielCryptoEffectVisualizer.SetActive(false);
        _speed = _speedPower;

    }
    void Update()
    {
        Movements ();     
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire) 
        {
            ShootLaser();
        }
        ThrustersChargeLevel();
        ThrustersAcceleration();
        DetectCrypto();

        //temporal
        if(Input.GetKeyDown(KeyCode.C))
        {
            _uiManager.KeyCPressed();
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            _uiManager.KeyCPressedDone();
        }
        //temporal
        if(Input.GetKeyDown(KeyCode.X))
        {
            _uiManager.KeyXPressed();
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            _uiManager.KeyXPressedDone();
        }

        if(Input.GetKeyDown(KeyCode.X) && isMissileTrue == true)
        {
            ShootMissile();
        }
    }

    void ThrustersChargeLevel()
    {
        _thrusterChargeLevel = Mathf.Clamp(_thrusterChargeLevel, 0, _thrusterChargeLevelMax);

        if (_thrusterChargeLevel <= 0.0f)
        {
            _canUseThrusters = false;
        }
        else if (_thrusterChargeLevel >= (_thrusterChargeLevelMax/0.75f))
        {
            _canUseThrusters = true;
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

    void ThrustersAcceleration()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canUseThrusters)
        {
            _speed = _speedPower * _speedMultiplier;
            _thrustersInUse = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speedPower;
            _thrustersInUse = false;
        }

        if(_thrustersInUse)
        {
            ThrustersActive();
        }
        else if(!_thrustersInUse)
        {
            StartCoroutine(ThrusterPowerReplenisRoutine());
        }
    }
    void ShootLaser()
    {
      _canFire = Time.time + _fireRate;
      
        if (_istripeShootActive == true && _bullets >= 1)
        {
            Instantiate(_tripleShootPrefab, _tripleShootPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            _bullets = _bullets - 3;
        }

        else if (_bullets >= 1)
        {
            Instantiate(_laserPrefab, _laserPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, 0) , Quaternion.identity); 
            _bullets = _bullets - 1;
        }
        _audioSource.clip = _laserAudio;
        _audioSource.Play();
        ShootCounter();
    }

    public void Damage()
    {
        if(_isShieldActive == true)
            {
                return;
            }

        _cameraShake.Shake();
        _lives --;
        if(_lives == 2)
            {
                _rightDamage.SetActive(true);
            }
        else if(_lives == 1)
            {
                _leftDamage.SetActive(true);
            }

        if (_lives <= 0)
        {         
            _lives = 0;
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject,0.3f);
        }
        _uiManager.UpdateLives(_lives);
        _spawnManager.UpdateLives(_lives);
        
    }

    public void UnDamage()
    {
        _lives = _lives + 1;
        _uiManager.UpdateLives(_lives);
        _spawnManager.UpdateLives(_lives);
        if(_lives == 2)
            {
                _rightDamage.SetActive(false);
            }
        else if(_lives == 3)
            {
                _leftDamage.SetActive(false);
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
       _speed = _speedPower *_speedMultiplier;
       Debug.Log("_speed  :" + _speed);
       StartCoroutine(SpeedBoostPowerDownCoroutine());
    }


    public void BulletsRefill()
    {
        if (_bullets < 0)
        {
            _bullets = 0;
            _bullets = _bullets + 30;
            _maxBullets = _maxBullets + 30;
        }
        else
        {
            _bullets = _bullets + 30;
            _maxBullets = _maxBullets + 30;
        }
    }

    IEnumerator SpeedBoostPowerDownCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = _speedPower /_speedMultiplier;
        Debug.Log("_speed after :" + _speed);
        //_isSpeedBoostActive = false;
    }

    public void ShieldisActive()
    {

       _shieldCount ++;
       _activeCoroutine = 1;

        if (_shieldCount == 1)
        {
            _shielVisualizer.SetActive(true);           
            _shielVisualizerMedium.SetActive(false);
            _shielVisualizerHigh.SetActive(false);
            _isShieldActive = true;
            StartCoroutine(ShieldPowerDownCoroutine(_timeCoroutine, _shieldCount));
        }
        else if(_shieldCount == 2 && _activeCoroutine == 1)
        {
            StopAllCoroutines();
            print("Stopped " + Time.time);
            _shielVisualizer.SetActive(false);
            _shielVisualizerMedium.SetActive(true);
            _shielVisualizerHigh.SetActive(false);
            _isShieldActive = true;
            StartCoroutine(ShieldPowerDownCoroutine(_timeCoroutine * 2, _shieldCount));

        }
        else if(_shieldCount == 3 && _activeCoroutine == 1)
        {
            StopAllCoroutines();
            print("Stopped " + Time.time);
            _shielVisualizer.SetActive(false);
            _shielVisualizerMedium.SetActive(false);
            _shielVisualizerHigh.SetActive(true);
            _isShieldActive = true;
            StartCoroutine(ShieldPowerDownCoroutine(_timeCoroutine * 3, _shieldCount));
        }
        else
        {
            _shieldCount = 0;
            _activeCoroutine = 0;
        }
    }

    IEnumerator ShieldPowerDownCoroutine(float wait, int _shielCount)
    {
        yield return new WaitForSeconds(wait);
        _shielVisualizer.SetActive(false);
        _shielVisualizerMedium.SetActive(false);
        _shielVisualizerHigh.SetActive(false);
        _isShieldActive = false;
        _shieldCount = 0;
        _activeCoroutine = 0;
    }

    public void UpdateScore(int _points)
    {
        _score += _points;
        _uiManager.UpdateScorex(_score);
    }

    public void ShootCounter()
    {
        _uiManager.UpdateShootScore(_bullets, _maxBullets);
    } 

    private void OnTriggerEnter2D(Collider2D Other)
    { 
        if(Other.tag == "EnemyLaser")
        {
           _countLaser = _countLaser + 1;
           Destroy(Other.gameObject);
    
            if (_countLaser < 2)
            {
                //Debug.LogError(_countLaser);
            }
            else if (_countLaser == 2)
            {
                _audioSource.clip = _explodeAudio;
                _audioSource.Play();
                Damage();
                _countLaser = 0;
            }
        }

    }

    private void ThrustersActive()
    {
        if(_canUseThrusters == true)
        {
            _thrusterChargeLevel -= Time.deltaTime * _changeDecreaseThrusterChargeBy;
            _uiManager.UpdateThrustersSlider(_thrusterChargeLevel);// Method to update the Thursters slide.
            _uiManager.UpdateThrusterScore(_thrusterChargeLevel);
            if(_thrusterChargeLevel <= 0)
            {
                _uiManager.ThursterSliderUsableColor(false);
                _thrustersInUse = false;
                _canUseThrusters = false;
                _speed = 0; //Resetspeed();
            }
        }
    }

    IEnumerator ThrusterPowerReplenisRoutine()
    {
        yield return new WaitForSeconds(_powerupThrustersWaitTimeLimit);
        while(_thrusterChargeLevel <= _thrusterChargeLevelMax && !_thrustersInUse)
        {
            yield return null;
            _thrusterChargeLevel += Time.deltaTime * _changeIncreaseThrusterChargeBy;
            _uiManager.UpdateThrustersSlider(_thrusterChargeLevel);
            _uiManager.UpdateThrusterScore(_thrusterChargeLevel);
        }
        if (_thrusterChargeLevel >= _thrusterChargeLevelMax)
        {
            _uiManager.ThursterSliderUsableColor(true);
            _canUseThrusters = true;
        }
    }

    private void DetectCrypto()
    {
        GameObject _kryptogameobject = GameObject.Find("Crypto_Rock Variant(Clone)");
        if (_kryptogameobject == null)
        {
            _fireRate = 0.5f;
            _shielCryptoEffectVisualizer.SetActive(false);
        }
        else
        {       
            _speed = _speedPower/2;
            _fireRate = 1.5f;
            _shielCryptoEffectVisualizer.SetActive(true);
        }
    }

    public void ShootMissileKey()
    {
        isMissileTrue = true;
        _missiles = 5;
        _uiManager.MissilesUpdate(_missiles);        
    }

    private void ShootMissile()
    {  
        if (_missiles >= 1)
        {
            Instantiate(_missile, _missile.transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            //_missileInstantiation.transform.rotation = Quaternion.Euler(0f,90f,90f);
            _missiles = _missiles - 1;
            _uiManager.MissilesUpdate(_missiles); 
        }
        else if(_missiles == 0)
        {
            isMissileTrue = false;
        }
        _audioSource.clip = __missileAudio;
        _audioSource.Play();

    }

}
