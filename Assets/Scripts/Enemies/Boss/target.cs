using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class target : MonoBehaviour
{    
    [SerializeField] private int _hp = 2;
    private int _maxHP;

    void Start()
    {
        _maxHP = _hp;        
    }



    void OnTriggerEnter2D(Collider2D other)
    {     

             if(other.CompareTag("Laser"))
            {
                _hp--;
              
                if (_hp <= _maxHP)
                {
                  Instantiate(Resources.Load("Explosion_Smaller"),transform.position, Quaternion.identity);
                }
                if (_hp <=0)
                {
                    GameObject explosion = Instantiate(Resources.Load("Explosion1"), transform.position, Quaternion.identity) as GameObject;
                    explosion.transform.localScale = transform.localScale;
                    Destroy(this.gameObject);
                }
                Destroy(other.gameObject);
            }
    }
}
