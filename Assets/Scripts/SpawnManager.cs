using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int _enemiesDestroyed =0;
    public int _enemiesOnScreen = 0;
    private float _maxEnemiesOnScreen = 1f;
    private bool _canSpawn = true;

    [SerializeField]
    private int _powerupToSpawn;

    private int _weightedTotal;

    private bool _firstPowerupSpawn = true;
    public bool _isBossFight = false;
    private int _bossFightLevel = 10;

    void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        if(!_isBossFight)
        {
        _level = 1;
        _timeToSpawn = 5f;
        _timeToWait = 5f;
        _maxEnemies = 4f;
        _maxEnemiesOnScreen = 3;
        _levelText.gameObject.SetActive(false);
        _minpowerupTime = 5f;
        _maxpowerupTime = 15f;
        _powerupTime = Random.Range(_minpowerupTime,_maxpowerupTime);
        }
        else
        {
            _level = 1;
            _timeToSpawn = 8f;
            _timeToWait = 8f;
            _maxEnemies = 1f;
            _maxEnemiesOnScreen = 1f;
            _enemiesOnScreen = 0;
            _levelText.gameObject.SetActive(false);
            _minpowerupTime = 5f;
            _maxpowerupTime = 10f;
            _powerupTime = _maxpowerupTime;

        }     
    }

    void Update()
    {
        //
    }
   
  
    //Decide on a percentage of Enemies & Powerups per level
    //Increase the number of enemies and powerups every few levels up to a limit.

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(ShowLevel());
    }

    // have enemy spawned count
    // max enemy count
    // destroyed enemy count
    // when destroyed enemies = max enemy count = new level.

    IEnumerator SpawnEnemyRoutine()
    {
        while(!_stopSpawning)
        {   

                yield return new WaitForSeconds(_timeToSpawn); //wait some time
                
                if (_enemiesOnScreen <= _maxEnemiesOnScreen)
                {
                    SpawnEnemy();
                }

                
                        

            yield return null;            
        }
    }

    private void SpawnEnemy()
    {
         float randomX = Random.Range(-xPos,xPos); //pick a random spot on the x axis
                Vector3 posToSpawn = new Vector3(randomX,Ypos,0); //put spawning position in variable
            
                ChooseEnemy();

                GameObject newEnemy = Instantiate (_enemyPrefab[Random.Range(0,_spawnEnemyNumber)],posToSpawn, Quaternion.identity);
                
                    //No shields for ships during bossfights.
                    float ShieldChance;
                    if(_isBossFight)
                    {
                        ShieldChance=1;
                    }
                    else
                    {
                        ShieldChance = .8f;
                    }
                    var shieldOrNo = Random.value;
                    if (shieldOrNo >ShieldChance)
                    {
                        newEnemy.GetComponent<Enemy>().SpawnShield();
                    }
                
                _enemiesSpawned++;
                _enemiesOnScreen ++;
                newEnemy.transform.parent = _enemyHolder.transform;    
    }
    public void EnemyDestroyed()
    {
        _enemiesDestroyed++;
        _enemiesOnScreen--;
        CheckAllEnemiesDestroyed();

    }

    public void CheckAllEnemiesDestroyed()
    {
         if(_enemiesDestroyed == (int)_maxEnemies)
                {  
                    if(!_isBossFight)
                    {
                     _enemiesSpawned = 0;
                     _enemiesDestroyed = 0;
                     _enemiesOnScreen = 0;
                    _timeToSpawn -= .2f;
                    _maxEnemies += .5f;
                    _maxEnemiesOnScreen += .5f;                    
                    _level ++;
                    StartCoroutine(ShowLevel());
                    }
                    else
                    {
                        _level = 999999999;
                        StartCoroutine(ShowLevel());
                    }   
                    if (_level == _bossFightLevel)
                    {
                        LoadBossFight();
                    }
                }  
    }
    void LoadBossFight()
    {
        SceneManager.LoadScene(2);
    }
    IEnumerator ShowLevel()
    {
        if (_level == 999999999)
        {
            _levelText.text = "Boss Fight!";
        }
        else
        {
            _levelText.text = "Level: "+_level;
        }
        
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
            90, // 2 sidewinder
            80, // 1 flip up
            75,  // rear shooter
            60, // 3 circler
            50,   // Dodger
            40,  // big boy
            30  //rammer
            
            
        };

        int[] enemyID =
        {
            0,  // main enemy
            2,  // sidewinder
            1,  // flip up
            6,  // rear shooter
            3,  // circler
            5,  //rammer  
            4,  // big boy                      
            7   // Dodger
        };

        if(_isBossFight)
        {
            _maxSpawnEnemyNumber = 7;
        }
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
            if(!_isBossFight)
            {
                if(_firstPowerupSpawn)
                {
                    _firstPowerupSpawn = false;
                    yield return new WaitForSeconds(30f);
                }
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
               50, // ammo
               25, // missile  
               16, // health   
               8, // shield   
               6, // speed     
               3, // tripleshot 
               2 // negative           
           };
            int[] powerupToAward =
           {
               3, //ammo
               5, //missile
               4, //health
               2, //shield
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
