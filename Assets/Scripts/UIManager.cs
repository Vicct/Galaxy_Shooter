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
    private Image _liveImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameObject _gameOverLabel;
    private GameManagers _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
       _scoreText.text = "Score:   " + 0;
       _gameOverLabel.gameObject.SetActive(false);
       _gameManager = GameObject.Find("GameManager").GetComponent<GameManagers>();
       if(_gameManager == null)
       {
        Debug.LogError("GameManager is null");
       }
    }

    public void UpdateScorex(int playerScore)
    {
        _scoreText.text = "Score:   " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _liveImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
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


}
