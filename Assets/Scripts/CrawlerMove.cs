using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrawlerMove : MonoBehaviour
{
    public Transform player;
    static Animator anim;
    public UnityEngine.UI.Image panel;
    public Transform[] goals;
    UnityEngine.AI.NavMeshAgent agent;
    Vector3 smoothDeltaPosition = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        panel.CrossFadeAlpha(0, 2, false);
        anim = GetComponent<Animator>();
        anim.SetBool("isIdle", false);
        anim.SetBool("isCrawling", true);
        int index = Random.Range(0, goals.Length);
        agent.destination = goals[index].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.destination, this.transform.position) < 2.0f)
        {
            int index = Random.Range(0, goals.Length);
            agent.destination = goals[index].position;
        }
        

        //////
        Vector3 direction = player.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);

        if (Vector3.Distance(player.position, this.transform.position) < 10)
        {
            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            agent.Stop();
            anim.SetBool("isIdle", false);
            if (direction.magnitude > 7)
            {
                this.transform.Translate(0, 0, 0.0125f);
                anim.SetBool("isCrawling", true);
                anim.SetBool("isCrawlingFast", false);
                anim.SetBool("isPouncing", false);
            }
            else if (direction.magnitude > 3)
            {
                this.transform.Translate(0, 0, 0.03f);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrawling", false);
                anim.SetBool("isCrawlingFast", true);
                anim.SetBool("isPouncing", false);
            }
            else
            {
                this.transform.Translate(0, 0, .05f);
                anim.SetBool("isIdle", false);
                anim.SetBool("isCrawling", false);
                anim.SetBool("isCrawlingFast", false);
                anim.SetBool("isPouncing", true);
                this.transform.Translate(0, .05f, 0);
            }
        }
        else
        {
            agent.Resume();
            anim.SetBool("isIdle", false);
            anim.SetBool("isCrawling", true);
            anim.SetBool("isCrawlingFast", false);
            anim.SetBool("isPouncing", false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Wall")
        {

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

}
