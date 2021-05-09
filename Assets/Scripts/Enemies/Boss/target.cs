using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    
    [SerializeField] private int _hp = 2;

    void OnTriggerEnter2D(Collider2D other)
    {     

             if(other.CompareTag("Laser"))
            {
                _hp--;

              
                if (_hp <=0)
                {
                    Destroy(other.gameObject);
                    Destroy(this.gameObject);
                }

            }

    }
}
