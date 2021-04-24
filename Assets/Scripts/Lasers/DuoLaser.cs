using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoLaser : Laser
{
    [SerializeField]
        public float duoLaserSpeed;

    
    protected override void Start()
    {
        base.Start();
        _speed = duoLaserSpeed;    
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
