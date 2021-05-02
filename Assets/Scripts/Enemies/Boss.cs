using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject _bossMover;
    [SerializeField] float _timeToWaitBetweenSections;
    
    [SerializeField] protected List<GameObject> _waypointsOriginal = new List<GameObject>();
    [SerializeField] protected List<GameObject> _waypoints = new List<GameObject>();
     [SerializeField] private float _speed;
     protected int _currentWaypoint;
     
    private bool _canMove = true;
    protected bool _nearWaypoint = false;

    [SerializeField] protected Text debugText;

  
    
    void Start()
    {
        _waypoints = _waypointsOriginal;
        _currentWaypoint = 0;

      for(int x = 0; x < _waypoints.Count; x++)
      {
          //debugText.text += "\n" + _waypoints[x];
      }
       // debugText.text += "\n All waypoints tallied";
        

       StartCoroutine(Move());
      // debugText.text += "\n_waypoints: " + _waypoints.Count;
       //debugText.text += "\n 0: "  +_waypoints[0];
       //debugText.text += "\n 09: " + _waypoints[9];
       //debugText.text += "\n";
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
            debugText.text = "All waypoints destroyed";
            Debug.Break();
        }
    }

    IEnumerator Move()
    {
        if(_canMove)
        {
            while(_waypoints.Count > 1)
            {
                    //debugText.text = "Moving to: " + _waypoints[_currentWaypoint];
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

    private void CheckIfNearWayPoint()
    {
        if(Vector3.Distance(transform.position,_waypoints[_currentWaypoint].transform.position ) <0.5f)
        {
            _nearWaypoint = true;
        }else{
            _nearWaypoint = false;
        }
    }

    protected void RemoveSectionByID(int id)
    {
        debugText.text = "";
        debugText.text = id.ToString();
        Debug.Log(id);
        _waypoints.RemoveAt(id);
    }

}
