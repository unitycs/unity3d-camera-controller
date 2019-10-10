using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public float sensitivityMouse = 2f;
    public Transform target;
    //观察距离  
    public float Distance = 5F;

    //旋转角度  
    private float mX = 0.0F;
    private float mY = 0.0F;
    //角度限制  
    private float MinLimitY = 5;
    private float MaxLimitY = 180;
    //是否启用差值  
    public bool isNeedDamping = true;
    //速度  
    public float Damping = 2.5F;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
       
    }

    void LateUpdate()
    {      
        //获取鼠标输入  
        mX += Input.GetAxis("Mouse X") * sensitivityMouse * 0.02F;
        mY -= Input.GetAxis("Mouse Y") * sensitivityMouse * 0.02F;
        //范围限制  
        mY = ClampAngle(mY, MinLimitY, MaxLimitY);

        //重新计算位置和角度  
        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        Vector3 mPosition = mRotation * new Vector3(0.0F, 2.0F, -Distance) + target.position;

        //设置相机的角度和位置  
        if (isNeedDamping)
        {
            //球形插值  
            transform.rotation = Quaternion.Slerp(transform.rotation, mRotation, Time.deltaTime * Damping);
            //线性插值  
            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            transform.rotation = mRotation;
            transform.position = mPosition;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
