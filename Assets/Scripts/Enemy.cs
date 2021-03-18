using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _enemySpeed = 4f;
    private float _bottomScreen = -6f;
    private float _topOfScreen = 8f;
    private float _leftBorder = -9;
    private float _rightBorder = 9;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        //move down 4 meters per second
        //when off the screen, respawn at top.

        if(transform.position.y < _bottomScreen)
        {
            //respawn at the top
            //transform.position = new Vector3(transform.position.x,_topOfScreen,transform.position.z);
        
            //Bonus challenge
            //respawn at top at a random x position
            float randomX = Random.Range(_leftBorder,_rightBorder);
            transform.position = new Vector3(randomX, _topOfScreen, transform.position.z);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit "+other.transform.name);
        //Hit Player

        //if other is Player
        //damage player
        //destroy us
        if(other.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");
            //Damage Player
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            
            Destroy(this.gameObject);
        }

        //if other is laser
        //destroy laser
        //destroy us

        if(other.CompareTag("Laser"))
        {
            //Debug.Log("Laser Hit");
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }


    }
}
