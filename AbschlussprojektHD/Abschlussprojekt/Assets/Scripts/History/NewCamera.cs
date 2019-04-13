using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour {

    private const float Y_ANGLE_MIN = -20f;
    private const float Y_ANGLE_MAX = 50f;

    [SerializeField]
    private GameObject m_target;
    public float rotateSpeed;

    private Vector3 offset;

    private void Start()
    {
        offset = m_target.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        m_target.transform.Rotate(0, horizontal, 0);

        float desiredAngle = m_target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = m_target.transform.position - (rotation * offset);

        transform.LookAt(m_target.transform);
    }
}
