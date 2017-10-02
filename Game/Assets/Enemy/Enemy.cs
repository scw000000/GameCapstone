using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    private GameObject m_player;
    private NavMeshAgent m_nav;
    public int Health = 4;
    private string state = "idle";
    private bool alive = true;
    public Transform eyes;
    public AudioSource roar;
    private Animator anim;
    private float wait = 0f;
    private bool highAlert = false;
    private float alertness = 20f;
    private float deathTime = 3f;

    // Use this for initialization
    void Start() {
        m_nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_nav.speed = 1.2f;
        anim.speed = 1.2f;
    }

    //check the player
    public void checkSight() {
        if (alive) {
            RaycastHit rayHit;
            if (Physics.Linecast(eyes.position, m_player.transform.position, out rayHit)) {
                print("hit " + rayHit.collider.gameObject.name);
                if (rayHit.collider.gameObject.tag=="Player") {
                    if (state != "kill") {
                        state = "chase";
                        m_nav.speed = 3.0f;
                        anim.speed = 3.0f;
                        roar.pitch = 1.2f;
                        roar.Play();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetFloat("velocity", m_nav.velocity.magnitude);
        anim.SetBool("alive", alive);
        if (alive)
        {
            if (state == "idle")
            {
                Vector3 RandomPos = Random.insideUnitCircle * alertness;
                NavMeshHit NavHit;
                NavMesh.SamplePosition(transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
                m_nav.SetDestination(NavHit.position);
                state = "walk";
                if (highAlert)
                {
                    NavMesh.SamplePosition(m_player.transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
                    alertness += 5f;
                    if (alertness > 20f)
                    {
                        highAlert = false;
                        alertness = 20f;
                        m_nav.speed = 1.2f;
                        anim.speed = 1.2f;
                    }
                }
            }
            if (state == "walk")
            {
                if (m_nav.remainingDistance <= m_nav.stoppingDistance && !m_nav.pathPending)
                {
                    state = "search";
                    wait = 5f;
                }
            }
            if (state == "search")
            {
                if (wait > 5f)
                {
                    wait -= Time.deltaTime;
                    transform.Rotate(0f, 120f * Time.deltaTime, 0f);
                }
                else
                {
                    state = "idle";
                }
            }
            if (state == "chase")
            {
                m_nav.destination = m_player.transform.position;

                //lose sight of player
                float distance = Vector3.Distance(transform.position, m_player.transform.position);
                if (distance > 10f)
                {
                    state = "hunt";
                }
                if (distance < 2f)
                {
                    
                }
            }
            if (state == "hunt")
            {
                if (m_nav.remainingDistance <= m_nav.stoppingDistance && !m_nav.pathPending)
                {
                    state = "search";
                    wait = 5f;
                    highAlert = true;
                    alertness = 5f;
                    checkSight();
                }
            }
        }
        else {
            if (deathTime > 0f)
            {
                deathTime -= Time.deltaTime;
            }
            else {
                Destroy(this.gameObject);
            }
        }
     
		//nav.SetDestination (player.transform.position);
	}
    public void TakeDamage()
    {
        Health--;
        if(Health<=0)alive = false;
        
    }
}
