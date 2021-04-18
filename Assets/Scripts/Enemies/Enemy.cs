using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _enemySpeed;
    [SerializeField]
    private int _enemyID;
    [SerializeField]

    protected float _fireRate = 3.0f;
    protected float _canFire = -1;
    protected bool _isExploding = false;

    protected Vector3 _currentPos; 


    [SerializeField]
    protected int _shotsToKill = 1;

    protected float _x,_y,_z,  _amp, _freq;
    protected Animator _anim;
    protected AudioSource _audioSource;

    [SerializeField]
    protected GameObject _enemyLaser;

    [SerializeField]
    protected AudioClip _enemyLaser_Clip;

    Vector3 _originalPosition;

    Vector3 _direction = new Vector3(0,-1,0);

    public bool isMovingRight;

    void Start()
    {
        _enemySpeed = Random.Range(1f,6f);        

        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource)
        {
            Debug.LogError("Audio source is null");
        }

        _anim = this.GetComponent<Animator>();
        if(!_anim)
        {
            Debug.LogError("The Animator inside Enemy is Null");
        }

       
    switch (_enemyID)
        {
            case 2:
            //Enemy 2 initialize side by side movement
            _amp = Random.Range(.1f,2.5f);
            _freq = Random.Range(.5f,4.5f);
            break;

            case 3:
            _amp = Random.Range (.2f, .4f);
            _freq = Random.Range(8f,10f); 
            
            var boolNum = Random.Range(0,2);
            if (boolNum>0)
            {isMovingRight = true;
            }
            else
            {
                {isMovingRight = false;}
            }

            if (isMovingRight)
            {
                transform.position = new Vector2(-10,Random.Range(5,0));
            }else
            {
                transform.position = new Vector2(10, Random.Range(5,0));
            }    
            break;
            default:
            break;
        }
   


    }

    protected virtual void Update()
    {
        CalculateMovementByID();
        FireLasers();
    }

    void CalculateMovementByID()
    {
        switch(_enemyID)
        {
            case 0:
                Enemy0Movement();
               
            break;
            case 1:
                Enemy1Movement();
            break;
            case 2:
                Enemy2Movement();
            break;

            case 3:
                Enemy3Movement();
            break;

            case 4:
            //big round laser enemy

            break;

            default:
                Enemy0Movement();
            break;
        }
        

    }

    void Move(Vector2 Direction, float speed)
    {
        _direction = Direction;
        _enemySpeed = speed;
        transform.Translate(_direction * _enemySpeed * Time.deltaTime);
    }

    void FireLasers()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f,7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            _audioSource.clip = _enemyLaser_Clip;
            _audioSource.Play();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        _currentPos = transform.position;
       
        if(other.CompareTag("Player"))
        {        
            Player.Instance.Damage();
            DestroyEnemyShip();
            
        }

        if(other.CompareTag("Laser"))
        {            
            Destroy(other.gameObject);
            _shotsToKill--;
            if(_shotsToKill <1)
            {
            Destroy(gameObject.GetComponent<Collider2D>());            
            Player.Instance.AddScore(Random.Range (7,11));
            DestroyEnemyShip();
            }

        }
    }

    public virtual void DestroyEnemyShip()
    {     
        _enemySpeed = 0;
        _canFire = 999999999;//REALLY make sure it doesn't fire again.
        _isExploding = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        _anim.SetTrigger("onEnemyDeath");
        _audioSource.Play();
        
    }

    public void RemoveGameObjectFromScene()
    {
        //public method called from within the enemy explosion animation.
        Destroy(this.gameObject);
    }

    void Enemy0Movement()
    {
        Move(new Vector2(0,-1),_enemySpeed);
        CheckBottom();
    }

    void CheckBottom()
    {
         if (transform.position.y < -6f)
        {
            PickNewTopPosition();
        }
    }

    void Enemy1Movement()
    {    
        Move(new Vector2(0,-1),_enemySpeed); 
        //if enemy reaches halfway down the screen
        if(transform.position.y < 1.3)
        {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(180,0,0), 400*Time.deltaTime);
            //_direction = new Vector3(0,1,0);
        }
        //turn around and move up.
        //if ship reaches top border, pick a new position and move down.
        if (transform.position.y > 9)
        {
            
            PickNewTopPosition();
            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

            //_direction = new Vector3(0,-1,0);
        }
            
    }

    void PickNewTopPosition()
    {
        transform.position = new Vector3 (Random.Range(-6.5f,6.5f),Random.Range(8f,8.9f),0);
    }

    void Enemy2Movement()
    {

         
         var lastX = _x;

            if(_isExploding)
            {
                _x=lastX;
            }
            else
            {
            _y = transform.position.y;
            _x = Mathf.Cos(Time.time * _freq)*_amp;
            _z = transform.position.z; 
            transform.position = new Vector3 (_x,_y,_z);
            Move(new Vector2(0,-1),_enemySpeed);
            CheckBottom();
            }        
    }

    void Enemy3Movement()
    {

         _y = Mathf.Sin(Time.time * _freq)*_amp;
         _x = Mathf.Cos(Time.time * _freq)*_amp;
         _z = transform.position.z;


            if(_isExploding)
            {
                _x=0;
                _y=0;
            }

            transform.position += new Vector3 (_x,_y,_z);
            
            if (isMovingRight)
            {
                _direction = new Vector2(1,0);
            }else
            {
                _direction = new Vector2(-1,0);
            }
            Move(_direction, _enemySpeed);
        Enemy3CheckBorders();
    
    }

    void Enemy3CheckBorders()
    {
        if(isMovingRight && transform.position.x >10)
        {
            isMovingRight = !isMovingRight;
        }else if (!isMovingRight && transform.position.x <-10)
        {
            isMovingRight = !isMovingRight;
        }

    }



}
