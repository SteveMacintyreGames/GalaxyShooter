using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    

    void Start()
    {
        //take the current position = new position(0,0,0)
        transform.position = new Vector3(0,0,0);
        
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * _speed *  Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //if the position of y is greater than 5.75
        //y pos will equal 5.75
        //else if y pos is less than -3.8
        //y pos is -3.8
        
        if (transform.position.y > 5.75)
        {
            transform.position = new Vector3(transform.position.x,5.75f,0);
        }else if (transform.position.y < -3.8){
            transform.position = new Vector3(transform.position.x,-3.8f,0);
        }

        //if x pos is greater than 9
        //x pos = 9
        //if x pos is less than -9
        //x pos is 9

        if (transform.position.x > 9)
        {
            transform.position = new Vector3(9f, transform.position.y,0);
        }else if(transform.position.x < -9)
        {
            transform.position = new Vector3(9f, transform.position.y,0);
        }

    }
}
