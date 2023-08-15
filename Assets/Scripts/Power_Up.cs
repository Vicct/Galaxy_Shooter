using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Up : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    [SerializeField]
    private int _powerUpID;
    [SerializeField]
    private AudioClip _powerUpaudioclip;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Player _player;
    
    private int _rayCount = 8; // Number of rays to cast
    private float _rayLength = 3.0f; // Length of each ray
    [SerializeField]
    private float _originOffset = 0.0f; //avoiding _player's width
    private float _distance;//distance from the _player to the power-up
    private float _distanceRange = 3.0f;
    private Vector3 _directionToPowerUp;

 
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        if(transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
        CatchPowerUpsKey();
    }
    private void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerUpaudioclip, transform.position);
            Player _player = Other.transform.GetComponent<Player>();
            if(_player != null)
            {
                switch(_powerUpID)
                {
                    case 0:
                        _player.TripleshotisActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        _player.ShieldisActive();
                        break;
                    case 3:
                        _player.BulletsRefill();
                        break;
                    case 4:
                        _player.UnDamage();
                        break;
                    case 6:
                        _player.ShootMissileKey();
                        break;
                }
            }
            Destroy(this.gameObject);
        }

        if(Other.tag == "Laser" && _powerUpID == 5)
        {
            Player _player = GameObject.Find("Player").GetComponent<Player>();
            _player.ShieldisActive();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(Other.gameObject);  
            Transform Bomb_Power_Up = transform;
            Transform Bomb_Effect = Bomb_Power_Up.GetChild(0);
            BlastWave _blastWave = Bomb_Effect.GetComponent<BlastWave>();
            if (_blastWave != null)
            {
                _blastWave.ExplodeBomb();
            }
            Destroy(this.gameObject, 0.3f);
        }

        if(Other.tag == "EnemyLaser" && _powerUpID == 4 || Other.tag == "Enemy" && _powerUpID == 4)
        {
            Destroy(Other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);  
            Destroy(this.gameObject, 0.3f);
        }
    }

    private void CatchPowerUpsKey()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            CatchPowerUps();
        };
        
        if (Input.GetKey(KeyCode.C))
        {
            CatchPowerUps();
        };
    }


    private void CatchPowerUps()
    {    
        GameObject _player = GameObject.Find("Player");
        if(_player != null && this.gameObject != null && _powerUpID != 4)
        {
            for (int i = 0; i < _rayCount; i++)
            {
                float angle = i * 360f / _rayCount; // Calculate angle for each ray
                // Convert angle to direction vector
                Vector3 _direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
                // Cast the ray and check for collisions
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + _originOffset, transform.position.y + _originOffset), _direction, _rayLength);
                if (hit.collider != null && hit.collider.gameObject.name == "Player")
                {
                    // Do something with the hit information (e.g., print the name of the hit object)
                    Debug.Log("Raycast hit " + hit.collider.gameObject.name + " at " + hit.point);
                    _distance = Vector3.Distance(hit.collider.transform.position, this.transform.position);

                    Debug.Log("_distance  : " + _distance);
                    if(_distance <= _distanceRange)
                    {
                        _directionToPowerUp = this.transform.position - hit.collider.transform.position;
                        _directionToPowerUp = _directionToPowerUp.normalized;
                        this.transform.position -= _directionToPowerUp * Time.deltaTime * (_powerUpSpeed * 4);
                    }
                }
                // Visualize the ray for debugging purposes (optional)
                Debug.DrawRay(new Vector2(transform.position.x + _originOffset, transform.position.y + _originOffset), _direction * _rayLength, Color.red);
            }
        }    
    }
}
