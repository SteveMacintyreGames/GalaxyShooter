using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSection : MonoBehaviour
{

    public delegate void DeleteSection(int sectionID);
    public static event DeleteSection deleteSection;

    [SerializeField] int _sectionID;
    bool sentMessage = false;

   

    void Update()
    {    var itemCount = 0;
        foreach (GameObject item in this.gameObject.transform)
        {
            if (item)
            {
                itemCount++;
            }

        }
        if (itemCount ==0)
        {
             if(deleteSection != null)
            {
                
                if(!this.sentMessage)
                {
                    Debug.Log("All children destroyed in section " + _sectionID);
 
                        deleteSection(_sectionID);
                
                    
                    this.sentMessage = true;
                    //HUGE thank you to Paul Marsh for the clue this was constantly on.
                }
                
                
            }

        }
        /*
        if(transform.childCount == 0)
        {
            if(deleteSection != null)
            {
                
                if(!this.sentMessage)
                {
                    Debug.Log("All children destroyed in section " + _sectionID);
 
                        deleteSection(_sectionID);
                
                    
                    this.sentMessage = true;
                    //HUGE thank you to Paul Marsh for the clue this was constantly on.
                }
                
                
            }
        }
        */
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            
        }
    }

}
