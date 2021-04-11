using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;
    [SerializeField]
    private int _enemyID;
    [SerializeField]

    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private bool _isExploding = false;

    //Screen Borders
    private float _bottomScreen = -6f;
    private float _topOfScreen = 8f;
    private float _leftBorder = -9;
    private float _rightBorder = 9;

    
    private int _playerLives;

    float _x,_y,_z;
    [SerializeField]
    float _amp;
    [SerializeField]
    float _freq;


    Player _player;
    Animator _anim;
    AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private AudioClip _enemyLaser_Clip;
    private AudioClip _explosion_Clip;

    private  bool _halfReached = false;
    bool _topPos = false;

    Vector3 _originalPosition;

    Vector3 _direction = new Vector3(0,-1,0);

    public bool isMovingRight;

    void Start()
    {
        _enemySpeed = Random.Range(1f,6f);        

        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(!_player)
        {
            Debug.LogError("The Player inside Enemy is Null");
        }

        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource)
        {
            Debug.LogError("Audio source is null");
        }

        _playerLives = _player._playerLives;

        _anim = this.GetComponent<Animator>();
        if(!_anim)
        {
            Debug.LogError("The Animator inside Enemy is Null");
        }

        _explosion_Clip = _audioSource.clip;
    switch (_enemyID)
        {
            case 2:
            //Enemy 2 initialize side by side movement
            _amp = Random.Range(.1f,2.5f);
            Debug.Log("Enemy2 amp");
            _freq = Random.Range(.5f,4.5f);
            Debug.Log("Enemy2 freq");
            break;

            case 3:
            _amp = Random.Range (.2f, .4f);
            Debug.Log("Enemy 3 Amp "+_amp);
            _freq = Random.Range(8f,10f);
            Debug.Log("Enemy3 Freq "+_freq);
            
            
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

    void Update()
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
            for (int i=0; i<lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            _audioSource.clip = _enemyLaser_Clip;
            _audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 _currentPos = transform.position;
       
        if(other.CompareTag("Player"))
        {            
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            DestroyEnemyShip();
            
        }

        if(other.CompareTag("Laser"))
        {            
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());            
            _player.AddScore(Random.Range (5,11));
            DestroyEnemyShip();
        }
    }

    public void DestroyEnemyShip()
    {        
        _enemySpeed = 0;
        _canFire = 999999999;//REALLY make sure it doesn't fire again.
        _isExploding = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        _anim.SetTrigger("onEnemyDeath");
        _audioSource.clip = _explosion_Clip;
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

         _y = transform.position.y;
         _x = Mathf.Cos(Time.time * _freq)*_amp;
         _z = transform.position.z;

            if(_isExploding)
            {
                _x=0;
            }
            transform.position = new Vector3 (_x,_y,_z);
            Move(new Vector2(0,-1),_enemySpeed);
            CheckBottom();        
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
