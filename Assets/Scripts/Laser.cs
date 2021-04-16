using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    protected bool _isEnemyLaser = false;

    private GameObject _target;
    private Vector3 _laserHomingDirection;
    private Vector3 _laserDirection;

    protected virtual void Start()
    {
        Invoke("DestroyLasers",5f);
        
        if(GameManager.Instance._negativePowerupActivated)
        {
        _target = GameObject.FindWithTag("Player");
        _laserHomingDirection = (_target.transform.position - transform.position).normalized;
        _laserDirection =  new Vector3(_laserHomingDirection.x,-1,0) * _speed * Time.deltaTime;
        }else
        {
            _laserDirection = Vector3.down * _speed * Time.deltaTime;
        }
    }


    protected virtual void Update()
    {
        if(!_isEnemyLaser)
        {
            MoveUp();
        }else
        {
            MoveDown();
        }
        
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        //Destroy the laser or the parent if tripleshot
        if (transform.position.y >= 8)
        {
            DestroyLasers();                    
        }
    }

        void MoveDown()
    {

        transform.Translate(_laserDirection);
        
        //Destroy the laser or the parent if tripleshot
        if (transform.position.y >= 8)
        {
         DestroyLasers();
        }
    }

    protected void DestroyLasers()
    {
            if(transform.parent == null)
            {
                Destroy(this.gameObject);
            }else{
                Destroy(transform.parent.gameObject);
            }   

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.Damage();
            }
        }
    }


}
