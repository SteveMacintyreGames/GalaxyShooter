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
            player.powerUpID = _powerUpID;
            if(player)
            {
               switch(_powerUpID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        Debug.Log("SPEED Powerup");
                         break;
                    case 2:
                        player.ActivateShields();
                        Debug.Log("SHIELD Powerup");
                         break;
                    default:
                         break;
                }
            }
            
        }
    }
}
