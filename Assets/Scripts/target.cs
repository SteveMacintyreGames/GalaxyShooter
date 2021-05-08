using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    [SerializeField] protected int _id;
    [SerializeField] private int _hp = 2;
    [SerializeField] private int _damageShow;
    [SerializeField] private bool _isBase;

    [SerializeField] private GameObject _damage = null;
    
    [SerializeField] private int childrenCount = 0;
    [SerializeField] private GameObject _path = null;


    void Start()
    {
        _damageShow = _hp/2;
        if(_damage != null)
        {
            _damage.gameObject.active = false;
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (transform.childCount == childrenCount)
        {
             if(other.CompareTag("Laser"))
            {
                _hp--;
                if(_damage != null)
                {
                     if(_hp <= _damageShow)
                        {                                                     
                                _damage.gameObject.active = true;
                                this.GetComponent<SpriteRenderer>().enabled = false;
                        }
                }
              
                if (_hp <=0)
                {
                    if(_isBase)
                    {                        
                        return;
                    }
                    Destroy(other.gameObject);
                    Destroy(this.gameObject);
                }

            }
        } else { return; }
       
    }
}
