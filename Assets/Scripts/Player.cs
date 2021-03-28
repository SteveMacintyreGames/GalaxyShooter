using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _powerUpTimer = 5.0f;

    //Borders of screen. Player cannot cross them.
    private float _maxHeight, _minHeight, _minWidth, _maxWidth;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    
    public int ammoCount = 15;
    private Sprite _laserHealthBar;

    [SerializeField]
    private AudioClip _ammoBuzzer;

    private GameObject _thruster;
    //[HideInInspector]
    public bool _isMoving;
    [SerializeField]
    private AudioSource _thrusterSound;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private AudioClip _laser_Clip;
    [SerializeField]
    private AudioClip _thrust_Clip;
    [SerializeField]
    private AudioClip _explosion_Clip;
    [SerializeField]
    private GameObject _explosion_anim;

    private GameObject _rightThruster;
    private GameObject _leftThruster;

    
    public int _playerLives = 3;

    private SpawnManager _spawnManager;   

    //Powerups
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private bool _isTripleShotActive;



    [SerializeField]
    private float _speed = 5f;
    private float _speedBoost = 2f;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;
    private int _shieldPower;

    [HideInInspector]
    public int powerUpID;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;
    private GameManager _gameManager;

    AudioSource _audioSource;

    //Challenge 2: Shield Strength - Visualize the strength of the shield
    // This can be done through UI or color changing of the shield
    // allow for 3 hits on the shield to accomodate visualization

    

    void Start()
    {   
        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource){
            Debug.LogError("AudioSource in the Player is null");
        }
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(!_spawnManager)
        {
            Debug.LogError("Spawn Manager in the Player is null");
        }
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (!_gameManager){
            Debug.LogError("Game Manager in the Player is null");
        }

        //Thruster sprites and turning them off.
        _rightThruster = GameObject.Find("Right_Thruster");
        _leftThruster = GameObject.Find("Left_Thruster");
        _thruster = GameObject.Find("Thruster");

        _thruster.gameObject.SetActive(false);
        _rightThruster.gameObject.SetActive(false);
        _leftThruster.gameObject.SetActive(false);
        
        //Initiate Shields
        _shield.SetActive(false);
        _shieldPower = 3;       

        if(_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL!");
        }

        //Initializing the players position
        transform.position = new Vector3(0,0,0);
        _maxHeight =  0f;
        _minHeight = -3.8f;
        _maxWidth  =  9f;
        _minWidth  = -_maxWidth;
        _speedBoost = 1f;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }

        
    }

    void Update()
    {
       CalculateMovement();
       CheckBorders();
       CheckFireButton();
       TurnThrustersOn();
       CheckBooster();
       Shields();
    }

    void CheckFireButton()
    {
       if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
       {
           FireLaser();
       }
    }


    void TurnThrustersOn()
    {
        
        if(Input.anyKey)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                return;
            }
            _isMoving = true;
            _thruster.gameObject.SetActive(true);
            _thrusterSound.Play();

        }else
        {
            _isMoving = false;
            _thruster.gameObject.SetActive(false);
            _thrusterSound.Stop();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput,0)*_speed * _speedBoost * Time.deltaTime);
    }

    void CheckBooster()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            _speedBoost = 3f;//setting speedboost to 3 so it's noticable. Normal would be 1.3 or 1.5
        }
        else
        {
            _speedBoost = 1f;
        }
    }

    void CheckBorders()
    {
        if (transform.position.y > _maxHeight)
        {
            transform.position = new Vector3(transform.position.x, _maxHeight, 0);
        }else if (transform.position.y < _minHeight){
            transform.position = new Vector3(transform.position.x, _minHeight ,0);
        }

        if (transform.position.x > _maxWidth)
        {
            transform.position = new Vector3(_maxWidth, transform.position.y,0);
        }else if(transform.position.x < _minWidth)
        {
            transform.position = new Vector3(_minWidth, transform.position.y,0);
        }
    }

    void FireLaser()
    {
        if(ammoCount > 0)
        {
            ammoCount --;
            _uiManager.UpdateAmmoCount();
            _canFire = Time.time + _fireRate;
            if(_isTripleShotActive)
            {
                Instantiate(_tripleShot, transform.position, Quaternion.identity);

            }else
            {
                Vector3 offset = new Vector3(0f,.8f,0f);
                Instantiate(_laserPrefab, transform.position+offset, Quaternion.identity);
            }
            _audioSource.clip = _laser_Clip;
            _audioSource.Play();
        }else{
            _audioSource.clip = _ammoBuzzer;
            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _shieldPower --;
            return;
        }

            
        _playerLives --;
        
        _uiManager.UpdateLives(_playerLives);

    DamageShip();

        if(_playerLives < 1)
        {
            Instantiate(_explosion_anim, transform.position, Quaternion.identity);
            _gameManager.GameOver();
            _uiManager.GameOverText();
            _spawnManager.OnPlayerDeath();
            PlayExplosionSound();
            gameObject.SetActive(false);
            Destroy(this.gameObject,1);
            
        }
    }

    private void DamageShip()
    {
            switch(_playerLives)
        {
            case 2:
            _leftThruster.gameObject.SetActive(true);
            break;
            case 1:
            _rightThruster.gameObject.SetActive(true);
            break;
            default:
            break;
        }
    }

    private void Shields()
    {
        if (_shieldPower <=0)
        {
            _shieldPower = 0;
        }
        Color tmp = _shield.GetComponent<SpriteRenderer>().color;
            switch(_shieldPower)
            {
                //_shield is shield GameObject 
                //               
                case 3:
                //strong
                //normal sprite
                tmp.a = 1f;
                _shield.GetComponent<SpriteRenderer>().color = tmp;
                _shield.GetComponent<SpriteRenderer>().color = Color.white;
                _isShieldActive = true;
                break;
                case 2:
                //getting weak
                //half the sprites alpha
                //change color to purple
                tmp.a = 0.5f;
                _shield.GetComponent<SpriteRenderer>().color = tmp;
                _shield.GetComponent<SpriteRenderer>().color = Color.magenta;
                _isShieldActive = true;
                break;
                case 1:
                //about to die
                //quarter the alpha
                //change the color to red
                tmp.a = .1f;
                _shield.GetComponent<SpriteRenderer>().color = tmp;
                _shield.GetComponent<SpriteRenderer>().color = Color.red;
                _isShieldActive = true;
                break;
                case 0:
                _isShieldActive = false;
                _shield.SetActive(false);
                break;

            }
            //_isShieldActive = false;
            //_shield.SetActive(false);
            return;
 
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(FinishPowerUp());
    }

    public void ActivateSpeedBoost()
    {
        _speedBoost = 2f;
        StartCoroutine(FinishPowerUp());
    }

    public void ActivateShields()
    {
        _shieldPower = 3;
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    IEnumerator FinishPowerUp()
    {
        yield return new WaitForSeconds(_powerUpTimer);
        switch(powerUpID)
        {
        case 0:
            _isTripleShotActive = false;
            break;
            
        case 1:
            _speedBoost = 1f;
            break;  
        }
    }

    public void AddScore(int points)
    {
        _score+=points; 
        _uiManager.UpdateScore(_score);       
    }

    public void PlayExplosionSound()
    {
        _audioSource.clip = _explosion_Clip;
        _audioSource.Play();
    }

    public void AddAmmo()
    {
        ammoCount += 15;
        _uiManager.UpdateAmmoCount();
    }
}


 