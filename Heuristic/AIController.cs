using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private Circuit circuit;
    
    private CarDrive carController;
    private Vector3 target;
    private int currentWP = 0;
    private Rigidbody rb;
    private GameObject tracker;
    private int currentTrackerWP = 0;
    private float lookAhead = 15;
    private float acceleration = 0.0f;
    private float steer = 0.0f;
    private float trackerSpeed = 15.0f;
    private AvoidDetector avoid;

    void Start()
    {
        carController = GetComponent<CarDrive>();
        target = circuit.waypoints[currentWP].transform.position;
        rb = this.GetComponent<Rigidbody>();

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());

        //Comment this line for tracker debugging
        tracker.GetComponent<MeshRenderer>().enabled = false;
        
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;
        avoid = this.GetComponent<AvoidDetector>();
    }

    void Update()
    {
        ProgressTracker();
        target = tracker.transform.position;

        Vector3 localTarget;
        if (Time.time < avoid.avoidTime)
            localTarget = tracker.transform.right * avoid.avoidPath;
        else
            localTarget = this.transform.InverseTransformPoint(target);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        steer = Mathf.Clamp(targetAngle, -1, 1) * Mathf.Sign(rb.velocity.magnitude);
        acceleration = 1;

        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerFactor = corner / 90.0f;

        //If corner is not too steep we just deaccelerate
        if(corner >20 && corner < 40 && rb.velocity.magnitude >10)
            acceleration = Mathf.Lerp(0, 1, 1 - cornerFactor);
        //If corner is too steep we brake
        else if (corner >=40 && rb.velocity.magnitude > 10)
		{
            acceleration = -1;
		}
        //If we need to reverse we assign -1 to acceleration and invert the steer
		if (avoid.reverse)
		{
            acceleration = -1;
            steer = -1 * steer;
		}

    }

	private void FixedUpdate()
	{
        carController.ProcessInputs(acceleration, steer);
	}

	private void ProgressTracker()
	{
        Debug.DrawLine(this.transform.position, tracker.transform.position);

        if (Vector3.Distance(this.transform.position, tracker.transform.position) > lookAhead)
		{
            trackerSpeed -= 1.0f;
            if (trackerSpeed < 2) trackerSpeed = 2;
            return;
		}
        if (Vector3.Distance(this.transform.position, tracker.transform.position) < lookAhead/2)
		{
            trackerSpeed += 1.0f;
            if (trackerSpeed > 2) trackerSpeed = 15;
        }

        tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, trackerSpeed * Time.deltaTime);

        if(Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTrackerWP].transform.position) < 1)
		{
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.waypoints.Length)
                currentTrackerWP = 0;
		}
	}
}
