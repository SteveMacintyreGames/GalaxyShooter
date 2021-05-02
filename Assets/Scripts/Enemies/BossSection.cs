using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSection : Boss
{
    [SerializeField] int _sectionID;
   

    void Update()
    {
        if(transform.childCount == 0)
        {
            RemoveCurrentWaypoint();
        }
    }
}
