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

        // mLineRenderer.SetVertexCount(1);
        mLineRenderer.positionCount = 1;
        mLineRenderer.SetPosition(0, transform.position);
        RaycastHit hit = new RaycastHit();
        bool isGOInWorldA = gameObject.layer == LayerMask.NameToLayer("WorldA");
        var playerGO = GameObject.FindGameObjectWithTag("Player");
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
            int portalLayer = LayerMask.NameToLayer("Portal");
            hitableLayers ^= (1 << 14);
            hitableLayers ^= (1 << 2);
            hitableLayers ^= (1 << LayerMask.NameToLayer("WorldAInPortal"));
            hitableLayers ^= (1 << LayerMask.NameToLayer("WorldBInPortal"));
            //Vector3 idealNextPoint = lastLaserPosition + (laserDirection * restDistance);
            //isHit = Physics.Raycast(startingPoint, laserDirection, out hit, restDistance, hitableLayers);
            //if (isHit) {
            //    idealNextPoint = hit.point;
            //}

            bool isInsidePortalCurrently = false;
            if (portal != null)
            {
                float portalRadius = portal.GetComponent<PortalLogic>()._portalCurrentRadius;
                isInsidePortalCurrently = Vector3.SqrMagnitude(startingPoint - portal.transform.position) <= portalRadius * portalRadius;
            }
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
                hitableLayers = (1 << 14);
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
                
                hitableLayers = -1;
                hitableLayers ^= (1 << 14);
                hitableLayers ^= (1 << 2);
                // hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldB") : LayerMask.NameToLayer("WorldA")));
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldAInPortal") : LayerMask.NameToLayer("WorldBInPortal")));
                isHit = Physics.Raycast(startingPoint, laserDirection, out hit, Vector3.Distance(longestPointInPortal, startingPoint), hitableLayers);
                // Debug.DrawLine(startingPoint + new Vector3(0, 1, 0), longestPointInPortal + new Vector3(0, 1, 0), Color.green, 4f);
                // restDistance -= hit.distance;
                // startingPoint = hit.point;
                // Forward hit, but will still test backward anyway
                // hitObject = hit.transform.gameObject;

                // Test backward
                // Debug.Log("testing backward");
                float testDistance = Vector3.Distance(longestPointInPortal, startingPoint);
                var hitResults = Physics.RaycastAll(longestPointInPortal, -laserDirection, testDistance, hitableLayers);
                //  Debug.DrawLine(longestPointInPortal + new Vector3(0, 1, 0), longestPointInPortal + -laserDirection * Vector3.Distance(longestPointInPortal, startingPoint) + new Vector3(0, 1, 0), Color.black, 4f);

                // Reach something in the portal with forward direction
                if (hitResults.Length != 0)
                {
                    int bestIdx = 0;
                    float minSqrDistance = Vector3.SqrMagnitude(startingPoint - hitResults[0].point);
                    for (int i = 1; i < hitResults.Length; ++i)
                    {
                        float currSqrDistance = Vector3.SqrMagnitude(startingPoint - hitResults[i].point);
                        if (minSqrDistance > currSqrDistance)
                        {
                            bestIdx = i;
                            minSqrDistance = currSqrDistance;
                        }
                    }
                    // Compare both backward hit and forward hit
                    // if forward hit result is better, use forward
                    if (isHit && Vector3.SqrMagnitude(hit.transform.position - lastLaserPosition) < minSqrDistance)
                    {
                        restDistance -= hit.distance;
                        startingPoint = hit.point;
                        hitObject = hit.transform.gameObject;
                    }
                    else
                    {
                        isHit = true;
                        hit = hitResults[bestIdx];
                        hitObject = hit.transform.gameObject;
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
                        hitObject = hit.transform.gameObject;
                    }
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
                hitableLayers ^= (1 << 14);
                hitableLayers ^= (1 << 2);
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldB") : LayerMask.NameToLayer("WorldA")));
                // shooting from outside and won't be blocked by object in the other world
                hitableLayers ^= (1 << (isGOInWorldA ? LayerMask.NameToLayer("WorldBInPortal") : LayerMask.NameToLayer("WorldAInPortal")));
                isHit = Physics.Raycast(startingPoint, laserDirection, out hit, restDistance, hitableLayers);
                if (isHit){
                    hitObject = hit.transform.gameObject;
                }
            }
            
            //if (Physics.Raycast(startingPoint, laserDirection, out hit, laserDistance, hitableLayers) && ((hit.transform.gameObject.tag == bounceTag) || (hit.transform.gameObject.tag == splitTag)))
            if (isHit && ((hitObject.tag == bounceTag) || (hitObject.gameObject.tag == splitTag)))
            {
                //Debug.Log("Bounce");
                laserReflected++;
                vertexCounter += 3;
                // mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.positionCount = vertexCounter;
                mLineRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, lastLaserPosition, 0.01f));
                mLineRenderer.SetPosition(vertexCounter - 2, hit.point);
                mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
                lastLaserPosition = hit.point;
                Vector3 prevDirection = laserDirection;
                laserDirection = Vector3.Reflect(laserDirection, hit.normal);

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

            }
            else
            {
                //Debug.Log("No Bounce");
                laserReflected++;
                vertexCounter++;
                // mLineRenderer.SetVertexCount(vertexCounter);
                mLineRenderer.positionCount = vertexCounter;
                Vector3 lastPos = lastLaserPosition + (laserDirection.normalized * hit.distance);
                //Debug.Log("InitialPos " + lastLaserPosition + " Last Pos" + lastPos);
                // if (Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance) && ((hit.transform.gameObject.tag == unlockTag)) && unlocked ==false)
                if (isHit && ((hitObject.tag == unlockTag)) && unlocked == false)
                {
                    Debug.Log("Unlocked");
                    unlocked = true;
                    hit.transform.SendMessage("TriggerEvent");
                }
                // if ((Physics.Raycast(lastLaserPosition, laserDirection, out hit, laserDistance)))
                if (isHit)
                {
                    // mLineRenderer.SetPosition(vertexCounter - 1, lastPos);
                    mLineRenderer.SetPosition(vertexCounter - 1, hit.point);
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
