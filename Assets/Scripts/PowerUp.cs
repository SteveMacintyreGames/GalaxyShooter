using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] //0 = Tripleshot, 1 = Speed, 2 = Shields.
    private int _powerUpID;

    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    private float _bottomOfScreen = -7.0f;

 
    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if(transform.position.y <= _bottomOfScreen)
        {
            Destroy(this.gameObject);
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Player player = other.transform.GetComponent<Player>();
            if(player)
            {
                if(_powerUpID == 0)
                {
                player.ActivateTripleShot();
                }
                else if (_powerUpID == 1)
                {
                    Debug.Log("Speed PowerUp Picked up!");
                }
                else if (_powerUpID == 2)
                {
                    Debug.Log("Shield Powerup Picked up!");
                }
            }
            
        }
    }
}
