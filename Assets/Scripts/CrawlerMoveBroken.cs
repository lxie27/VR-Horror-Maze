using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrawlerMoveBroken : MonoBehaviour
{
    private float moveSpeed = 1.0f;
    private float chaseSpeed = 2.0f;
    private Ray sight;
    private string mode;
    private Vector3 direction;
    private Vector3 pos;
    public UnityEngine.UI.Image panel;

    public Transform playerPos;
    private float viewDistance = 10.0f;
    private float viewAngle = 45.0f;
    Animator anim;
    private AnimatorClipInfo[] clipInfo;
    private Vector3 waypoint;
    public Vector3[] waypoints;
    private Vector3 currentWaypoint;
    private float wanderingRadius = 3.0f;

    public enum Behavior
    {
        idle, patrol, chase, attack
    }

    public Behavior currentState;

    private int wpIndex = 0; // index of the current waypoint in array


    public LayerMask layer; // layer of all obstacles


    // Start is called before the first frame update
    void Start()
    {
        currentState = Behavior.idle;
        anim = gameObject.GetComponent<Animator>();
        panel.CrossFadeAlpha(0, .1f, false);
        direction = -transform.right;

        anim.SetTrigger("Patrol");
        waypoint = SelectRandomPoint();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current behavior: " + currentState);
        switch (currentState)
        {
            case Behavior.idle:

                currentState = Behavior.patrol;
                break;

            case Behavior.patrol:
                if (findThePlayer())
                {
                    currentState = Behavior.chase;
                    transform.LookAt(playerPos);
                    transform.position = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * chaseSpeed * Time.deltaTime);
                }
                else
                {
                    Patrol();
                }
                break;

            case Behavior.chase:
                if (Vector3.Distance(transform.position, playerPos.position) < 0.5f)
                {
                    Attack();
                }
                else if (findThePlayer())
                {
                    Chase();
                }
                else
                {
                    Patrol();
                }
                break;

            case Behavior.attack:
                if (Vector3.Distance(transform.position, playerPos.position) < 0.5f)
                {
                    Attack();
                }
                break;
        }

    }

    private void Patrol()
    {
        if (Physics.OverlapSphere(currentWaypoint, 1, layer).Length != 0)
        {
            Debug.Log("Arrived at waypoint" + currentWaypoint);
            currentWaypoint = SelectRandomPoint();
        }

        if (WpReached(transform.position, currentWaypoint, 2.0f))
        {
            Debug.Log("Arrived at waypoint" + currentWaypoint);
            currentWaypoint = SelectRandomPoint(); // select a new waypoint
        }

        currentWaypoint.y = transform.position.y;
        //transform.LookAt(currentWaypoint);
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);  // move towards the current waypoint
    }

    private void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * chaseSpeed * Time.deltaTime);
        currentState = Behavior.chase;
    }
    private void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos.position, moveSpeed * 5.0f * Time.deltaTime);
        currentState = Behavior.idle;
    }
    private Vector3 SelectRandomPoint()
    {
        // select a random point inside a circle
        Vector3 point = Random.insideUnitSphere * wanderingRadius + transform.position;
        // eliminate the change of Y
        point.y = 0;
        // Add the difference to the current location of enemy
        Debug.Log("Current point: " + transform.position);
        Debug.Log("Selected point: " + point);
        return point;
    }

    private bool WpReached(Vector3 position, Vector3 target, float allowance)
    {
        return Vector3.Distance(position, target) <= allowance;
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision");
        if (col.gameObject.tag == "Wall")
        {
            //transform.rotation = Quaternion.Inverse(transform.rotation);
            //transform.rotation = Quaternion.Euler(transform.rotation.x, Random.Range(-70.0f, 70.0f), transform.rotation.z);
            direction = -transform.forward;
        }

        if (col.gameObject.tag == "Player")
        {
            Debug.Log("End the game");
            endGame();
        }
    }

    void endGame()
    {
        panel.CrossFadeAlpha(1, 2, false);
        Application.LoadLevel(Application.loadedLevel);
        panel.CrossFadeAlpha(0, 2, false);

    }

    bool findThePlayer()
    {
        if (Vector3.Distance(transform.position, playerPos.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerPos.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(-transform.forward, directionToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2) //check if player is in the angle
            {
                if (!Physics.Linecast(transform.position, playerPos.position)) //draw ray between player and crawler
                {
                    return true;
                }

            }
        }
        return false;
    }

}
