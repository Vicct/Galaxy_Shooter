using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    public GameObject EnemyContainer;
    public bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private GameObject _healthPrefab;
    private bool _healthActive = false;
    [SerializeField]
    private int[] _waves;
    public int _currentWaveIndex = 0;
    private int _enemyCount = 2;
    private bool _stopWave = false;
    private UIManager _uimanager;

    void Start()
    {
        _uimanager = FindObjectOfType<UIManager>();
    }
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
        while (_stopSpawning == false)
        {
            bool IsEven(int number)
            {
                return number % 2 == 0;
            }

            for (int e = 0; e < _waves.Length; e++)
            {
                _currentWaveIndex = e + 1;
                Debug.Log("e  :" + e + "_currentWaveIndex :"  +  _currentWaveIndex);
                _uimanager.WaveUpdate(_currentWaveIndex);  
                if (IsEven(_currentWaveIndex))
                {
                    for (int i = 0; i < _enemyCount; i++)
                    {
                        Debug.Log("e  :" + e + "_currentWaveIndex :"  +  _currentWaveIndex);
                        int j = i + 1;
                        _uimanager.EnemyCount( j, _enemyCount);
                        Debug.Log("i  :" + i);
                        Vector3 _enemyPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
                        GameObject newEnemy = Instantiate(_enemyPrefab[1], _enemyPos, Quaternion.identity);  
                        newEnemy.transform.parent = EnemyContainer.transform;
                        yield return new WaitForSeconds(5.0f);
                        if (_stopSpawning == true)
                        {
                            Debug.Log("Loop interrupted at i = " + i);
                            break;
                        }
                    }
                    _enemyCount = _enemyCount * 2;
                    _stopWave = true;
                }
                else
                {
                    for (int i = 0; i < _enemyCount; i++)
                    {
                        _currentWaveIndex = e + 1;
                        Debug.Log("e  :" + e + "_currentWaveIndex :"  +  _currentWaveIndex);
                        _uimanager.WaveUpdate(_currentWaveIndex);
                        int j = i + 1;
                        _uimanager.EnemyCount(j, _enemyCount);
                        Debug.Log("i  :" + i);
                        Vector3 _enemyPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
                        GameObject newEnemy = Instantiate(_enemyPrefab[0], _enemyPos, Quaternion.identity);  
                        newEnemy.transform.parent = EnemyContainer.transform;
                        yield return new WaitForSeconds(5.0f);
                        if (_stopSpawning == true)
                        {
                            Debug.Log("Loop interrupted at i = " + i);
                            break;
                        }
                    }
                    _enemyCount = _enemyCount * 2;
                    _stopWave = true;        
                }
                yield return new WaitForSeconds(10.0f);
                if(_currentWaveIndex == _waves.Length)
                {
                    _uimanager.GameOverSequence();
                    _stopSpawning = true;
                    _stopWave = true;

                } 
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
       yield return new WaitForSeconds(3.0f);
       while (_stopSpawning == false | _stopWave == false)  
        {
            Vector3 _powerUpPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
            int randomPowerUp = UnityEngine.Random.Range(0,5);

            Transform SpawnManager = transform;
            Transform EnemyContainer = SpawnManager.GetChild(0);
            int childCount = EnemyContainer.childCount;

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
