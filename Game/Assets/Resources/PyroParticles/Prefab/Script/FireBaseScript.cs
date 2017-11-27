using UnityEngine;
using System.Collections;

namespace DigitalRuby.PyroParticles
{
    [System.Serializable]
    public struct RangeOfIntegers
    {
        public int Minimum;
        public int Maximum;
    }

    [System.Serializable]
    public struct RangeOfFloats
    {
        public float Minimum;
        public float Maximum;
    }

    public class FireBaseScript : MonoBehaviour
    {
        [Tooltip("Optional audio source to play once when the script starts.")]
        public AudioSource AudioSource;

        [Tooltip("How long the script takes to fully start. This is used to fade in animations and sounds, etc.")]
        public float StartTime = 1.0f;

        [Tooltip("How long the script takes to fully stop. This is used to fade out animations and sounds, etc.")]
        public float StopTime = 3.0f;

        [Tooltip("How long the effect lasts. Once the duration ends, the script lives for StopTime and then the object is destroyed.")]
        public float Duration = 2.0f;
        private float StoreDuration;

        [Tooltip("How much force to create at the center (explosion), 0 for none.")]
        public float ForceAmount;

        [Tooltip("The radius of the force, 0 for none.")]
        public float ForceRadius;

        [Tooltip("A hint to users of the script that your object is a projectile and is meant to be shot out from a person or trap, etc.")]
        public bool IsProjectile;

        [Tooltip("Particle systems that must be manually started and will not be played on start.")]
        public ParticleSystem[] ManualParticleSystems;

        [Tooltip("How long the effect pauses inbetween playing.")]
        public float Pause = 0f;
        private float StorePause;
        [Tooltip("Ice to be melted")]
        public GameObject Ice;

        private bool Activate = true;
        private bool Check = true;

        private float startTimeMultiplier;
        private float startTimeIncrement;

        private float stopTimeMultiplier;
        private float stopTimeIncrement;

        private GameObject Player;
        private bool InFire;
        private bool Invincible;
        private float InvincibleTime;
        private FlamethrowerTrigger FlamethrowerTriggerComp;
        /*private IEnumerator CleanupEverythingCoRoutine()
        {
            // 2 extra seconds just to make sure animation and graphics have finished ending
            yield return new WaitForSeconds(StopTime + 2.0f);

            GameObject.Destroy(gameObject);
        }*/

        private void StartParticleSystems()
        {
            foreach (ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                if (ManualParticleSystems == null || ManualParticleSystems.Length == 0 ||
                    System.Array.IndexOf(ManualParticleSystems, p) < 0)
                {
                    
                    p.Play();
                }
            }
        }

        protected virtual void Awake()
        {
            Starting = true;
            Invincible = false;
            InvincibleTime = .8f;
            int fireLayer = UnityEngine.LayerMask.NameToLayer("FireLayer");
            //UnityEngine.Physics.IgnoreLayerCollision(fireLayer, fireLayer);
        }

        protected virtual void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            FlamethrowerTriggerComp = transform.parent.GetChild(0).GetChild(0).gameObject.GetComponent<FlamethrowerTrigger>();
            StoreDuration = Duration;
            StorePause = Pause;
            /*if (AudioSource != null)
            {
                AudioSource.Play();
            }
            StoreDuration = Duration;
            // precalculate so we can multiply instead of divide every frame
            stopTimeMultiplier = 1.0f / StopTime;
            startTimeMultiplier = 1.0f / StartTime;

            // if this effect has an explosion force, apply that now
            CreateExplosion(gameObject.transform.position, ForceRadius, ForceAmount);

            // start any particle system that is not in the list of manual start particle systems
            StartParticleSystems();*/

            // If we implement the ICollisionHandler interface, see if any of the children are forwarding
            // collision events. If they are, hook into them.
            ICollisionHandler handler = (this as ICollisionHandler);
            if (handler != null)
            {
                FireCollisionForwardScript collisionForwarder = GetComponentInChildren<FireCollisionForwardScript>();
                if (collisionForwarder != null)
                {
                    collisionForwarder.CollisionHandler = handler;
                }
            }
        }

