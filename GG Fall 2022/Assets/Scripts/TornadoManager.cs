using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoManager : MonoBehaviour
{

    public List<Tornado> tList;
    public SpawnArea[] spawnAreas;
    int spawnAreaCount = 4;
    int spawnAreaCounter = 0;
    public float spawnTime; // Spawns tornado every x time
    float spawnTimer = 0; // Keeps track of when the tornado should spawn
    public Tornado tornadoPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tornado t in tList)
        {
            if (t.gameObject.activeSelf)
                t.move();
            if (t.gameObject.activeSelf)
                t.checkTime(Time.deltaTime);
        }
        SpawnCheck();
    }
    void SpawnCheck()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnTime)
        {
            Spawn();
            spawnTimer -= spawnTime;
        }
    }
    
    void Spawn()
    {
        //Vector3 spawnPosition = new Vector3(Random.Range(tornadoSpawn.position.x - spawnSpread, tornadoSpawn.position.x + spawnSpread), spawnY, Random.Range(tornadoSpawn.position.z - spawnSpread, tornadoSpawn.position.z + spawnSpread));
        Quaternion spawnRotation = Quaternion.identity;
        tList.Add(Instantiate(tornadoPrefab, spawnAreas[spawnAreaCounter %= spawnAreaCount].GetRandomPositionInBounds(), spawnRotation));
        spawnAreaCounter++;
    }
}
