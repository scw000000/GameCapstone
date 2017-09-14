using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensFlareControl : MonoBehaviour {
    public float _lensFlareScale;
    private float Size;
    private LensFlare Flare;
    void Start()
    {
        if (Flare == null)
            Flare = GetComponent<LensFlare>();

        if (Flare == null)
        {
            Debug.LogError("No LensFlare on " + name + ", destroying.", this);
            Destroy(this);
            return;
        }

        Size = Flare.brightness;
    }

    void Update()
    {
        float ratio = Mathf.Sqrt(Vector3.Distance(transform.position, Camera.main.transform.position));
        Flare.brightness = _lensFlareScale * Size / ratio;
    }
}
