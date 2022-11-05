using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public static SpawnArea Instance;
    Collider myCol;
    Vector3 areaBounds;
    public bool GameBoundsInstance;
    private void OnDrawGizmos()
    {
        myCol = GetComponent<Collider>();
        areaBounds = myCol.bounds.size;

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, areaBounds);
    }
    private void Awake()
    {
        if(GameBoundsInstance)
         Instance = this;
        myCol = GetComponent<Collider>();
        areaBounds = myCol.bounds.size;
    }
    public Vector3 GetRandomPositionInBounds()
    {
        Vector3 extents = areaBounds / 2;
        Vector3 pos = new Vector3(Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z));
        return transform.position + pos;
    }
}
