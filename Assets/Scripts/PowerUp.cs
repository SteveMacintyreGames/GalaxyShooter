using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    private float _bottomOfScreen = -7.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down speed of 3. adjustable in inspector
        //when leaving screen, destroy this object
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);

        if(transform.position.y <= _bottomOfScreen)
        {
            Destroy(this.gameObject);
        }

    }

    //ontrigger collision
    //only be collectable by player (hint use tags)
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //Activate TripleShot in player
            Player player = other.transform.GetComponent<Player>();
            if(player)
            {
                player.ActivateTripleShot();
            }
            
        }
    }
}
