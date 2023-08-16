using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.tag == "Laser")
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(Other.gameObject);
                _spawnManager.startspawning();
                Destroy(this.gameObject, 0.3f);
            }
    }

}
