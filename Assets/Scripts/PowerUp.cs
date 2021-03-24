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

    [SerializeField]
    private AudioClip _audioClip;


 
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
            AudioSource.PlayClipAtPoint(_audioClip,transform.position);

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
                         break;
                    case 2:
                        player.ActivateShields();                        
                         break;
                    default:
                         break;
                }
            }            
            Destroy(this.gameObject);
        }
    }
}
