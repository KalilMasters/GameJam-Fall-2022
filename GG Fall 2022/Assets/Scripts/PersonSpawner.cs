using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonSpawner : MonoBehaviour
{
    [SerializeField] private PersonController personPrefab;
    [SerializeField] private SpawnArea spawnArea;
    [SerializeField] private float timeBetweenPersonSpawns;
    [SerializeField] private float minSpawns, maxSpawns;
    Color[] peopleColors;

    private void Awake()
    {
        peopleColors = new Color[] { Color.red, Color.blue, Color.yellow, Color.magenta};
        StartCoroutine(SpawnPersons());
    }
    IEnumerator SpawnPersons()
    {
        while (true)
        {
            int spawnAmount = (int)Random.Range(minSpawns, maxSpawns);
            while(spawnAmount > 0)
            {
                SpawnRandomPerson();
                spawnAmount--;
            }
            yield return new WaitForSeconds(timeBetweenPersonSpawns);
        }
    }
    private void SpawnRandomPerson()
    {
        PersonController newPerson = Instantiate(personPrefab, spawnArea.GetRandomPositionInBounds(), Quaternion.identity, transform);
        newPerson.Init(peopleColors[Random.Range(0, 4)]);
    }
}
