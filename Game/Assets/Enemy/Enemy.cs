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
    public AudioSource _hurt;
    public AudioSource _death;
    public GameObject _key;
    private Animator _anim;
    private Rigidbody _rb;

    public int Health;

    private string state = "idle";
    private bool alive = true;
    private float wait = 0f;
    private float chargeWait = 0f;
    private float chargeTime = 0f;
    private float alertness = 20.0f;
    private float deathTime = 3f;
    private Vector3 chargingPosition;

    // Use this for initialization
    void Start()
    {
        _nav = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

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
                    chargingPosition = rayHit.collider.gameObject.transform.position - transform.position;
                    chargingPosition.Normalize();

                    _anim.speed = 3.0f;
                    _roar.pitch = 1.2f;
                    _roar.Play();
                    chargeWait = 1.0f;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _anim.SetBool("alive", alive);
        if (alive)
        {
            if (state == "idle")
            {
                Vector3 RandomPos = Random.insideUnitCircle * alertness;
                NavMeshHit NavHit;
                NavMesh.SamplePosition(_player.transform.position + RandomPos, out NavHit, 20f, NavMesh.AllAreas);
                _nav.SetDestination(NavHit.position);
                _nav.Resume();
                state = "walk";
            }
            if (state == "walk")
            {
                _anim.SetBool("charging", true);

                if (_nav.remainingDistance <= 1f && !_nav.pathPending)
                {
                    _nav.speed = 3.0f;
                    _anim.speed = 1.2f;
                    state = "search";
                    _anim.SetBool("charging", false);
                    wait = 2f;
                }
            }
            if (state == "search")
            {
                _anim.speed = 1.2f;

                _rb.velocity = new Vector3(0,0,0);
                if (wait > 0f)
                {
                    wait -= Time.deltaTime;
                    transform.Rotate(0f, 30f * Time.deltaTime, 0f);
                }
                else
                {
                    _rb.angularVelocity = new Vector3(0,0,0);
                    state = "idle";
                }
            }
            if (state == "chase")
            {
                _anim.SetBool("charging",true);
                if (chargeWait > 0f)
                {
                    _nav.Stop();
                    transform.LookAt(transform.position+chargingPosition);
                    chargeWait -= Time.deltaTime;
                    chargeTime = 0.5f;
                }
                else {
                    if (chargeTime > 0f)
                    {
                        chargeTime -= Time.deltaTime;
                        _rb.AddForce(chargingPosition * 5f, ForceMode.VelocityChange);
                    }
                    else
                    {
                        _anim.SetBool("charging", false);
                        state = "search";
                        wait = 2f;
                    }
                }
            }
        }
        else
        {
            _nav.Stop();
            _rb.velocity = new Vector3(0, 0, 0);
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
        if (Health <= 0) {
            _death.Play();
            alive = false;
        }

        _hurt.Play();

        _anim.SetBool("charging", false);
        state = "search";
        wait = 2f;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "IceWall" && state == "chase")
        {
            this.TakeDamage();
        }
    }

}
