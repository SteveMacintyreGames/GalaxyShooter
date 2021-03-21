using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 3.5f;
    private float _maxHeight, _minHeight, _minWidth, _maxWidth;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private int _playerLives = 3;

    private SpawnManager _spawnManager;   

    //Powerups
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private bool _isTripleShotActive;
    [SerializeField]
    private float _powerUpTimer = 5.0f;
    [SerializeField]
    private bool _isSpeedBoostActive;
    [SerializeField]
    private float _speedBoost = 8.5f;
    [SerializeField]
    private float _currentSpeed;

    public int powerUpID;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL!");
        }

        transform.position = new Vector3(0,0,0);
        _maxHeight =  0f;
        _minHeight = -3.8f;
        _maxWidth  =  10f;
        _minWidth  = -_maxWidth;
        _currentSpeed = _speed;

    }

    void Update()
    {
       CalculateMovement();
       if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
       {
           FireLaser();
       }
       
    }

    void CalculateMovement()
    {
        if(_isSpeedBoostActive)
        {
            _currentSpeed = _speedBoost;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * _speed *  Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _currentSpeed * Time.deltaTime);
        
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
            transform.position = new Vector3(_maxWidth, transform.position.y,0);
        }
    }

    void FireLaser()
    {       
        _canFire = Time.time + _fireRate;
        if(_isTripleShotActive)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);

        }else
        {
            Vector3 offset = new Vector3(0f,.8f,0f);
            Instantiate(_laserPrefab, transform.position+offset, Quaternion.identity);
        }
    }

    public void Damage()
    {
        _playerLives --;

        if(_playerLives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(FinishPowerup());
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(FinishPowerup());
    }

    IEnumerator FinishPowerup()
    {
        yield return new WaitForSeconds(_powerUpTimer);
        
        switch(powerUpID)
        {
        case 0:
            _isTripleShotActive = false;
            Debug.Log("TripleShot OVER");
            break;
            
        case 1:
            _isSpeedBoostActive = false;
            _currentSpeed = _speed;
            Debug.Log("SPEED OVER");
            break;
          
        case 2:
            //_isShieldActive = false;
            break;
          
        default:
            break;
           
        }

    }
}


