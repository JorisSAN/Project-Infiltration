using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
   // Animator anim;
    public Transform player;
    public State currentState;
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent=this.GetComponent<NavMeshAgent>();
        //anim = this.GetComponent<Animator>();
        currentState = new Idle(this.gameObject, agent, player);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("test");
        currentState = currentState.Process();   
    }
}
