using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private int _sdh_id;
    [SerializeField] private int childrenCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == childrenCount)
        {
            Destroy(this.gameObject);
        }

    }
}
