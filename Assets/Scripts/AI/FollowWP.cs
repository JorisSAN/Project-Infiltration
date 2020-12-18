using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
   
    public GameObject wpManager;
    GameObject[] wps;
    UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void GoToRight()
    {
        agent.SetDestination(wps[3].transform.position);
     //   g.AStar(currentNode, wps[3]);
     //   currentWP = 0;
    }

    public void GoToDown()
    {
        agent.SetDestination(wps[9].transform.position);

        //      g.AStar(currentNode, wps[9]);
        //    currentWP = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {


    }
}
