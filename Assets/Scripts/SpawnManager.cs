using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get{
            if(_instance == null)
                Debug.Log("SpawnManager is NULL");
            
            return _instance;
        }
    }

    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private Text _enemyHolderHolder;

    [SerializeField]
    private GameObject[] _powerUps;
    private float _minpowerupTime, _maxpowerupTime;
    public float _powerupTime;

    [SerializeField]
    private GameObject _enemyHolder;
    private float xPos = 6.5f;
    private float Ypos = 8f;
    [SerializeField]
    private float _timeToWait = 5.0f;

    private bool _stopSpawning = false;
    private int _powerUpId;


[SerializeField]
    private Text _levelText;

[SerializeField]
    private int _level;
    private int _spawnEnemyNumber;
    private float _timeToSpawn;
    
    [SerializeField]
    private float _maxEnemies;
    
    [SerializeField]
    private int _enemiesSpawned = 0;
    public int _enemiesOnScreen = 0;
    private float _maxEnemiesOnScreen = 3f;
    private bool _canSpawn = true;

    [SerializeField]
    private int _powerupToSpawn;

    private int _weightedTotal;

    private bool _firstPowerupSpawn = true;

    void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        _level = 1;
        _timeToSpawn = 3f;
        _timeToWait = 3f;
        _maxEnemies = 5f;
        _levelText.gameObject.SetActive(false);
        _minpowerupTime = 10f;
        _maxpowerupTime = 20f;
        _powerupTime = _maxpowerupTime;
    }

    void Update()
    {
        CheckIfCanSpawn();
    }

    void CheckIfCanSpawn()
    {
        if(_enemiesOnScreen > (int)_maxEnemiesOnScreen)
        {
            _canSpawn = false;
        }else
        {
            _canSpawn = true;
        }
    }
    //Decide on a percentage of Enemies & Powerups per level
    //Increase the number of enemies and powerups every few levels up to a limit.

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(!_stopSpawning)
        {   
            StartCoroutine(ShowLevel());
            yield return new WaitForSeconds(3f);
    
            for (int i = 0; i < (int)_maxEnemies; i++)
            {
                yield return new WaitForSeconds(_timeToSpawn);         
                float randomX = Random.Range(-xPos,xPos);
                Vector3 posToSpawn = new Vector3(randomX,Ypos,0);
                if(_canSpawn)               
                {
                    ChooseEnemy();

                    GameObject newEnemy = Instantiate (_enemyPrefab[Random.Range(0,_spawnEnemyNumber)],posToSpawn, Quaternion.identity);
                    
                   
                        var shieldOrNo = Random.value;
                        if (shieldOrNo >.7)
                        {
                            newEnemy.GetComponent<Enemy>().SpawnShield();
                        }
                   
                    _enemiesSpawned++;
                    _enemiesOnScreen ++;
                    newEnemy.transform.parent = _enemyHolder.transform;
                }
                else
                {
                    yield return new WaitForSeconds(_timeToWait);
                }
                

               if(_enemiesSpawned >= (int)_maxEnemies)
                {      
                     _enemiesSpawned=0;
                    _timeToSpawn -= .2f;
                    _maxEnemies += .5f;
                    _maxEnemiesOnScreen += .3f;                    
                    _level ++;
                    yield return new WaitForSeconds(_timeToWait);    
                }
                // all enemies spawned
            }

            yield return null;            
        }
    }
    
    IEnumerator ShowLevel()
    {

        _levelText.text = "Level: "+_level;
        _levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _levelText.text = "GET READY!";
        yield return new WaitForSeconds(1f);
        _levelText.gameObject.SetActive(false);
        yield return new WaitForSeconds (2f);

    }

    void ChooseEnemy()
    {
        _weightedTotal = 0;

        var _maxSpawnEnemyNumber = _level;
        if (_maxSpawnEnemyNumber > _enemyPrefab.Length)
        {
            _maxSpawnEnemyNumber = _enemyPrefab.Length;
        }

        int[] enemyTable =
        {
            100, // 0 main enemy
            93, // 1 flip up
            87, // 2 side by side
            80, // 3 circler
            75 // big boy
        };

        int[] enemyID =
        {
            0,
            1,
            2,
            3,
            4
        };

        for(int i = 0; i < _maxSpawnEnemyNumber; i++)
        {
            _weightedTotal += enemyTable[i];
        }

        var randomNumber = Random.Range(0, _weightedTotal);
        var x = 0;
        foreach (var weight in enemyTable)
        {
             if(randomNumber <= weight)
                {
                    _spawnEnemyNumber = enemyID[x];
                    return;
                }                
                else
                {
                    x++;
                    randomNumber -= weight;
                }   
        }


    }
    IEnumerator SpawnPowerUpRoutine()
    {
        while(!_stopSpawning)
        {   
            if(_firstPowerupSpawn)
            {
                _firstPowerupSpawn = false;
                yield return new WaitForSeconds(30f);
            }

            float randomX = Random.Range(-xPos,xPos);
            Vector3 posToSpawn = new Vector3(randomX, Ypos,0);

            ChooseAPowerup();

            GameObject newPowerUp = Instantiate(_powerUps[_powerupToSpawn], posToSpawn,Quaternion.identity);
            float _powerupTime = Random.Range(_minpowerupTime,_maxpowerupTime);
            yield return new WaitForSeconds(_powerupTime);   
        }
    }

    void ChooseAPowerup()
    {
        _weightedTotal = 0;

           int[] powerupTable =
           {
               700, // ammo
               300, // health
               275, // shield
               200, // missile
               120, // speed  
               75, // tripleshot 
               40 // negative           
           };
            int[] powerupToAward =
           {
               3, //ammo
               4, //health
               2, //shield
               5, //missile
               1, // speed
               0, // tripleshot
               6 // negative
           };

            foreach(var item in powerupTable)
            {
                _weightedTotal += item;
            }

            var randomNumber = Random.Range(0, _weightedTotal);
            var i = 0;
            
            foreach(var weight in powerupTable)
            {
                if(randomNumber <= weight)
                {
                    _powerupToSpawn = powerupToAward[i];
                    return;
                }                
                else
                {
                    i++;
                    randomNumber -= weight;
                }
            }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
