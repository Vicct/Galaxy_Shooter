using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagers : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1); //Space Shooter scene
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); //Space Shooter quit
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
