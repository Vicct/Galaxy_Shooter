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

    
    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score:   " +   0;
       _laserCounter.text = "Bullets:   " +  15;
       _gameOverLabel.gameObject.SetActive(false);
       _gameManager = GameObject.Find("GameManager").GetComponent<GameManagers>();
       if(_gameManager == null)
       {
        Debug.LogError("GameManager is null");
       }
       _bulletsScoreLabel.gameObject.SetActive(true);
    }

    public void UpdateScorex(int _playerScore)
    {
        _scoreText.text = "Score:   " + _playerScore.ToString();
    }

    public void UpdateShootScore(int _bulletsScore)
    {
        if (_bulletsScore <= 0)
        {
           _bulletsScore = 0;
           _laserCounter.text = "Bullets:   " + _bulletsScore.ToString();
           StartCoroutine(BulletsOverCoroutine ());
        }
        else
        {
            _bulletLabelCoroutine = false;
            _laserCounter.text = "Bullets:   " + _bulletsScore.ToString();
        }
    }

    public void UpdateLives(int currentLives)
    {
        _liveImg.sprite = _liveSprites[currentLives];
        Debug.Log("Current Lives  :" + currentLives );
        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
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

}
