using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerUps;
    private float _powerUpTime;

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
    private int _maxEnemies;
    
    [SerializeField]
    private int _enemiesSpawned=0;

    void Start()
    {
        _level = 1;
        _timeToSpawn = 4f;
        _maxEnemies = 3;
        _levelText.gameObject.SetActive(false);
    }


    public void StartSpawning()
    {
        StartCoroutine(ShowLevel());
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(!_stopSpawning)
        {   
            
            for (int i = 0; i < _maxEnemies; i++)
            {
                yield return new WaitForSeconds(_timeToSpawn);         
                float randomX = Random.Range(-xPos,xPos);
                Vector3 posToSpawn = new Vector3(randomX,Ypos,0);
                ChooseEnemy();
                GameObject newEnemy = Instantiate (_enemyPrefab[Random.Range(0,_spawnEnemyNumber)],posToSpawn, Quaternion.identity);
                _enemiesSpawned++;
                newEnemy.transform.parent = _enemyHolder.transform;
            }

            if(_enemiesSpawned >= _maxEnemies)
            {                
                _enemiesSpawned=0;
                _timeToSpawn -= .3f;
                _maxEnemies += (int)_level/2;
                _level ++;
                StartCoroutine(ShowLevel());
            }
            yield return new WaitForSeconds(_timeToWait);            
        }
    }
    IEnumerator ShowLevel()
    {
        _levelText.text = "Level: "+_level;
        _levelText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        _levelText.gameObject.SetActive(false);

    }
    void ChooseEnemy()
    {
             _spawnEnemyNumber = _level;
        if (_spawnEnemyNumber > _enemyPrefab.Length)
        {
            _spawnEnemyNumber = _enemyPrefab.Length;
        }
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        while(!_stopSpawning)
        {   
            yield return new WaitForSeconds(3f); 
            float randomX = Random.Range(-xPos,xPos);
            Vector3 posToSpawn = new Vector3(randomX, Ypos,0);
            GameObject newPowerUp = Instantiate(_powerUps[Random.Range(0,_powerUps.Length)], posToSpawn,Quaternion.identity);
            _powerUpTime = Random.Range(3,8);
            yield return new WaitForSeconds(_powerUpTime);   
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
