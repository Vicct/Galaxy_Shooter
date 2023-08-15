using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaShotter : MonoBehaviour
{

    private float _downwardSpeed = 1.0f;  // Speed of downward movement

    private bool _isMovingDownward = true;
    private bool _isMovingUpwnward = false;
    private bool _isRotation = false;

    private float _stopVerticalDownDuration = 10f;
    private float _stopVerticalUpDuration = 5f;
    private float _stopVerticalDownTimer = 0f;
    private float _stopVerticalUpTimer = 0f;
    private float _rotationSpeed = 0.5f;
    private GameObject _player;

    public GameObject _laserPrefab;
    public float _shootForce = 1f;
    public int _numberOfLasers = 16; // Number of lasers to shoot
    [SerializeField]
    private float _angleShot;
    
    private float _timeShot;
    private float _stopShotDuration = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        this.transform.position = new Vector3(0.0f,8.0f,0f);
    }

// Update is called once per frame
    void Update()
    {  
        EnemyMovement();
    }

    void EnemyMovement()
    {     
        if(_isMovingDownward)
        {
            this.transform.Translate(Vector3.down * _downwardSpeed * Time.deltaTime);
            if (_stopVerticalDownTimer >= _stopVerticalDownDuration)
            {
                // Stop circular movement and start downward movement
                _isMovingDownward = false;
                _isMovingUpwnward = true;
                _isRotation = false;
                _stopVerticalDownTimer = 0f;
            }
            else
            {
                // Increment the stop timer
                _stopVerticalDownTimer += Time.deltaTime;
            }
        }
        else if (_isMovingUpwnward)
        {
            // Move downward
            this.transform.Translate(Vector3.up * _downwardSpeed * Time.deltaTime);
            if (_stopVerticalUpTimer >= _stopVerticalUpDuration)
            {
                // Stop circular movement and start downward movement
                _isMovingUpwnward = false;
                _isRotation = true;
                _stopVerticalUpTimer = 0f;
            }
            else
            {
                // Increment the stop timer
                _stopVerticalUpTimer += Time.deltaTime;
            }
        }
        else if (_isRotation)
        {
            // Calculate the new rotation angles
            float newYRotation = transform.eulerAngles.y + _rotationSpeed * Time.deltaTime;
            float newZRotation = transform.eulerAngles.z + _rotationSpeed * Time.deltaTime;

            // Apply the new rotation while keeping X rotation unchanged
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);

            if (_timeShot >= _stopShotDuration)
            {
                _isRotation = false;
            }
            else
            {
                ShootLasers();
                _timeShot += Time.deltaTime;
            }
            if (_player != null)
            {
                // Stop circular movement and start downward movement
                _isRotation = true;
            }
            else
            {
                _isRotation = false;
            }
        }

    }

    void ShootLasers()
    {
        float _angleStep = _angleShot / _numberOfLasers;

        for (int i = 0; i < _numberOfLasers; i++)
        {
            float _angle = i * _angleStep;
            Quaternion _rotation = Quaternion.Euler(0f, 0f, 90f+_angle);
            Vector2 direction = _rotation * Vector2.up;

            GameObject _laser = Instantiate(_laserPrefab, transform.position, _rotation);
            Rigidbody2D _laserRigidbody = _laser.GetComponent<Rigidbody2D>();
            
            if (_laserRigidbody != null)
            {
                _laserRigidbody.AddForce(direction * _shootForce, ForceMode2D.Impulse);
            }

            if (_laser.transform.position.y >= 8.0f || _laser.transform.position.x <= -8.0f || _laser.transform.position.x >= 8.0f)
            {     
                if(transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                Destroy(this.gameObject);
            }
        }

    }
}
