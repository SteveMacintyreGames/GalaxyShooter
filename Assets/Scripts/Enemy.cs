using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed;
    private float _bottomScreen = -6f;
    private float _topOfScreen = 8f;
    private float _leftBorder = -9;
    private float _rightBorder = 9;
    private bool _isExploding;
    private float _timeToShoot;
    private int _playerLives;


    Player _player;
    Animator _anim;
    AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaser;

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

        StartCoroutine("PewPew");
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y < _bottomScreen)
        {
            if(!_isExploding)
            {
                float randomX = Random.Range(_leftBorder,_rightBorder);
                transform.position = new Vector3(randomX, _topOfScreen, transform.position.z);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StopCoroutine("PewPew");
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
        _isExploding=true;
        _anim.SetTrigger("onEnemyDeath");
        _audioSource.Play();
        
    }

    public void RemoveGameObjectFromScene()
    {
        Destroy(this.gameObject);
    }

    IEnumerator PewPew()
    {
        while(true)
        {
        _timeToShoot = Random.Range(1,4);
        yield return new WaitForSeconds (_timeToShoot);
        Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        }

    }
}
