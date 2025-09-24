using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform camTarget;
    public float height = 5f;
    public float rotationDamping = 1f;
    public float heightDamping = 0.5f;
    public float distance = 6f;

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (camTarget == null)
        {
            return;
        }

        // 取一些值, 将要旋转和定位的值
        float targetRotationAngle = camTarget.eulerAngles.y;
        float targetHeight = camTarget.position.y + height;
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // 第一步, 把摄像机位置移动到被观察者
        transform.position = camTarget.position;

        // 第二步, 在被观察者基础上往后偏移
        transform.position -= currentRotation * Vector3.forward * distance;

        // 重置相机高度
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // 摄像机看向被观察物体
        transform.LookAt(camTarget);
    }

}
