using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 8f;

    [SerializeField]
    protected Vector3 _laserDirection = Vector3.up;

    protected virtual void Start()
    {
        Invoke("DestroyLasers",4f);
    }


    protected virtual void Update()
    {
        Move();    
    }


    protected virtual void Move()
    {
        transform.Translate(_laserDirection * _speed * Time.deltaTime);
        
    }

    protected virtual void DestroyLasers()
    {
        this.gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //is this really needed?
    }


}
