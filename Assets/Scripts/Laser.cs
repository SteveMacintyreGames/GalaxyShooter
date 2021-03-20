using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        //Destroy the laser or the parent if tripleshot
        if (transform.position.y >= 8)
        {
            if(transform.parent == null)
            {
                Destroy(this.gameObject);
            }else{
                Destroy(transform.parent.gameObject);
            }            
        }
    }
}
