using UnityEngine;
using System.Collections;

public class ParallaxManager : MonoBehaviour {

    public FreeParallax parallax;
    public Camera mainCamera;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (parallax != null)
        {
            parallax.Speed = -1 * mainCamera.velocity.x;
        }
    }
}
