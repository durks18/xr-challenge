using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject Player;
    public NavMeshAgent navMeshAgent;  // Nav Mesh Agent component
    public float startWaitTime = 4;    // wait time of each action
    public float timeToRotate = 2;     // wait time when the enemy detect near the player without seeing
    public float speedWalk = 6;        // enemy walking speed
    public float speedRun = 10;         // enemy running speed

    public float viewRadius = 15;      // radius of the enemy view
    public float viewAngle = 90;       // Angle of the enemy view 
    public LayerMask playerMask;       // to detect the player with Raycast
    public LayerMask obstacleMask;     // to detect obstacles with Raycast
    public float meshResolution = 1f;  // how many Rays will be cast per degree 
    public int edgeIterations = 4;     // number of iterations to get a better performance of the mesh filter when the Raycast hit an obstacle
    public float edgeDistance = 0.5f;  // max distance to calculate the minimum and maximum Raycast when it hits something
    public int maxHealth = 1; // enemies max health
    public int currentHealth; // enemy's current health
    public Player playerHealth; //reference to the player health

    public Transform[] waypoints;      // all the way points the enemy patrols
    int m_CurrentWayPointIndex;        // current waypoint the enemy is patroling

    Vector3 playerLastPosition = Vector3.zero;  // last position of the player when they was near the enemy
    Vector3 m_PlayerPosition;         // last position of the player when they are seen by the enemy 

    float m_WaitTime;                 // variable of the wait time that makes the delay
    float m_TimeToRotate;             // variable of the wait time to rotate when the player is near by that makes the delay
    bool m_playerInRange;             // if the player is in range of vision, state of chasing
    bool m_PlayerNear;                // if the plater us near state of hearing
    bool m_IsPatrol;                  // if the enemy is patroling, state of patroling
    bool m_CaughtPlayer;              // if the enemy has caught the player


    void start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_WaitTime = startWaitTime;  // set the wait time variable that will change
        m_TimeToRotate = timeToRotate;

        m_CurrentWayPointIndex = 5;// set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;  // set the navmesh speed with the normal speed of the enemy
        navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position); // set the destination of the first waypoint

        currentHealth = 5;

    }

    void Update()
    {
        EnviromentView();             //check whether or not the player is in the enemy's field of view

        if(!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
        
    }

    private void Chasing()
    {
        m_PlayerNear = false;         // set false that the player is near because the enemy already sees the player
        playerLastPosition = Vector3.zero;  // reset the player near position

        if(!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition); // set the destination of the enemy to the player location
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)  // control if the enemy arrives to the players location
        {
            if(m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)>= 6f)
            {

                // check if the enemy is not near to the player, returns to patrol after the wait time delay 
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position)>= 1f)
                {

                    // wait if the current position is not the player position
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {

            // check if the enemy detects near the player, so the enemy will move to that position
            if(m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {

                // the enemy waits for a moment and then goes to the last player position
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;  // the okayer is not near when the enemy is patroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);  // set the enemy destination to the next way point
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {

                // if the enemy arrives to the waypoint position then wait for a moment and go to the next one
                if(m_WaitTime<= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }    
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }    

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        
      //m_CurrentWayPointIndex = (m_CurrentWayPointIndex+1) % waypoints.Length;
     // navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
        

        if (m_CurrentWayPointIndex >= 1)
        {
            m_CurrentWayPointIndex = 0;
        }
        else
        {
            m_CurrentWayPointIndex = (m_CurrentWayPointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
        }
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;

    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player)<= 0.3)
        {
            if(m_WaitTime <=0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWayPointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            if (m_playerInRange)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);

            currentHealth--;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}

