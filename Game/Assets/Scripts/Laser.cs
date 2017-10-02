using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float updateFrequency = 0.1f;
    public int laserDistance;
    public string bounceTag;
    //public string splitTag;
    //public string spawnedBeamTag;
    public int maxBounce;
    //public int maxSplit;
    public GameObject unlockObject;
    private float timer = 0;
    private LineRenderer mLineRenderer;
    private bool unlocked;
    private bool laserOn;
    // Use this for initialization
    void Start()
    {
        timer = 0;
        bounceTag = "Bounce";
        //spawnedBeamTag = "Spawn";
        //splitTag = "Split";
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
            /*
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
            */
            mLineRenderer = gameObject.GetComponent<LineRenderer>();
            StartCoroutine(RedrawLaser());
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
        Vector3 lastLaserPosition = transform.localPosition + laserDirection * 0.5f; //origin of the next laser
        Vector3 hitNormal = new Vector3();
        Vector3 hitPoint = new Vector3();
        // mLineRenderer.SetVertexCount(1);
        mLineRenderer.positionCount = 1;
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit = new RaycastHit();
        bool isGOInWorldA = gameObject.layer == LayerMask.NameToLayer("WorldA") || gameObject.layer == LayerMask.NameToLayer("WorldBInPortal");
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        int portalLayer = 1 << LayerMask.NameToLayer("Portal");
        GameObject portal = null;
        if (playerGO != null)
        {
            portal = playerGO.GetComponent<PlayerStatus>()._currentPortal;
        }
        // Debug.Log(isInsidePortal ? "Inside portal" : "Outside portal");
        while (loopActive)
        {
            GameObject hitObject = null;
            int hitableLayers = -1;
            float restDistance = laserDistance;
            bool isHit = false;
            Vector3 startingPoint = lastLaserPosition;
            hitableLayers = -1;
            hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldB") : LayerMask.NameToLayer("WorldA")));
            

            bool isInsidePortalCurrently = false;
            if (portal != null)
            {
                float portalRadius = portal.GetComponent<PortalLogic>()._portalCurrentRadius;
                isInsidePortalCurrently = Vector3.SqrMagnitude(startingPoint - portal.transform.position) <= portalRadius * portalRadius;
            }
            // First test if it's out side portal, ray cast unitil hit portal or hit some object before portal
            if (!isInsidePortalCurrently) {
                hitableLayers = -1;
                hitableLayers ^= (1 << 2);
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldB") : LayerMask.NameToLayer("WorldA")));
                // shooting from outside and won't be blocked by object in the other world
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldBInPortal") : LayerMask.NameToLayer("WorldAInPortal")));
                isHit = Physics.Raycast(startingPoint, laserDirection, out hit, restDistance, hitableLayers);
                // Reach portal boundary, that means nothing blocking for now
                if (!isHit)
                {
                    // Debug.Log("out portal can't hit");
                }
                // The ray arrived outer rim of portal, so nothing is colliding now
                if (isHit) {
                    if (hit.transform.gameObject.GetComponent<PortalLogic>() != null)
                    {
                        // Debug.Log("out portal reach " + restDistance);
                        isHit = false;
                        restDistance -= hit.distance;
                        startingPoint = hit.point + laserDirection * 0.1f;
                    }
                    else {
                        hitObject = hit.transform.gameObject;
                        hitNormal = hit.normal;
                        hitPoint = hit.point;
                    }
                 //   Debug.DrawLine(hit.point + new Vector3(0, 1, 0), hit.point + laserDirection + new Vector3(0, 1, 0), Color.red, 4f);
                }
                
            }

            if (portal != null) {
                float portalRadius = portal.GetComponent<PortalLogic>()._portalCurrentRadius;
                isInsidePortalCurrently = Vector3.SqrMagnitude(startingPoint - portal.transform.position) <= portalRadius * portalRadius;
            }
            //// Inside portal
            if (!isHit && restDistance >= 0f && isInsidePortalCurrently)
            {
                // Find another point in portal, project from the other side
                hitableLayers = portalLayer;
                //Debug.DrawLine(lastLaserPosition + (laserDirection * laserDistance) + new Vector3(0, 1, 0), lastLaserPosition + new Vector3(0, 1, 0), Color.green, 4f);
                Vector3 longestPointInPortal;
                if (Physics.Raycast(lastLaserPosition + (laserDirection * laserDistance), -laserDirection, out hit, laserDistance, hitableLayers))
                {
                    longestPointInPortal = (Vector3.Distance(startingPoint, hit.point) > restDistance ? startingPoint + laserDirection * restDistance : hit.point);
                    // Debug.Log("hit portal? " + hit.transform.gameObject.name);
                }
                else {
                    // Debug.Log("This could happen when portal is in the bottom of laser");
                    longestPointInPortal = startingPoint + laserDirection * restDistance;
                }
                // Next test points inside the portal
                hitableLayers = -1;
                hitableLayers ^= portalLayer;
                hitableLayers ^= (1 << 2);
                // hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldB") : LayerMask.NameToLayer("WorldA")));
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldAInPortal") : LayerMask.NameToLayer("WorldBInPortal")));
                isHit = Physics.Raycast(startingPoint, laserDirection, out hit, Vector3.Distance(longestPointInPortal, startingPoint), hitableLayers);
                //if (isHit) {
                //    Debug.Log("hit something in portal with forward dir  " + hit.transform.gameObject);
                //}

                // Test backward
                // Debug.Log("testing backward");
                float testDistance = Vector3.Distance(longestPointInPortal, startingPoint);
                var hitResults = Physics.RaycastAll(longestPointInPortal, -laserDirection, testDistance, hitableLayers);
                // Debug.DrawLine(longestPointInPortal + new Vector3(0, 1, 0), longestPointInPortal + -laserDirection * Vector3.Distance(longestPointInPortal, startingPoint) + new Vector3(0, 1, 0), Color.black, 0.1f);

                // Reach something in the portal with forward direction
                if (hitResults.Length != 0)
                {
                    int bestIdx = 0;
                    float minBackDistanceSq = Vector3.SqrMagnitude(lastLaserPosition - hitResults[0].point);
                    for (int i = 1; i < hitResults.Length; ++i)
                    {
                        float currSqrDistance = Vector3.SqrMagnitude(lastLaserPosition - hitResults[i].point);
                        if (minBackDistanceSq > currSqrDistance)
                        {
                            bestIdx = i;
                            minBackDistanceSq = currSqrDistance;
                        }
                    }
                    // if(isHit)
                    // Debug.Log(Vector3.SqrMagnitude(hit.point - lastLaserPosition) + " " + minBackDistanceSq);
                    
                    
                    // Compare both backward hit and forward hit
                    // if forward hit result is better, use forward
                    if (isHit)
                    {
                        // Debug.DrawLine(hit.point + new Vector3(0, 0.5f, 0), hit.point - laserDirection + new Vector3(0, 0.5f, 0), Color.red, 0.1f);
                        // Debug.DrawLine(hitResults[bestIdx].point + new Vector3(0, 0.5f, 0), hitResults[bestIdx].point + laserDirection + new Vector3(0, 0.5f, 0), Color.blue, 0.1f);
                        float forwardDistanceSq = Vector3.SqrMagnitude(hit.point - lastLaserPosition);
                        if (forwardDistanceSq < minBackDistanceSq || Mathf.Abs(forwardDistanceSq - minBackDistanceSq) <= 0.01f)
                        {
                            // Debug.Log("both hit but forward is better  ");
                            restDistance -= hit.distance;
                            startingPoint = hit.point;
                            hitObject = hit.transform.gameObject;
                            hitNormal = hit.normal;
                            hitPoint = hit.point;
                        }
                        else
                        {
                            // Debug.Log("both hit but back is better  ");
                            isHit = false;
                        }
                    }
                    if(!isHit)
                    {
                        isHit = true;
                        hit = hitResults[bestIdx];
                        hitObject = hit.transform.gameObject;
                        hitNormal = hit.normal;
                        hitPoint = hit.point;
                        // Debug.Log("hit something in portal with back dir  " + hit.transform.gameObject);
                        startingPoint = hitResults[bestIdx].point;
                    }
                }
                else
                {
                    // Already Reach something in the portal with forward direction
                    if (isHit) {
                        // Debug.Log("hit something in portal with forward dir  " + hit.transform.gameObject);
                        // Debug.DrawLine(hit.point + new Vector3(0, 1, 0), hit.point + laserDirection + new Vector3(0, 1, 0), Color.blue, 4f);
                        restDistance -= hit.distance;
                        startingPoint = hit.point;
                        hitNormal = hit.normal;
                        hitPoint = hit.point;
                        hitObject = hit.transform.gameObject;
                    }
                    // Both forward and backward failed
                    else {
                        isHit = false;
                        startingPoint = longestPointInPortal;
                        restDistance -= testDistance;
                        // Debug.Log("in portal backward can't hit");
                    }
                }
            }
            if (!isHit && restDistance >= 0f) {
                hitableLayers = -1;
                hitableLayers ^= portalLayer;
                hitableLayers ^= (1 << 2);
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldB") : LayerMask.NameToLayer("WorldA")));
                // shooting from outside and won't be blocked by object in the other world
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldBInPortal") : LayerMask.NameToLayer("WorldAInPortal")));
                isHit = Physics.Raycast(startingPoint, laserDirection, out hit, restDistance, hitableLayers);
                if (isHit){
                    // Debug.Log("out hit something in portal with forwad dir  " + hit.transform.gameObject);
                    hitObject = hit.transform.gameObject;
                    hitNormal = hit.normal;
                    hitPoint = hit.point;
                }
            }
            
            //if (Physics.Raycast(startingPoint, laserDirection, out hit, laserDistance, hitableLayers) && ((hit.transform.gameObject.tag == bounceTag) || (hit.transform.gameObject.tag == splitTag)))
            if (isHit && (hitObject.transform.parent != null&& hitObject.transform.parent.tag == bounceTag) )
            {
                //Debug.Log("Bounce");
                laserReflected++;
                vertexCounter += 3;
                // mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.positionCount = vertexCounter;
                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hitPoint, lastLaserPosition, 0.01f));
                mLineRenderer.SetPosition(vertexCounter - 2, hitPoint);
                mLineRenderer.SetPosition(vertexCounter - 1, hitPoint);
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hitNormal);
                lastLaserPosition = hitPoint + laserDirection * 0.1f;
                /*
                if (hitObject.tag == splitTag)
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
                */

            }
            else
            {
                //Debug.Log("No Bounce");
                laserReflected++;
                vertexCounter++;
                // mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.positionCount = vertexCounter;
                // Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * hit.distance);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                // if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == unlockTag)) && unlocked ==false)
                if (isHit && ((hitObject == unlockObject)) && unlocked == false)
                {
                    Debug.Log("Unlocked");
                    unlocked = true;
                    hit.transform.SendMessage("TriggerEvent");
                }
                // if ((Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance)))
                if (isHit)
                {
                    // mLineRenderer.SetPosition(vertexCounter - 1, lastPos);
                    mLineRenderer.SetPosition(vertexCounter - 1, hitPoint);
                }
                else
                {
                    // Nothing is hit
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