        protected virtual void Update()
        {
            if (StartTime < 0)
            {
                StartCoroutine(FireControl());
                bool goInWorldA = gameObject.layer == LayerMask.NameToLayer("WorldA");
                bool playerInWorldA = (Player.layer == LayerMask.NameToLayer("WorldA") && !Player.GetComponent<WorldSwitch>()._insidePortal)
                    || (Player.layer == LayerMask.NameToLayer("WorldB") && Player.GetComponent<WorldSwitch>()._insidePortal);
                if (Check == true && FlamethrowerTriggerComp._playerInFire == true && Invincible == false && goInWorldA == playerInWorldA)
                {
                    if (Player != null)
                    {
                        Player.GetComponent<PlayerStatus>().AddHitPoints(-20f);
                        Invincible = true;
                    }
                }

                if (Invincible == true)
                {
                    StartCoroutine(InvincibleTimer());
                }
                if (Ice.gameObject != null)
                {
                    if (gameObject.layer == Ice.layer
                        || (gameObject.layer == LayerMask.NameToLayer("WorldA") && Ice.layer == LayerMask.NameToLayer("WorldBInPortal"))
                        || (gameObject.layer == LayerMask.NameToLayer("WorldB") && Ice.layer == LayerMask.NameToLayer("WorldAInPortal"))
                        || (gameObject.layer == LayerMask.NameToLayer("WorldBInPortal") && Ice.layer == LayerMask.NameToLayer("WorldA"))
                        || (gameObject.layer == LayerMask.NameToLayer("WorldAInPortal") && Ice.layer == LayerMask.NameToLayer("WorldB")))

                    {
                        Ice.GetComponent<IceBlockLogic>().Melt();
                        if (gameObject.layer == 10)
                        {
                            
                        }
                    }
                }

            }
            else
            {
                StartTime -= Time.deltaTime;
            }
            /*// reduce the duration
            Duration -= Time.deltaTime;
            if (Stopping)
            {
                // increase the stop time
                stopTimeIncrement += Time.deltaTime;
                if (stopTimeIncrement < StopTime)
                {
                    StopPercent = stopTimeIncrement * stopTimeMultiplier;
                }
            }
            else if (Starting)
            {
                // increase the start time
                startTimeIncrement += Time.deltaTime;
                if (startTimeIncrement < StartTime)
                {
                    StartPercent = startTimeIncrement * startTimeMultiplier;
                }
                else
                {
                    Starting = false;
                }
            }
            else if (Duration <= 0.0f)
            {
                // time to stop, no duration left
                Stop();
            }*/
        }
        IEnumerator FireControl()
        {
            // reduce the duration
            if (Check == true)
            {
                StoreDuration -= Time.deltaTime;
            }
            else if (Check == false)
            {
                StorePause -= Time.deltaTime;
            }
            if (Activate == true)
            {
                if (AudioSource != null)
                {
                    AudioSource.Play();
                }
                // precalculate so we can multiply instead of divide every frame
                stopTimeMultiplier = 1.0f / StopTime;
                startTimeMultiplier = 1.0f / StartTime;

                // if this effect has an explosion force, apply that now
                CreateExplosion(gameObject.transform.position, ForceRadius, ForceAmount);

                // start any particle system that is not in the list of manual start particle systems
                StartParticleSystems();
                Check = true;
                Activate = false;
            }
            else if (StoreDuration <= 0.0f)
            {
                // time to stop, no duration left
                Stop();
                StoreDuration = Duration;
                Check = false;
            }
            else if(StorePause <= 0.0f)
            {
                StorePause = Pause;
                Activate = true;
                Stopping = false;
            }
            yield return new WaitForEndOfFrame();
        }
       
        public static void CreateExplosion(Vector3 pos, float radius, float force)
        {
            if (force <= 0.0f || radius <= 0.0f)
            {
                return;
            }

            // find all colliders and add explosive force
            Collider[] objects = UnityEngine.Physics.OverlapSphere(pos, radius);
            foreach (Collider h in objects)
            {
                Rigidbody r = h.GetComponent<Rigidbody>();
                if (r != null)
                {
                    r.AddExplosionForce(force, pos, radius);
                }
            }
        }

        //so Player doesn't get Instant Killed
        IEnumerator InvincibleTimer()
        {
            InvincibleTime -= Time.deltaTime;
            if (InvincibleTime <= 0)
            {
                Invincible = false;
                InvincibleTime = .8f;
            }
            yield return new WaitForEndOfFrame();
        }

        public virtual void Stop()
        {
            if (Stopping)
            {
                return;
            }
            Stopping = true;

            // cleanup particle systems
            foreach (ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>())
            {
                p.Stop();
            }

            //StartCoroutine(CleanupEverythingCoRoutine());
        }

        
        public bool Starting
        {
            get;
            private set;
        }

        public float StartPercent
        {
            get;
            private set;
        }

        public bool Stopping
        {
            get;
            private set;
        }

        public float StopPercent
        {
            get;
            private set;
        }
    }
}