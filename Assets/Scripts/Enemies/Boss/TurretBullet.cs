using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private float _bulletSpeed = -6f;

    public float BulletSpeed
    {
        get{
            return _bulletSpeed;
        }
        set{
            _bulletSpeed = value;
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        Invoke ("DestroyBullet",5f);
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(Vector3.up * _bulletSpeed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
