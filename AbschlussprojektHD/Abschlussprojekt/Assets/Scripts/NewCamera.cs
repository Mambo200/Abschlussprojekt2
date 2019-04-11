using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour {

    private const float Y_ANGLE_MIN = -20f;
    private const float Y_ANGLE_MAX = 50f;

    public Transform lootAt;
    [HideInInspector]
    public Transform camTransform;

    private Camera m_Camera;

    private float distance = 1f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 4.0f;
    private float sensitivityY = 1.0f;


	// Use this for initialization
	private void Start ()
    {
        lootAt = this.GetComponentInParent<Transform>();
        camTransform = transform;
        m_Camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        currentX = Input.GetAxis("Mouse X");
        currentY = Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate ()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 nP = lootAt.position + rotation * dir;
        camTransform.position = nP;
        camTransform.LookAt(lootAt.position);
	}
}
