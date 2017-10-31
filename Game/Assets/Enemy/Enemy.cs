using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private NavMeshAgent _nav;
    public Transform _eyes;
    public AudioSource _roar;
    public GameObject _key;
    private Animator _anim;

    public int Health = 4;
    private string state = "idle";
    private bool alive = true;
    private float wait = 0f;
    private float chargeWait = 0f;
    private float alertness = 20.0f;
    private float deathTime = 3f;
    private Vector3 chargingDirection;

    // Use this for initialization
    void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

        _nav.speed = 5.0f;
        _anim.speed = 1.2f;
    }

    //check the player
    public void checkSight()
    {
        if (alive && state!="chase")
        {
            RaycastHit rayHit;
            if (Physics.Linecast(_eyes.position, _player.transform.position, out rayHit))
            {
                print("hit " + rayHit.collider.gameObject.name);
                if (rayHit.collider.gameObject.tag == "Player")
                {
                    state = "chase";

                    _nav.speed = 15.0f;
                    _anim.speed = 3.0f;
                    _roar.pitch = 1.2f;
                    _roar.Play();
                    chargeWait = 2.0f;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _anim.SetFloat("velocity", _nav.velocity.magnitude);
        _anim.SetBool("alive", alive);
        if (alive)
        {
            if (state == "idle")
            {
                Vector3 RandomPos = Random.insideUnitCircle * alertness;
                NavMeshHit NavHit;
                NavMesh.SamplePosition(_player.transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
                _nav.SetDestination(NavHit.position);
                state = "walk";
            }
            if (state == "walk")
            {
                if (_nav.remainingDistance <= 1f && !_nav.pathPending)
                {
                    _nav.speed = 5.0f;
                    _anim.speed = 1.2f;
                    state = "search";
                    wait = 2f;
                }
            }
            if (state == "search")
            {
                _anim.speed = 1.2f;
                if (wait > 2f)
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
                if (chargeWait > 0f)
                {
                    _nav.destination = _player.transform.position;
                    chargeWait -= Time.deltaTime;
                }
                else {
                    _nav.speed= 5.0f;
                    _anim.speed = 1.2f;
                    state = "search";
                    wait = 2f;
                }
            }
        }
        else
        {
            _nav.Stop();
            if (deathTime > 0f)
            {
                deathTime -= Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
                _key.SetActive(true);
            }
        }

    }
    public void TakeDamage()
    {
        Health--;
        if (Health <= 0) alive = false;

        Vector3 RandomPos = Random.insideUnitCircle * alertness;
        NavMeshHit NavHit;
        NavMesh.SamplePosition(_player.transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
        _nav.SetDestination(NavHit.position);
        state = "walk";
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "IceWall")
        {
            this.TakeDamage();
        }
    }
}
