using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Tornado : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int currentWaypoint;
    GameObject waypointSpawnerGameObject;
    SpawnArea waypointSpawner;
    //public float waypointSpread;
    public Transform emptyTemp;
    public Transform waypointHolder;

    [SerializeField] float tornadoTime;
    float tornadoTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentWaypoint = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[0].position);
    }
    void Awake()
    {
        waypointSpawnerGameObject = GameObject.Find("SpawnArea (Tornado WayPoint)");
        waypointSpawner = waypointSpawnerGameObject.GetComponent<SpawnArea>();
        initWaypoints();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void move()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            Vector3 waypointTemp = waypointSpawner.GetRandomPositionInBounds();
            Destroy(waypoints[currentWaypoint].gameObject);
            waypoints[currentWaypoint] = Instantiate(emptyTemp, waypointTemp, Quaternion.identity);
            waypoints[currentWaypoint].transform.SetParent(waypointHolder);
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }
    public void checkTime(float time)
    {
        tornadoTimer += time;
        if(tornadoTimer >= tornadoTime)
        {
            this.gameObject.SetActive(false);
        }
    }
    void initWaypoints()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 waypointTemp = waypointSpawner.GetRandomPositionInBounds();
            waypoints[i] = Instantiate(emptyTemp, waypointTemp, Quaternion.identity);
            waypoints[i].transform.SetParent(waypointHolder);
        }
    }
}
