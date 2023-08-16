using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Krypto_Rock : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 2.0f;
    [SerializeField]
    private float _powerUpSpeed = 1.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private int _hitted = 0;
    private Player _player;
    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The player is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        if(transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(Other.gameObject);
            _hitted =_hitted + 1;
            if (_hitted == 5)
            {
                Destroy(this.gameObject);
            }
        }
        else if (Other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();  
            }
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
