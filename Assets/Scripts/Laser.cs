using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    private bool _isEnemyLaser = false;

    void Update()
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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        //Destroy the laser or the parent if tripleshot
        if (transform.position.y >= 8)
        {
         DestroyLasers();
        }
    }

    void DestroyLasers()
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

    void OnTriggerEnter2D(Collider2D other)
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
