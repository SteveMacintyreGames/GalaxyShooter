using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] Transform _bossMover;
    
    [SerializeField] GameObject[] _waypoints;
    private int _currentWaypoint = 0;
    [SerializeField] float _speed;
    private bool _waypointChosen = false;
    void Update()
    {
         Move();       
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_currentWaypoint].transform.position, Time.deltaTime * _speed);
        
        if(transform.position == _waypoints[_currentWaypoint].transform.position)
        {
            StartCoroutine(WaitAndGoToNextWaypoint());
            
        }
    }

    void NextWayPoint()
    {
        if(_waypointChosen == false)
        {
            _currentWaypoint ++;
            if(_currentWaypoint >= _waypoints.Length)
            {
                _currentWaypoint = 0;
            }
            _waypointChosen = true;
            StartCoroutine(ClearWayPointChosen());
        }
        
    }

    IEnumerator WaitAndGoToNextWaypoint()
    {
        yield return new WaitForSeconds(3f);
        NextWayPoint();        
    }

    IEnumerator ClearWayPointChosen()
    {
        yield return new WaitForSeconds(3f);
        _waypointChosen = false;
    }
}
