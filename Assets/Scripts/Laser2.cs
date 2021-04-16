using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser2 : MonoBehaviour
{
    protected  void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Hit ENEMY");
            return;
        }
        if(other.CompareTag("Player"))
        {
            Debug.Log("Hit PLAYER");
            Player.Instance.Damage();
        }
        
    }

}
