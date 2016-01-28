using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Hitbox : MonoBehaviour {

    public string ownerTag;
    protected Transform trans;
    protected HashSet<Transform> hitObjects;

    public virtual void Start () {
        hitObjects = new HashSet<Transform>();
        this.gameObject.layer = LayerMask.NameToLayer(ownerTag + "Attacks");
        trans = GetComponent<Transform>();
    }

    void Update () {
	}
}
