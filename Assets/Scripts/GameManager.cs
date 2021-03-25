using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    void Update()
    {

        if(Input.GetKey(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(0); //Current game scene
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        
    }
    public void GameOver()
    {
        _isGameOver = true;
    }

}
