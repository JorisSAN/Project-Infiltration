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

    float visDist = 10.0f;
    float visAngle = 30.0f;
    float shootDist = 7.0f;


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
    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude <visDist && angle < visAngle)
        {
            return true;
        }
        return false;
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
    public Idle(GameObject _npc, NavMeshAgent _agent,  Transform _player)
        : base(_npc, _agent,  _player)
    {
        name = STATE.IDLE;
    }
    public override void Enter()
    {
        base.Enter();
      //  anim.SetTrigger("isIdle");
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
        else if(Random.Range(0,100)<10)
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT;
        }

    }
    public override void Exit()
    {
        //anim.ResetTrigger("isIdle");
        base.Exit();
    }
}


public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Transform _player)
        : base(_npc, _agent, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0 ; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if(distance < lastDist)
            {
                currentIndex = i-1;
                lastDist = distance;
            }    
        }
     //   anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if(currentIndex>= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }
        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
       // anim.ResetTrigger("isWalking");
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
            else if (!CanSeePlayer())
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
        if (!CanAttackPlayer())
        {
            nextState = new Search(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
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
        base.Enter();
        lastPosition = player.position;
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if (agent.remainingDistance < 1)
        {
            new Idle(npc, agent, player);
        }

    }
}