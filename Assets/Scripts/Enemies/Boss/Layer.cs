using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : BossSection
{

    void Start()
    {
        
    }

    void Update()
    {
        if(transform.childCount == 0)
        {   

            base._currentLayer --;
            Destroy(this.gameObject);
            
        }
    }
}
