using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Handle to text 
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _laserCounter;
    [SerializeField]
    private Image _liveImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameObject _gameOverLabel;
    private GameManagers _gameManager;
    [SerializeField]
    private GameObject _bulletsScoreLabel;
    private bool _bulletLabelCoroutine;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField] 
    private Image _thrusterSliderFill;
    [SerializeField]
    private Text _thrusterText;
    [SerializeField]
    private Text _waveInfo;
    [SerializeField]
    private Text _enemiesInfo;
    [SerializeField]
    private Text _keyCPressedText;

    [SerializeField]
    private Text _keyMPressedText;
   
    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score:   " +   0;
       _laserCounter.text = "Bullets:   " + 0 + "/" + 15;
       _gameOverLabel.gameObject.SetActive(false);
       _gameManager = GameObject.Find("GameManager").GetComponent<GameManagers>();
       if(_gameManager == null)
       {
        Debug.LogError("GameManager is null");
       }
       _bulletsScoreLabel.gameObject.SetActive(true);
       //temporal
       _keyCPressedText.gameObject.SetActive(false);
       _keyMPressedText.gameObject.SetActive(false);
    }

    public void UpdateScorex(int _playerScore)
    {
        _scoreText.text = "Score:   " + _playerScore.ToString();
    }

    public void UpdateShootScore(int _bulletsScore, int _maxBullets)
    {
        if (_bulletsScore <= 0)
        {
           _bulletsScore = 0;
           _laserCounter.text = "Bullets:   " + _bulletsScore.ToString() + "/" + _maxBullets.ToString();
           StartCoroutine(BulletsOverCoroutine ());
        }
        else
        {
            _bulletLabelCoroutine = false;
            _laserCounter.text = "Bullets:   " + _bulletsScore.ToString() + "/" + _maxBullets.ToString();
        }
    }

    public void UpdateLives(int currentLives)
    {
        _liveImg.sprite = _liveSprites[currentLives];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    public void GameOverSequence()
    {
        _gameOverLabel.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        while (true)
        {
            _gameOverLabel.gameObject.SetActive(false);  
            yield return new WaitForSeconds(0.5f);
            _gameOverLabel.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator BulletsOverCoroutine()
    {
        _bulletLabelCoroutine = true;
        while (_bulletLabelCoroutine)
        {
            _bulletsScoreLabel.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _bulletsScoreLabel.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            yield return null;
        }
    }

    public void UpdateThrustersSlider(float _thrustValue) // Method to update the Thursters slide.
    {
        if(_thrustValue >= 0 && _thrustValue <= 10)
        {
            _thrusterSlider.value = _thrustValue;
        }
    }
    public void ThursterSliderUsableColor(bool _usableThrusters)
    {
        if (_usableThrusters)
        {
            _thrusterSliderFill.color = Color.green;
        }
        else if (!_usableThrusters)
        {
            _thrusterSliderFill.color = Color.red;
        }
    }
    public void UpdateThrusterScore(float _thrusterScore)
    {
        int _truncateThrusterScore = Mathf.RoundToInt(_thrusterScore);
        _truncateThrusterScore = _truncateThrusterScore * 10;
        _thrusterText.text = "Thruster "+ _truncateThrusterScore.ToString() + "%";
    }

    public void WaveUpdate(int _waveNumber)
    {
        _waveInfo.text = "Wave:   " + _waveNumber.ToString();
    }

    public void EnemyCount(int i, int _enemyCount)
    {
        _enemiesInfo.text = "Enemies:   " + i.ToString() + "/" + _enemyCount.ToString();
    }

    public void KeyCPressed()
    {
        _keyCPressedText.gameObject.SetActive(true);
    }

    public void KeyCPressedDone()
    {
        _keyCPressedText.gameObject.SetActive(false);
    }

    public void KeyXPressed()
    {
        _keyMPressedText.gameObject.SetActive(true);
    }

    public void KeyXPressedDone()
    {
        _keyMPressedText.gameObject.SetActive(false);
    }

}
