using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _powerUpTimer = 5.0f;

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

    private float _speed = 3.5f;
    private float _speedBoost = 2f;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;

    public int powerUpID;

    void Start()
    {   _shield.SetActive(false);       

        if(_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL!");
        }

        transform.position = new Vector3(0,0,0);
        _maxHeight =  0f;
        _minHeight = -3.8f;
        _maxWidth  =  10f;
        _minWidth  = -_maxWidth;
        _speedBoost = 1f;
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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput,0)*_speed * _speedBoost * Time.deltaTime);
        
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
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
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
        StartCoroutine(FinishPowerUp());
    }

    public void ActivateSpeedBoost()
    {
        _speedBoost = 2f;
        StartCoroutine(FinishPowerUp());
    }

    public void ActivateShields()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    IEnumerator FinishPowerUp()
    {
        yield return new WaitForSeconds(_powerUpTimer);
        Debug.Log(powerUpID);
        switch(powerUpID)
        {
        case 0:
            _isTripleShotActive = false;
            Debug.Log("TripleShot OVER");
            break;
            
        case 1:
            _speedBoost = 1f;
            Debug.Log("SPEED OVER");
            break;  
        }
    }
}


 