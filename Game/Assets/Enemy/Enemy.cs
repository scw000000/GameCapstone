using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    private GameObject _player;
    private NavMeshAgent _nav;
    public Transform _eyes;
    public AudioSource _roar;
    private Animator _anim;

    public int Health = 4;
    private string state = "idle";
    private bool alive = true;
    private float wait = 0f;
    private bool highAlert = false;
    private float alertness = 20f;
    private float deathTime = 3f;

    // Use this for initialization
    void Start() {
        _nav = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

        _nav.speed = 1.2f;
        _anim.speed = 1.2f;
    }

    //check the player
    public void checkSight() {
        if (alive) {
            RaycastHit rayHit;
            if (Physics.Linecast(_eyes.position, _player.transform.position, out rayHit)) {
                print("hit " + rayHit.collider.gameObject.name);
                if (rayHit.collider.gameObject.tag=="Player") {
                    if (state != "kill") {
                        state = "chase";
                        _nav.speed = 5.0f;
                        _anim.speed = 3.0f;
                        _roar.pitch = 1.2f;
                        _roar.Play();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _anim.SetFloat("velocity", _nav.velocity.magnitude);
        _anim.SetBool("alive", alive);
        if (alive)
        {
            if (state == "idle")
            {
                Vector3 RandomPos = Random.insideUnitCircle * alertness;
                NavMeshHit NavHit;
                NavMesh.SamplePosition(transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
                _nav.SetDestination(NavHit.position);
                state = "walk";
                if (highAlert)
                {
                    NavMesh.SamplePosition(_player.transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
                    alertness += 5f;
                    if (alertness > 20f)
                    {
                        highAlert = false;
                        alertness = 20f;
                        _nav.speed = 1.2f;
                        _anim.speed = 1.2f;
                    }
                }
            }
            if (state == "walk")
            {
                if (_nav.remainingDistance <= _nav.stoppingDistance && !_nav.pathPending)
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
                _nav.destination = _player.transform.position;

                //lose sight of player
                float distance = Vector3.Distance(transform.position, _player.transform.position);
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
                if (_nav.remainingDistance <= _nav.stoppingDistance && !_nav.pathPending)
                {
                    state = "search";
                    wait = 5f;
                    highAlert = true;
                    alertness = 5f;
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
     
	}
    public void TakeDamage()
    {
        Health--;
        if(Health<=0)alive = false;
        
    }
}
