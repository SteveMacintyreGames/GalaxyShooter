using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _enemy_Laser_Speed = 8.0f;

    Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

     void Update()
    {
        transform.Translate(Vector3.down * _enemy_Laser_Speed * Time.deltaTime);

        if(transform.position.y <= -7 )
        {
            if(transform.parent==null)
            {
            Destroy(this.gameObject);
            }
            else{
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage();
            Destroy(this.gameObject);
        }
    }
}
