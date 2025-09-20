using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Transform[] wheelMeshes;
    public WheelCollider[] wheelColliders;

    public int rotateSpeed;
    public int rotateAngle;
    public int wheelRotateSpeed;

    private int targetRotation;

    private void LateUpdate()
    {
        for (int i = 0; i < wheelMeshes.Length; i++)
        {
            Quaternion quat;
            Vector3 pos;
            wheelColliders[i].GetWorldPose(out pos, out quat);

            wheelMeshes[i].position = pos;

            wheelMeshes[i].Rotate(Vector3.right * (Time.deltaTime * wheelRotateSpeed));
        }

        if (Input.GetMouseButton(0) || Input.GetAxis("Horizontal") != 0)
        {
            UpdateTargetRotation();
        }
        else if (targetRotation != 0)
        {
            targetRotation = 0;
        }

        Vector3 rotation = new Vector3(transform.localEulerAngles.x, targetRotation, transform.localEulerAngles.z);
        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotation), rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// update car rotation
    /// </summary>
    void UpdateTargetRotation()
    {
        if (Input.GetAxis("Horizontal") == 0)
        {
            if (Input.mousePosition.x > Screen.width * 0.5)
            {
                // turn right
                targetRotation = rotateAngle;
            }
            else
            {
                // turn left
                targetRotation = -rotateAngle;
            }
        }
        else
        {
            // if get input with ad or <- ->
            targetRotation = (int)(rotateAngle * Input.GetAxis("Horizontal"));
        }
    }
}
