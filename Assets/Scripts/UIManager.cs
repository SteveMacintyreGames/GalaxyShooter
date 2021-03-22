using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartGameText;

    private bool _canRestart;


    void Awake()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
    }
    void Start()
    {
        scoreText.text = "Score: 0";        
    }

    void Update()
    {
        if(_canRestart)
        {
            if(Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: "+playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOverText()
    {
        _canRestart=true;
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        StartCoroutine(ShowRestartText());

        while(_canRestart)
        {
            yield return new WaitForSeconds(.75f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

    IEnumerator ShowRestartText()
    {
        yield return new WaitForSeconds(3);
        _restartGameText.gameObject.SetActive(true);
    }

}
