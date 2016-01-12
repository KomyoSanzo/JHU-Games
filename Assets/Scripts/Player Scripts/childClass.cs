using UnityEngine;
using System.Collections;

public class childClass : parentClass {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Test()
    {
        print("test2");
    }
}
