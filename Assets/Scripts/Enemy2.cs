using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    float _minX, _maxX, _minY,_maxY,_pauseTimer, _pauseTime, _shootingLaserPauseTime;
    bool _isAlive = true;

    [SerializeField]
    private int _shotsToKill = 1;
    [SerializeField]
    private float _enemySpeed;
    
    Vector2 _nextWayPoint;
    [SerializeField]
    int _fireCount;

    [SerializeField]
    GameObject _enemy2laser;
    private Animator _laserAnim;

    [SerializeField]
    string _anim2Play;

    private Vector3 _currentPos;


    //behaviour:
    //pick a random point
    //move to it
    //stay for a few seconds
    //shoot some lasers
    //pick a new random point
    //do everything again until player kills you.


    void Start()
    {
        _laserAnim = _enemy2laser.GetComponent<Animator>();
        

        //Initialize borders of screen
        _minX = -7f; _maxX = 7f; _minY = 0; _maxY = 4.65f;
        _pauseTime = 1f;
        _pauseTimer = _pauseTime;
        _shootingLaserPauseTime = 4f;
        _fireCount = 0;

        PickARandomPoint();
        StartCoroutine(MoveAround()); 
        

    }

    void PickARandomPoint()
    {
        var x = Random.Range(_minX,_maxX);
        //only choose spots halfway down screen.
        var y = Random.Range(_minY,_maxY);
        _nextWayPoint = new Vector3(x,y, transform.position.z);
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
                if(_fireCount < 3)
                {
                    _fireCount ++;
                    yield return new WaitForSeconds(_pauseTimer);
                    PickARandomPoint();                    
                }
                
                if (_fireCount == 3)
                {
                    _pauseTimer = _shootingLaserPauseTime;
                    StartCoroutine(FireMahLazer());
                    
                }
                
                _pauseTimer = _pauseTime;
            }         

            
        }
    }

    IEnumerator FireMahLazer()
    {
        _fireCount =0;
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
     
        yield return new WaitForSeconds(_shootingLaserPauseTime);
        PickARandomPoint();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("EnemyLaser"))
        {
            return;
        }
        else if(other.CompareTag("Player"))
        {        
            Player.Instance.Damage();
            //I wanted to subtract -5 every time the player
            //hit the ship, as the ship will have 10-20 hp
            //but the child laser seems to be attached to
            //the ship and if the laser is hitting the player
            //the enemy ship is taking -5 shots per frame.
            //I've tried many different things to try to
            //resolve this from changing tags, putting checks
            //for enemy lasers, tags, putting things on
            //various layers to no avail.
            //I have an inkling as to why it's happening
            //but I still don't know for sure how to fix it.
            //If you put this current code in, the enemy ship will self destruct
       
        } 

        else if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
  
            _shotsToKill --;

            if(_shotsToKill <1)
            {
                DestroyEnemyShip();
            }
        }
       
        
    }

    public void DestroyEnemyShip()
    {
        _enemySpeed = 0;
        Player.Instance.AddScore(Random.Range (50,101));
        Destroy(gameObject);
    }
}
