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


    [SerializeField]
    private GameObject Explosion;


 
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
                    case -1:
                        GameManager.Instance.ActivateNegativePowerup1();
                    break;
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                         break;
                    case 2:
                        player.ActivateShields();                        
                         break;
                    case 3:
                        player.AddAmmo();
                        break;
                    case 4:
                        player.AddHealth();
                        break;
                    case 5:
                        player.ActivateMissiles();
                        break;
                    default:
                         break;
                }
            }
            Destroy(this.gameObject);
        }

        if(other.CompareTag("EnemyLaser"))
        {
            StartCoroutine(DestroyPowerup());
        }
    }

    IEnumerator DestroyPowerup()
    {
        Instantiate(Explosion,transform.position+ new Vector3(.3f,0-.3f,0),Quaternion.identity);
        yield return new WaitForSeconds(.4f);
        Destroy(this.gameObject);
    }
}
