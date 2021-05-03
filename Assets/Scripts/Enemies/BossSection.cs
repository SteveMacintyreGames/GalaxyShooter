using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSection : MonoBehaviour
{

    public delegate void DeleteSection(int sectionID);
    public static event DeleteSection deleteSection;

    [SerializeField] int _sectionID;
   

    void Update()
    {
        if(transform.childCount == 0)
        {
            if(deleteSection != null)
            {
                Debug.Log("All children destroyed in section " + _sectionID);
                deleteSection(_sectionID);
                
            }
        }
    }

}
