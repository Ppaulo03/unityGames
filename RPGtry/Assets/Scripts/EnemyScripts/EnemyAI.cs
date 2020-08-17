using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float nextWayPointDistant = 1f;


    Path path;
    int currentWayPoint = 0;

    Seeker seeker;
    Rigidbody2D myRigidbody;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        myRigidbody = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath",0f, 0.5f);
    }

    void FixedUpdate()
    {

        if(path == null || currentWayPoint >= path.vectorPath.Count){
            return;
        }
        Vector2 direction =((Vector2) path.vectorPath[currentWayPoint] - myRigidbody.position).normalized;
        myRigidbody.velocity = direction*speed;

        float distance = Vector2.Distance(myRigidbody.position,path.vectorPath[currentWayPoint]);

        if(distance< nextWayPointDistant){
            currentWayPoint ++;
        }

    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWayPoint = 0;
        }
    }
    
    void UpdatePath(){
        if(seeker.IsDone()) seeker.StartPath(myRigidbody.position, target.position, OnPathComplete);
    }
}
