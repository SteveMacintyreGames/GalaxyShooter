using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
    private float _bottomScreen = -6f;
    private float _topOfScreen = 8f;
    private float _leftBorder = -9;
    private float _rightBorder = 9;
    private bool _isExploding;

    Player _player;
    Animator anim;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(!_player)
        {
            Debug.LogError("The Player is Null");
        }
        anim = this.GetComponent<Animator>();
        if(!anim)
        {
            Debug.LogError("The Animator is Null");
        }
    }

    void Update()
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
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            EnemyGoBoom();
            
        }

        if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            _player.AddScore(10);
            EnemyGoBoom();
        }
    }

    private void EnemyGoBoom()
    {
        _enemySpeed = 0;
        _isExploding=true;
        anim.SetTrigger("onEnemyDeath");
    }

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
