using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE,PATROL,PURSUE,ATTACK,SEARCH,SLEEP
    }

    public enum EVENT
    {
        ENTER,UPDATE,EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    //protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    float shootDist = 2.0f;


    public State(GameObject _npc,NavMeshAgent _agent, Transform _player)
    {
        npc = _npc;
        agent = _agent;
    //    anim = _anim;
        stage = EVENT.ENTER;
        player = _player;

    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT) 
        {
            Exit();
            return nextState;
        }
        return this;
    }
    public bool CanSeePlayer(GameObject npc)
    {
        return npc.GetComponent<FieldOfView>().playerSeen;

    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }
}


public class Idle : State
{
    bool patrol;
    public Idle(GameObject _npc, NavMeshAgent _agent,  Transform _player)
        : base(_npc, _agent,  _player)
    {
        name = STATE.IDLE;
    }
    public override void Enter()
    {
        patrol = npc.GetComponent<AIController>().basePatrol;
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer(npc))
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
        else if(patrol && Random.Range(0,6000)<10)
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT;
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}


public class Patrol : State
{
    int currentIndex = -1;
    GameObject[] checkpoints;
    public Patrol(GameObject _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        checkpoints = npc.GetComponent<AIController>().waypoints;
        float lastDist = Mathf.Infinity;
        for (int i = 0 ; i <checkpoints.Length; i++)
        {
            GameObject thisWP = checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if(distance < lastDist)
            {
                currentIndex = i-1;
                lastDist = distance;
            }    
        }
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if(currentIndex>=checkpoints.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            agent.SetDestination(checkpoints[currentIndex].transform.position);
        }
        if (CanSeePlayer(npc))
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}


public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Transform _player)
       : base(_npc, _agent, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;

    }

    public override void Enter()
    {
        //anim.setTrigger("isRunning")
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, player);
                stage = EVENT.EXIT;

            }
            else if (!CanSeePlayer(npc))
            {
                nextState = new Search(npc, agent, player);
                stage= EVENT.EXIT;
            }
        }
    }
    public override void Exit()
    {
        //anim.ResetTrigger("isRunning);
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    public Attack(GameObject _npc, NavMeshAgent _agent, Transform _player)
       : base(_npc, _agent, _player)
    {
        name = STATE.ATTACK;

    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotationSpeed);
        if (!CanSeePlayer(npc))
        {
            nextState = new Search(npc, agent, player);
            stage = EVENT.EXIT;
        }
        else if (!CanAttackPlayer())
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
        else
        {
            //code de l'attaque
        }
    }

    public override void Exit()
    {
        agent.isStopped = false;

        base.Exit();
    }

}


public class Search : State
{
    Vector3 lastPosition;
    public Search(GameObject _npc, NavMeshAgent _agent, Transform _player)
       : base(_npc, _agent, _player)
    {
        name = STATE.SEARCH;

    }

    public override void Enter()
    {
        lastPosition = player.position;

        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(lastPosition);
        if (CanSeePlayer(npc))
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;

        }
        else if (agent.remainingDistance < 1)
        {
            nextState = new Idle(npc, agent, player);
            stage = EVENT.EXIT;

        }

    }
}