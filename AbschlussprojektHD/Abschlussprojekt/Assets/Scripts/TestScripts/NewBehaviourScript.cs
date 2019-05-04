using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    Animator animator;
    Animation animation;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animation = GetComponent<Animation>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
