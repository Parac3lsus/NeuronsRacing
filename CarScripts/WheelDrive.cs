using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDrive : MonoBehaviour
{
    public bool canTurn = false;
    [SerializeField]
    private GameObject wheelMesh;

    private WheelCollider WC;


    void Start()
    {
        WC = this.GetComponent<WheelCollider>();
    }

    public void Move(float accel)
    {
        WC.motorTorque = accel;

    }
    public void Turn(float steer)
	{
            WC.steerAngle = steer;
	}

    public void WheelBrake(float brakeForce)
	{
        WC.brakeTorque = brakeForce;
	}

    public void ClearTorque()
	{
        WC.motorTorque = 0.0f;
        WC.brakeTorque = 0.0f;
	}

    public float GetSteerAngle()
	{
        return WC.steerAngle;
	}

    public void UpdateMeshPosition()
	{
        Quaternion quat;
        Vector3 position;
        WC.GetWorldPose(out position, out quat);

        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
	}
}
