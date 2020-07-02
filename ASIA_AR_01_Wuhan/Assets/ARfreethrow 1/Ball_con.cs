using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(Rigidbody))]
public class Ball_con : MonoBehaviour
{
    public float m_ThrowForce = 100f;

    public float m_ThrowDirectionX = 0.17f;
    public float m_ThrowDirectionY = 0.67f;

    public Vector3 m_BallCameraOffest = new Vector3(0f, -1.4f, 3f);

    private Vector3 StartPosition;
    private Vector3 ThrowDirection;
    private Vector3 direction;
    private float startTime;
    private float endTime;
    private float duration;
    private bool directionChosen = false;
    private bool throwStarted = false;

    [SerializeField]
    GameObject ARCam;

    [SerializeField]
    ARSessionOrigin m_SessionOrigin;

    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        m_SessionOrigin = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        ARCam = m_SessionOrigin.transform.Find("AR Camera").gameObject;
        ResetBall();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartPosition = Input.mousePosition;
            startTime = Time.time;
            throwStarted = true;
            directionChosen = false;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endTime = Time.time;
            duration = endTime - startTime;
            direction = Input.mousePosition - StartPosition;
            directionChosen = true;
        }

        if(directionChosen)
        {
            rb.mass = 1;
            rb.useGravity = true;

            rb.AddForce(
                ARCam.transform.forward * m_ThrowForce / duration +
                ARCam.transform.up *direction.y *m_ThrowDirectionY +
                ARCam.transform.right *direction.x *m_ThrowDirectionX);

            startTime = 0.0f;
            duration = 0.0f;

            StartPosition = new Vector3(0, 0, 0);
            direction = new Vector3(0, 0, 0);

            throwStarted = false;
            directionChosen = false;
        }

        if (Time.time - endTime >= 5 && Time.time - endTime <= 6)
            ResetBall();
    }
    public void ResetBall()
    {
        rb.mass = 0;
        rb.useGravity = false;
        rb.angularVelocity = Vector3.zero;
        endTime = 0.0f;

        Vector3 ballPos = ARCam.transform.position+ ARCam.transform.forward *m_BallCameraOffest .z+ ARCam.transform.up* m_BallCameraOffest.y;
        transform.position = ballPos;
    }
}
