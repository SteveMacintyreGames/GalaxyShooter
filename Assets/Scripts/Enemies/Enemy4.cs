using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    float _minX, _maxX, _minY,_maxY;
    [SerializeField]
    float _pauseTimer, _pauseTime = 1f;
    float _shootingLaserPauseTime = 3f;
    bool _isAlive;

    [SerializeField]
    private int _shotsToKill = 1;
    [SerializeField]
    private float _enemySpeed;
    
    Vector2 _nextWayPoint;
    [SerializeField]
    int _fireCountDown;

    [SerializeField]
     int _fireCount = 3;

    [SerializeField]
    GameObject _enemy2laser;

    [SerializeField]
    GameObject _explosionHolder;

    Animator _anim;
    private Animator _laserAnim;
    string _anim2Play;

    private Vector3 _currentPos;

    void Awake()
    {
        _explosionHolder.gameObject.SetActive(false);
        _anim = GetComponent<Animator>();
        _laserAnim = _enemy2laser.GetComponent<Animator>();
    }
    void Start()
    {    
        
        //Initialize borders of screen
        _minX = -7f; _maxX = 7f; _minY = 0; _maxY = 4.65f;

        //_pauseTime - time inbetween travel points
        //_pauseTimer - parameter buffer.
        //_ShootingLaserPauseTime - laser needed a little more time so it gets its own variable.
        //firecount - how many moves before firing
        //fireCountDown - internal variable to countdown to _firecount.
        _pauseTimer = _pauseTime;
        _fireCountDown = 0;
        _isAlive=true;

        PickARandomPoint();
        StartCoroutine(MoveAround()); 
        

    }

    void PickARandomPoint()
    {
        var x = Random.Range(_minX,_maxX);
        var y = Random.Range(_minY,_maxY);
        _nextWayPoint = new Vector2(x,y);
    }

    IEnumerator MoveAround()
    {
        
        while (_isAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, _nextWayPoint, _enemySpeed * Time.deltaTime);


            if ((Vector2)transform.position != (Vector2)_nextWayPoint)
            {       
                yield return null;
            }
            else
            {
                if(_fireCountDown < _fireCount)
                {
                    _fireCountDown ++;
                    yield return new WaitForSeconds(_pauseTimer);
                    PickARandomPoint();                    
                }
                
                if (_fireCountDown == _fireCount)
                {
                    FireLaser();                       
                    _pauseTimer = _shootingLaserPauseTime;
                    yield return new WaitForSeconds(_pauseTimer);
                     _pauseTimer = _pauseTime;
                    _fireCountDown = 0;
                    PickARandomPoint();
                   
                                       
                }               
                
            }  
            yield return null;
          
        }
        
    }
    private void FireLaser()
    {
        var randomAnim = Random.Range(0,2);
        if(randomAnim == 0)
            {
                _anim2Play = "left_anm";
            }
        else
            {
                _anim2Play = "right_anm";
            }

        _laserAnim.Play(_anim2Play);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("EnemyLaser"))
        {
            return;
        }
        else if(other.CompareTag("Player"))
        {   
            Debug.Log("PLAYER HIT");
            Player.Instance.Damage();
            _shotsToKill -= 3;
            DeathCheck();
  
       
        } 

        else if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
  
            _shotsToKill --;

            DeathCheck();
        }
       
        
    }

    private void DeathCheck()
    {
        if(_shotsToKill <1)
        {
            _explosionHolder.gameObject.SetActive(true);
             _enemySpeed = 0;
            _isAlive = false;
            _anim.SetTrigger("isDead");
        }    
    }
    public void DestroyEnemyShip()
    {       
        Player.Instance.AddScore(Random.Range (50,101));
        Destroy(gameObject);
    }
}

