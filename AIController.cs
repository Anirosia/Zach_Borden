using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    Transform target;
    Transform player;
    Transform lastKnown;
    NavMeshAgent agent;

    int state=0; //0 = Patrol/retreat 1 = Chase 2 = search 3 = attack 
    GameObject[] waypoints;
    int currentWaypoint=0; //index in waypoints array of current waypoint target
    public float chaseRange = 20; //determines the distance at which chase mode is engaged
    public float searchRange = 25; //distance at which mode is switched from chase to search
    public float attackRange = 5; //distance at which mode is switched from chase to attack

    public float attackTime = 5;
    public float searchTime = 5; //time in seconds after which mode is switched from search to retreat
    public float enemyHealth;

    float timeStamp;
    float hitTimer;
    float despawnTimer;
    float setActiveTimer;

    bool hit;
    bool dead;

    Animator animator;

    PersonController player_cs;

    // Start is called before the first frame update
    void Start()
    {
        //waypoints are ordered in the array by order in hierarchy
        if(gameObject.tag == "Enemy01")
        {
            waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
            target = waypoints[0].transform;
        }
        else
        {
            target = gameObject.transform;
        }
        agent = gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastKnown = GameObject.FindGameObjectWithTag("LastKnown").transform;
        animator = gameObject.GetComponentInChildren<Animator>();

        player_cs = GameObject.FindGameObjectWithTag("Player").GetComponent<PersonController>();
        hitTimer = 0.1f;
        despawnTimer = 0.1f;
        setActiveTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        
        agent.destination = target.position;

        if(agent.velocity!= Vector3.zero)
        {
            animator.SetBool("run",true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        switch (state)
        {

            //patrol mode
            case 0:
            animator.SetBool("swing", false);
                //player detected?
                if (Vector3.Distance(gameObject.transform.position, player.position) <= chaseRange)
                {
                    state = 1;
                    target = player;
                }
                if(Vector3.Distance(gameObject.transform.position, target.position)<=1)
                {
                    if (gameObject.tag == "Enemy01")
                    {
                        target = nextWaypoint();
                    }
                }
                break;


            //chase mode
            case 1:
                
                //in attack range?
                if (Vector3.Distance(gameObject.transform.position, player.position)<=attackRange)
                {
                    timeStamp = Time.time;
                    state = 3;
                }
                //player leaves chase range?
                else if(Vector3.Distance(gameObject.transform.position, player.position) >= searchRange)
                {
                    lastKnown.position = player.position;
                    target = lastKnown;
                    timeStamp = Time.time;
                    state = 2;
                }
                break;

            //search mode
            case 2:
                
                //in chase range?
                if (Vector3.Distance(gameObject.transform.position, player.position) <= chaseRange)
                {
                    state = 1;
                    target = player;
                }
                //search time elapsed?
                else if (Time.time > timeStamp + searchTime)
                {
                    if (gameObject.tag == "Enemy01")
                    {
                        //find closest waypoint to return to 
                        findClosest();
                    }
                        state = 0;
                }
                break;

            //attack mode
            case 3:
                //stops agent so player can get away
                //agent.isStopped = true;
                agent.speed = 0;
                gameObject.transform.LookAt(player.transform.position);
                animator.SetBool("swing", true);
                if (Time.time >= timeStamp + attackTime)
                {
                    //agent.isStopped = false;
                    agent.speed = 2;
                    state = 0;
                }
                if(player_cs.dead)
                {
                    animator.SetBool("swing", false);
                }
                if (hit)
                {
                    animator.SetBool("Hit", true);
                    animator.SetBool("swing", false);
                    hitTimer -= Time.deltaTime;
                    if(hitTimer < 0)
                    {
                        timerReset();
                        hit = false;
                        animator.SetBool("Hit", false);
                    }
                }

                break;
            //Dead
            case 4:
                agent.isStopped = true;
                despawnTimer -= Time.deltaTime;
                if (despawnTimer < 0)
                {
                    animator.SetBool("Dead", false);
                    setActiveTimer -= Time.deltaTime;
                    if(setActiveTimer < 0)
                    {
                        gameObject.SetActive(false);
                    }
                }
                break;

            default:
                break;
        }

        if (enemyHealth <= 0 && !dead)
        {
            Dead();
        }
    }

   
    private Transform nextWaypoint()
    {
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0;
        }
        else
        {
            currentWaypoint++;
        }
        return waypoints[currentWaypoint].transform;
    }

    void findClosest()
    {
        float closest = Vector3.Distance(gameObject.transform.position, waypoints[0].transform.position);
        float compare;

        for (int x = 1; x < waypoints.Length; x++)
        {
            compare = Vector3.Distance(gameObject.transform.position, waypoints[x].transform.position);
            if (Vector3.Distance(gameObject.transform.position, waypoints[x].transform.position) <= closest)
            {
                closest = compare;
                target = waypoints[x].transform;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        hit = true;
        enemyHealth = enemyHealth - damage;
        
    }

    void timerReset()
    {
        hitTimer = 0.1f;
    }
    void Dead()
    {
        dead = true;
        animator.SetBool("Dead", true);
        state = 4;
    }

}
