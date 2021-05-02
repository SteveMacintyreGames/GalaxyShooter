using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform _bossMover;
    [SerializeField] float _timeToWaitBetweenSections;
    
    [SerializeField] protected List<GameObject> _waypointsOriginal;
    protected List<GameObject> _waypoints;
     [SerializeField] private float _speed;
     protected int _currentWaypoint;

    private bool _canMove = true;
    protected bool _nearWaypoint = false;
  
    // get list of waypoints
    //move to a waypoint
    //stop
    //move to the next.
    
    void Start()
    {
        _waypoints = _waypointsOriginal;
        _currentWaypoint = 0;
       StartCoroutine(Move());
    }
    void Update()
    {
        CheckNumberOfWaypoints();
        CheckIfNearWayPoint();
    }

    void CheckNumberOfWaypoints()
    {
        if (_waypoints.Count == 0)
        {
            _canMove = false;
            Debug.Log("All waypoints destroyed!");
            Debug.Break();
        }
    }

    IEnumerator Move()
    {
        if(_canMove)
        {
            while(_waypoints.Count > 1)
            {
                   transform.position = Vector3.MoveTowards(transform.position, 
                    _waypoints[_currentWaypoint].transform.position, Time.deltaTime * _speed);
                if(_nearWaypoint)
                {
                        yield return new WaitForSeconds(_timeToWaitBetweenSections);
                        _currentWaypoint ++;
                        if (_currentWaypoint > _waypoints.Count-1)
                        {
                            _currentWaypoint = 0;
                        }   
                }  
                yield return 0;     
            }        
                                

             
        }
    }   



    protected void RemoveCurrentWaypoint()
    {
        _waypoints.Remove(_waypoints[_currentWaypoint].gameObject);
        foreach (GameObject section in _waypoints)
        {
            Debug.Log(section.name);
        }
        

    }

    private void CheckIfNearWayPoint()
    {
        if(Vector3.Distance(transform.position,_waypoints[_currentWaypoint].transform.position ) <0.5f)
        {
            _nearWaypoint = true;
        }else{
            _nearWaypoint = false;
        }
    }


}
