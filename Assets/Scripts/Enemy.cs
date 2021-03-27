using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private bool _isExploding;

    //Screen Borders
    private float _bottomScreen = -6f;
    private float _topOfScreen = 8f;
    private float _leftBorder = -9;
    private float _rightBorder = 9;

    
    private int _playerLives;


    Player _player;
    Animator _anim;
    AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private AudioClip _enemyLaser_Clip;
    private AudioClip _explosion_Clip;

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
    }

    void Update()
    {
        CalculateMovement();
        FireLasers();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y < _bottomScreen)
        {
            //If the enemy isn't exploding, then it can respawn at the top of the screen.
            //I know it isn't useful after Jonathans solution but leaving it in just
            //in case of a fluke.

            if(!_isExploding)
            {
                float randomX = Random.Range(_leftBorder,_rightBorder);
                transform.position = new Vector3(randomX, _topOfScreen, transform.position.z);
            }

        }
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

    private void DestroyEnemyShip()
    {        
        _enemySpeed = 0;
        _canFire = 999999999;//REALLY make sure it doesn't fire again.
        _isExploding=true;
        _anim.SetTrigger("onEnemyDeath");
        _audioSource.clip = _explosion_Clip;
        _audioSource.Play();
        
    }

    public void RemoveGameObjectFromScene()
    {
        //public method called from within the enemy explosion animation.
        Destroy(this.gameObject);
    }


}
