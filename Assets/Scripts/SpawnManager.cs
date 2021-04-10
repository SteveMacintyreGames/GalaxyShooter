using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(!_stopSpawning)
        {   
            yield return new WaitForSeconds(3f);         
            float randomX = Random.Range(-xPos,xPos);
            Vector3 posToSpawn = new Vector3(randomX,Ypos,0);
            GameObject newEnemy = Instantiate (_enemyPrefab[Random.Range(0,_enemyPrefab.Length)],posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyHolder.transform;
            yield return new WaitForSeconds(_timeToWait);            
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
