using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Enemy : Laser
{

    private GameObject _target;
    private Vector3 _laserHomingDirection;
    public Vector3 whatDirection = Vector3.down;

   
    protected override void Start()
    {
        Invoke("DestroyLasers",4f);
        

         if(GameManager.Instance._negativePowerupActivated)
        {
        _target = GameObject.FindWithTag("Player");
        _laserHomingDirection = (_target.transform.position - transform.position).normalized;
        _laserDirection =  new Vector3(_laserHomingDirection.x,-1,0) * _speed * Time.deltaTime;
        }else
        {
            Debug.Log(whatDirection);
            _laserDirection = whatDirection * _speed * Time.deltaTime;
        }
    }
    

    protected override void DestroyLasers()
    {
        if(transform.parent.gameObject.name == "Enemy_DuoLaser(Clone)")
        {
            //Debug.Log("EnemyLaser parent destroyed");
            transform.parent.gameObject.SetActive(false);
            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            Player.Instance.Damage();
        }
    }

}
