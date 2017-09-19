using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    public string splitTag;
    public string spawnedBeamTag;
    public string unlockTag;
    public int maxBounce;
    public int maxSplit;
    private float timer = 0;
    private LineRenderer mLineRenderer;
    private bool unlocked;
    private bool laserOn;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        bounceTag = "Bounce";
        spawnedBeamTag = "Spawn";
        splitTag = "Split";
        unlockTag = "Unlock";
        mLineRenderer = gameObject.GetComponent<LineRenderer>();
        mLineRenderer.enabled = false;
        StartCoroutine(RedrawLaser());
        mLineRenderer.startWidth = .25f;
        mLineRenderer.endWidth = .25f;
        unlocked = false;
        laserOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (laserOn)
        {
            if (gameObject.tag != spawnedBeamTag)
            {
                if (timer >= updateFrequency)
                {
                    timer = 0;
                    //Debug.Log("Redrawing laser");
                    foreach (GameObject laserSplit in GameObject.FindGameObjectsWithTag(spawnedBeamTag))
                        Destroy(laserSplit);

                    StartCoroutine(RedrawLaser());
                }
                timer += Time.deltaTime;
            }
            else
            {
                mLineRenderer = gameObject.GetComponent<LineRenderer>();
                StartCoroutine(RedrawLaser());
            }
        }
    }

    void LaserOn()
    {
        laserOn = true;
        mLineRenderer.enabled = laserOn;
    }

    IEnumerator RedrawLaser()
    {
        //Debug.Log("Running");
        int laserSplit = 1; //How many times it got split
        int laserReflected = 1; //How many times it got reflected
        int vertexCounter = 1; //How many line segments are there
        bool loopActive = true; //Is the reflecting loop active?

        Vector3 laserDirection = transform.forward; //direction of the next laser
        Vector3 lastLaserPosition = transform.localPosition; //origin of the next laser

        mLineRenderer.SetVertexCount(1);
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;

        while (loopActive)
        {
            //Debug.Log("Physics.Raycast(" + lastLaserPosition + ", " + laserDirection + ", out hit , " + laserDistance + ")");
            int reflectableLayers = -1;
            reflectableLayers = ~( 1 << 2);
            if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance, reflectableLayers) && ((hit.transform.gameObject.tag == bounceTag) || (hit.transform.gameObject.tag == splitTag)))
            {
                //Debug.Log("Bounce");
                laserReflected++;
                vertexCounter += 3;
                mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                lastLaserPosition = hit.point;
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);

                if (hit.transform.gameObject.tag == splitTag)
                {
                    //Debug.Log("Split");
                    if (laserSplit >= maxSplit)
                    {
                        Debug.Log("Max split reached.");
                    }
                    else
                    {
                        //Debug.Log("Splitting...");
                        laserSplit++;
                        Object go = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
                        go.name = spawnedBeamTag;
                        ((GameObject)go).tag = spawnedBeamTag;
                    }
                }

            }
            else
            {
                //Debug.Log("No Bounce");
                laserReflected++;
                vertexCounter++;
                mLineRenderer.SetVertexCount(vertexCounter);
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * hit.distance);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == unlockTag)) && unlocked ==false)
                {
                    Debug.Log("Unlocked");
                    unlocked = true;
                    hit.transform.SendMessage("TriggerEvent");
                }
                if ((Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance)))
                {
                    mLineRenderer.SetPosition(vertexCounter - 1, lastPos);
                }
                else
                {
                    mLineRenderer.SetPosition(vertexCounter-1, lastLaserPosition + (laserDirection.normalized * laserDistance));
                }

                    loopActive = false;
            }
            if (laserReflected > maxBounce)
                loopActive = false;
        }

        yield return new WaitForEndOfFrame();
    }
}
