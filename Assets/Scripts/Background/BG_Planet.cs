using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Planet : MonoBehaviour
{
    private bool _isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        _isMoving=true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * 1* Time.deltaTime); 
        if(transform.position.y < -300)
        {
            StartCoroutine(ReachBottom());
        }
        
    }
    IEnumerator ReachBottom()
    {
            _isMoving=false;
            yield return new WaitForSeconds(Random.Range(60f,360f));
            transform.position = new Vector2(Random.Range(-220f,220f),300f);
            _isMoving=true;
    }
}
