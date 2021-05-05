using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
  
    [SerializeField] private Text _debugText;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _timeToWaitBetweenSections = 1f;

    [SerializeField] protected List<GameObject> _waypoints = new List<GameObject>();


    protected int _currentWaypoint;

    bool _canMove = true;

    void OnEnable()
    {
        BossSection.deleteSection += IgnoreSectionNow;
    }

    void OnDisable()
    {
        BossSection.deleteSection -= IgnoreSectionNow;
    }

    //cycle through all 10 sections
    //if a section is destroyed
    //skip over to the next section
    //continue until you find a live section
    //move there and continue cycling.

    //if a section is destroyed while your in it,
    //move on to the next section.

    void Start()
    {
        _currentWaypoint = 0;
             
        StartCoroutine(MoveThroughWaypoints());
        
    }
  
    IEnumerator MoveThroughWaypoints()
    {
        while(_canMove)
        {
            MoveTowardsWaypoint();
        
            if(Vector3.Distance(transform.position,_waypoints[_currentWaypoint].transform.position ) <0.5f)
            {
                if(_waypoints.Count <=1)
                {
                    _canMove = false;
                }

               
                yield return new WaitForSeconds(_timeToWaitBetweenSections);
                ChooseNextWaypoint();
            }
            yield return 0;
        }
        yield return null;       

    }

    void MoveTowardsWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            _waypoints[_currentWaypoint].transform.position, Time.deltaTime * _speed);
        
        
    }
    void ChooseNextWaypoint()
    {
        _currentWaypoint ++;  
            
        if(_currentWaypoint >= _waypoints.Count)
        {
            _currentWaypoint = 0;
        }
    }

    void IgnoreSectionNow(int id)
    {   
        string x = "Path ("+id+")";
        for(int i = _waypoints.Count-1; i>=0 ; i--)
        {
            if(x == _waypoints[i].transform.name)
            {
                _waypoints.RemoveAt(i);
                ChooseNextWaypoint();
                //_debugText.text += _waypoints.Count.ToString();
                break;

            }            
        } 
    }

}
