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
    private int _currentWaveIndex = 0;
    private int _enemyCount = 2;
    private bool _stopWave = false;
    private UIManager _uimanager;
    private Player _player;
    private int _countPowerUp = 0;
    private GameObject _enemies;
    private int _childCount;

    void Start()
    {
        _uimanager = FindObjectOfType<UIManager>();
        _player = FindObjectOfType<Player>();

    }

    void Update()
    {
        SpawnMegaShoter();
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
        if (_stopSpawning == false)
        {
            bool IsEven(int number)
            {
                return number % 2 == 0;
            }

            for (int e = 0; e < _waves.Length; e++)
            {
                Debug.Log("e :" + e);
                _currentWaveIndex = e + 1;
                _uimanager.WaveUpdate(_currentWaveIndex);
                  
                if (IsEven(_currentWaveIndex))
                {
                    for (int i = 0; i < _enemyCount; i++)
                    {
                        int j = i + 1;
                        _uimanager.EnemyCount( j, _enemyCount);
                        Vector3 _enemyPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
                        GameObject _newEnemy = Instantiate(_enemyPrefab[1], _enemyPos, Quaternion.identity);  
                        _newEnemy.transform.parent = EnemyContainer.transform;
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
                        _uimanager.WaveUpdate(_currentWaveIndex);
                        int j = i + 1;
                        _uimanager.EnemyCount(j, _enemyCount);
                        Vector3 _enemyPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
                        GameObject _newEnemy = Instantiate(_enemyPrefab[0], _enemyPos, Quaternion.identity);  
                        _newEnemy.transform.parent = EnemyContainer.transform;
                        yield return new WaitForSeconds(5.0f);
                        if (_stopSpawning == true)
                        {
                            Debug.Log("Loop interrupted at i = " + i);
                            break;
                        }
                    }
                }
                yield return new WaitForSeconds(10.0f);
            }
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
       yield return new WaitForSeconds(3.0f);
       while (_stopSpawning == false | _stopWave == false)  
        {
            Vector3 _powerUpPos = new Vector3(UnityEngine.Random.Range(-8f, 8f), 8.0f, 0);
            int randomPowerUp = UnityEngine.Random.Range(0,7);

            Transform SpawnManager = transform;
            Transform EnemyContainer = SpawnManager.GetChild(0);
            int childCount = EnemyContainer.childCount;

            if(randomPowerUp == 4 && childCount >= 4 )
            {
                Instantiate(_powerUps[randomPowerUp], _powerUpPos, Quaternion.identity);
                yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
            }
            else if (randomPowerUp != 4 && randomPowerUp != 5)
            {
                Instantiate(_powerUps[randomPowerUp], _powerUpPos, Quaternion.identity);
                yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
            }
            else if (randomPowerUp == 5)
            {
                _countPowerUp = _countPowerUp + 1;
                if(_countPowerUp == 3)
                {
                    Instantiate(_powerUps[randomPowerUp], _powerUpPos, Quaternion.identity);
                    yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
                    _countPowerUp = 0;
                }
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

    void SpawnMegaShoter()
    {
        Transform SpawnManager = transform;
        Transform EnemyContainer = SpawnManager.GetChild(0);
        _childCount = EnemyContainer.childCount;


        if(_currentWaveIndex == _waves.Length && _childCount == 0)
        {
            Debug.Log("_currentWaveIndex   :"+  _currentWaveIndex);
            Debug.Log("_waves.Length   :"+  _waves.Length);
            Vector3 _enemyMegaPos = new Vector3(0.0f,8.0f,0f);
            GameObject _newMegaEnemy = Instantiate(_enemyPrefab[2], _enemyMegaPos, Quaternion.identity);
            _newMegaEnemy.transform.parent = EnemyContainer.transform;
        }

        if(_enemyPrefab[2] == null)
        {
            _uimanager.GameOverSequence();
            _stopSpawning = true;
            _stopWave = true;  
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
