using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    public GameObject EnemyContainer;
    public bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _healthPrefab;
    private bool _healthActive = false;

    public void startspawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());

    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 2)
        {
            StartCoroutine(SpawnHealthRoutine());
        }
        else 
        {   
            _healthActive = false;            
        }
    }
    IEnumerator SpawnEnemyRoutine()
    {
       yield return new WaitForSeconds(1.0f);
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
       yield return new WaitForSeconds(3.0f);
       while (_stopSpawning == false)  
        {
            Vector3 _powerUpPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
            int randomPowerUp = UnityEngine.Random.Range(0,5);
            Debug.Log("Number randomPowerUp is: " + randomPowerUp);

            Transform SpawnManager = transform;
            Transform EnemyContainer = SpawnManager.GetChild(0);
            int childCount = EnemyContainer.childCount;
            Debug.Log("Number of child objects: " + childCount);

            if(randomPowerUp == 4 && childCount >= 4 )
            {
                Instantiate(_powerUps[randomPowerUp], _powerUpPos, Quaternion.identity);
                yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
            }
            else if (randomPowerUp != 4)
            {
                Instantiate(_powerUps[randomPowerUp], _powerUpPos, Quaternion.identity);
                yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
            }     
        }
    }

    IEnumerator SpawnHealthRoutine()
    {
       _healthActive = true;
       yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
       while (_healthActive == true && _stopSpawning == false)  
        {
            Vector3 _healtPrefabPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
            Instantiate(_healthPrefab, _healtPrefabPos, Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
