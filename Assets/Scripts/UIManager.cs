using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    //Ammo count information
    private Text _ammoCountText;
    private int _ammoCount;
    private int _maxAmmo;
    [SerializeField]
    private Image _ammoHealthBar;

    private bool _canRestart;

    Player player;


    void Awake()
    {   
        player = GameObject.Find("Player").GetComponent<Player>();
        _maxAmmo = _ammoCount = player.ammoCount;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _ammoCountText = GameObject.Find("AmmoText").GetComponent<Text>();
    }
    void Start()
    {
        UpdateAmmoCount();
        UpdateScore(0);
    }


    public void UpdateAmmoCount()
    {
        _ammoCount = player.ammoCount;
        _ammoCountText.text = "Ammo: "+_ammoCount.ToString();

        _ammoHealthBar.fillAmount =  (float)_ammoCount/(float)_maxAmmo;
        Debug.Log("Ammo : "+_ammoCount + " Max Ammo: "+_maxAmmo);
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
