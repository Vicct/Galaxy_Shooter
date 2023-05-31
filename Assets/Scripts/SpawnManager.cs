using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _powerUpPrefab;
    [SerializeField]
    private GameObject _speedPowerUpPrefab;
    public GameObject EnemyContainer;
    public bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerUps;

    
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false) 
        {
            Vector3 _enemyPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, _enemyPos, Quaternion.identity);  
            newEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(5.0f);    
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)  
        {
            Vector3 _powerUpPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
            int randomPowerUp = UnityEngine.Random.Range(0,3);
            Instantiate(_powerUps[randomPowerUp], _powerUpPos, Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));     
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
