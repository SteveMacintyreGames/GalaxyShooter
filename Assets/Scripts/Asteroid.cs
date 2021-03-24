using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    float _speed = -5f;
    [SerializeField]
    private GameObject _explosion;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateAsteroid();
    }

    void RotateAsteroid()
    {
        transform.Rotate(0,0,_speed*Time.deltaTime,Space.Self);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            GameObject kaboom = Instantiate(_explosion,transform.position,Quaternion.identity);
            Destroy(this.gameObject,.40f);
            Destroy(other.gameObject);
        }
    }
}
