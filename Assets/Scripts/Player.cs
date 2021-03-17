using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _maxHeight, _minHeight, _minWidth, _maxWidth;

    

    void Start()
    {
        //take the current position = new position(0,0,0)
        transform.position = new Vector3(0,0,0);
        _maxHeight =  0f;
        _minHeight = -3.8f;
        _maxWidth  =  10f;
        _minWidth  = -_maxWidth;
        
    }

    void Update()
    {
       CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * _speed *  Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        
        if (transform.position.y > _maxHeight)
        {
            transform.position = new Vector3(transform.position.x, _maxHeight, 0);
        }else if (transform.position.y < _minHeight){
            transform.position = new Vector3(transform.position.x, _minHeight ,0);
        }

        if (transform.position.x > _maxWidth)
        {
            transform.position = new Vector3(_maxWidth, transform.position.y,0);
        }else if(transform.position.x < _minWidth)
        {
            transform.position = new Vector3(_maxWidth, transform.position.y,0);
        }

    }
}
