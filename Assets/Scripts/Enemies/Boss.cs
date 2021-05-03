using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject _bossMover;
    [SerializeField] private Text _debugText;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _timeToWaitBetweenSections = 1f;

    [SerializeField] protected List<GameObject> _waypoints = new List<GameObject>();
    bool[] canAttack = new bool[10];


    protected int _currentWaypoint;
    int _destroyedWaypoints = 0;
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

    void Awake()
    {
        _currentWaypoint = 0;
        _destroyedWaypoints = 0;
        for (int y =0; y < _waypoints.Count; y++)
        {
            _debugText.text += _waypoints[y] + ", ";
        }
        for (int x=0; x < canAttack.Length; x++)
        {
            canAttack[x] = true;
        }
        StartCoroutine(MoveThroughWaypoints());
    }
    void Start()
    {
       
    }
        


    void Update()
    {
        string txt = "";
     //_debugText.text = "Waypoint: "+_currentWaypoint;
             for (int y =0; y < _waypoints.Count; y++)
        {
             txt += _waypoints[y] + ", ";
        }
        _debugText.text = txt;
    }
    void IfOnLastSection()
    {
        if((Vector3.Distance(transform.position,_waypoints[_currentWaypoint].transform.position ) <0.5f) &&
        _destroyedWaypoints == 9)
        {
            _canMove = false;
        }
    }

    IEnumerator MoveThroughWaypoints()
    {
        while(_canMove)
        {
            MoveTowardsWaypoint();
        
            if(Vector3.Distance(transform.position,_waypoints[_currentWaypoint].transform.position ) <0.5f)
            {
               
                IfOnLastSection();

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
        var _id = id;
        //GameObject removeThis = _waypoints[id];
        //_waypoints.Remove(removeThis);
        if(_waypoints[_id] != null){
        _waypoints.RemoveAt(_id);
        _debugText.text = "Waypoint "+_id+" destroyed";
        }     
       
        canAttack[_id] = false;
        _destroyedWaypoints ++;
        if (_destroyedWaypoints == 10)
        {
            Debug.Log("All waypoints destroyed in IgnoreSectionNow");
        }
        //ChooseNextWaypoint();
        
    }

}
