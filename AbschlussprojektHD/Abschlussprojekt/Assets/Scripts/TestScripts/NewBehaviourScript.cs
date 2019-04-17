using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(this.transform.position, transform.forward, Color.blue);
	}
}
