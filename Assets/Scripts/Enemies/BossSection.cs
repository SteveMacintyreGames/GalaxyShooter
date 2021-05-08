using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSection : MonoBehaviour
{

    public delegate void DeleteSection(int sectionID);
    public static event DeleteSection deleteSection;

    [SerializeField] int _sectionID;
    [SerializeField] int _childrenCount = 1;
    bool sentMessage = false;

   

    void Update()
    {   
        
        if(transform.childCount == _childrenCount)
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

}
