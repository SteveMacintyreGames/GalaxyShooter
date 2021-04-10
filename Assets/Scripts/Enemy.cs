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
    float _amp;
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

    Vector3 _direction = new Vector3(0,-1,0);

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

        //Enemy 2 initialize side by side movement
        _amp = Random.Range(.1f,2.5f);
        _freq = Random.Range(.5f,4.5f);
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

            default:
                Enemy0Movement();
            break;
        }
        

    }

    void Move()
    {

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
        Move();
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
        Move(); 
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
            Move();
            CheckBottom();
            


    }




}
