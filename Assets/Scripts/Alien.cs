using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alien : MonoBehaviour
{

    enum AlienState
    {
        patrolling,
        chasing,
        dead
    }

    private AlienState currentState = AlienState.patrolling;

    private Rigidbody rb;


    public Vector3 currentWaypoint;
    public float patrolWaypointBuffer = 1f;
    public bool hasWaypoint = false;
    Vector3 directionToMove;

    [Header("Movement")]
    public float patrolSpeed = 10f;
    public float chaseSpeed = 10f;



    [Header("Awareness")]
    public float patrolRadius = 20f;
    public float viewRange = 5f;

    [Header("Combat")]
    public float attackRange = 1f;
    public float attackDelay = 2;
    public float attackDamage = 20f;
    private float lastAttackTime = 0f;


    public float health = 100f;
    public float deathEnergyBonus = 10f;

    private PlanetGravity planet;
    public LayerMask planetLayer;

    public Slider healthBar;

    public Transform roverTransform;


    public GameObject biologyMaterialPrefab;

    private int spawnIndex = -1;

    private float lastNoiseTime = 0f;

    private float noiseDelay = 6f;

    // Start is called before the first frame update
    void Start()
    {
        planet = FindObjectOfType<PlanetGravity>();
        rb = GetComponent<Rigidbody>();
        roverTransform = FindObjectOfType<RoverController>().transform;
        healthBar.maxValue = health;
        healthBar.value = health;
        FindNewWaypoint();
        GetComponent<AudioSource>().volume *= AppManager.instance.fxVolume;
        noiseDelay = Random.Range(6f, 20f);
    }


    public void SetSpawnInfo(int _index)
    {
        spawnIndex = _index;
    }



    /* 
     * Find a random point within a sphere centered around the alien with a radius of patrolRadius.
     * Project this point onto the surface of the planet and set the projected point as the new waypoint.
     */
    void FindNewWaypoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * patrolRadius + transform.position;
        Vector3 dirToCenter = (randomPoint - planet.transform.position);
        float dirToCenterMag = dirToCenter.magnitude;
        Vector3 scaledVector = (planet.GetComponent<SphereCollider>().radius * planet.transform.localScale.x / dirToCenterMag) * dirToCenter;
        currentWaypoint = planet.transform.position + scaledVector;
        hasWaypoint = true;


    }

    // Update is called once per frame
    void Update()
    {   
        
        
        if (Time.time - lastNoiseTime >= noiseDelay){
            GetComponent<AudioSource>().Play();
            lastNoiseTime = Time.time;
            noiseDelay = Random.Range(6f, 20f);

        }

        //if we're dead don't do anything
        if (currentState == AlienState.dead) { return; }
        if (roverTransform == null)
        {
            currentState = AlienState.dead;
            return;
        }

        //if we can see the rover then start chasing
        if (Vector3.SqrMagnitude(transform.position - roverTransform.position) <= viewRange * viewRange)
        {
            if (currentState != AlienState.chasing)
            {
                currentState = AlienState.chasing;
            }
        }
        else
        {
            //if we can no longer see the rover then stop chasing
            if (currentState == AlienState.chasing)
            {
                currentState = AlienState.patrolling;
            }
        }

        RaycastHit hit; //this variable is used when checking the surface normal of the ground
        switch (currentState)
        {
            case AlienState.patrolling:
                if (hasWaypoint)
                {
                    //if we are close enough to the current waypoint, then generate a new waypoint
                    if (Vector3.SqrMagnitude(transform.position - currentWaypoint) < patrolWaypointBuffer * patrolWaypointBuffer)
                    {
                        hasWaypoint = false;
                        FindNewWaypoint();
                        break;
                    }

                    directionToMove = (currentWaypoint - transform.position).normalized;
                    //We want to ensure our move direction is parallel to the surface of the planet
                    if (Physics.Raycast(transform.position, directionToMove, out hit, 5f, planetLayer))
                    {
                        directionToMove = Vector3.ProjectOnPlane(directionToMove, hit.normal);
                    }
                    //look where we are going
                    transform.rotation = Quaternion.LookRotation(directionToMove, Vector3.up);

                    //this makes sure that we are still aligned with the surface of the planet. 
                    transform.rotation = Quaternion.FromToRotation(transform.up, (transform.position - planet.transform.position).normalized) * transform.rotation;

                }
                break;
            case AlienState.chasing:

                //if we are within attack range, then attack the rover
                if (Vector3.SqrMagnitude(transform.position - roverTransform.position) <= attackRange * attackRange)
                {
                    //delay the attacks to make it fair for the player
                    if (Time.time - GameManager.instance.worldStartTime - lastAttackTime >= attackDelay)
                    {
                        PlayerManager.instance.TakeDamage(attackDamage);
                        lastAttackTime = Time.time - GameManager.instance.worldStartTime;
                    }
                }

                directionToMove = (roverTransform.position - transform.position).normalized;
                //We want to ensure our move direction is parallel to the surface of the planet
                if (Physics.Raycast(transform.position, directionToMove, out hit, 5f, planetLayer))
                {
                    directionToMove = Vector3.ProjectOnPlane(directionToMove, hit.normal);
                }
                //look where we are going
                transform.rotation = Quaternion.LookRotation(directionToMove, Vector3.up);

                //this makes sure that we are still aligned with the surface of the planet. 
                transform.rotation = Quaternion.FromToRotation(transform.up, (transform.position - planet.transform.position).normalized) * transform.rotation;
                break;
        }
    }

    private void FixedUpdate()
    {
        //if we're dead don't do anything
        if (currentState == AlienState.dead) { return; }
        if (roverTransform == null)
        {
            currentState = AlienState.dead;
            return;
        }


        //handle movement
        switch (currentState)
        {
            case AlienState.patrolling:
                rb.MovePosition(transform.position + directionToMove * Time.fixedDeltaTime * patrolSpeed);
                break;
            case AlienState.chasing:
                rb.MovePosition(transform.position + directionToMove * Time.fixedDeltaTime * chaseSpeed);
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        //draw helpful debug information
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        if (currentWaypoint != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(currentWaypoint, .5f);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.value = health;
        if (health <= 0)
        {

            Die();
        }
    }

    private void Die()
    {
        if (currentState == AlienState.dead) { return; }
        currentState = AlienState.dead;
        Instantiate(biologyMaterialPrefab, transform.position + (transform.up * 3), Quaternion.identity);
        if (spawnIndex != -1)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.GetComponent<AlienSpawner>().AlienDied(spawnIndex);
            }
        }
        PlayerManager.instance.GetComponent<EnergyEquipment>().energy += deathEnergyBonus;
        PlayerManager.instance.GetComponent<EnergyEquipment>().energy = Mathf.Clamp(PlayerManager.instance.GetComponent<EnergyEquipment>().energy, 0, PlayerManager.instance.GetComponent<EnergyEquipment>().energyCapacity);
        Destroy(this.gameObject);
    }
}
