using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Enemy : Laser
{
   
    protected override void Start()
    {
        Invoke("DestroyLasers",4f);
        _laserDirection = Vector3.down;
    }

    protected override void DestroyLasers()
    {
        Debug.Log(transform.parent.gameObject.name);
        if(transform.parent.gameObject.name == "Enemy_DuoLaser")
        {
            //Debug.Log("EnemyLaser parent destroyed");
            transform.parent.gameObject.SetActive(false);
            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.Damage();
        }
    }


}
