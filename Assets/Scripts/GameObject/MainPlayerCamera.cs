using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家摄像机
/// </summary>
public class MainPlayerCamera : MonoSingleton<MainPlayerCamera>
{
    /// <summary>
    /// 摄像机
    /// </summary>
    public Camera camera;

    public Transform viewPoint;
    /// <summary>
    /// 玩家
    /// </summary>
    public GameObject player;
    /// <summary>
    /// 跟随速度
    /// </summary>
    public float followSpeed = 5f;
    /// <summary>
    /// 转向速度
    /// </summary>
    public float rotateSpeed = 5f;

    Quaternion yaw = Quaternion.identity;
	

    private void LateUpdate()
    {
        if (player == null && User.Instance.CurrentCharacterObject != null)
        {
            player = User.Instance.CurrentCharacterObject.gameObject;
        }

        if (player == null)
            return;

        this.transform.position = Vector3.Lerp(this.transform.position, player.transform.position, this.followSpeed * Time.deltaTime);
        if (Input.GetMouseButton(1))
        {
            Vector3 angleBase = this.transform.localEulerAngles;
            this.transform.localRotation = Quaternion.Euler(angleBase.x - Input.GetAxis("Mouse Y") * this.rotateSpeed, angleBase.y + Input.GetAxis("Mouse X") * this.rotateSpeed, 0);
            Vector3 angle = this.transform.rotation.eulerAngles - player.transform.rotation.eulerAngles;
            angle.z = 0;
            yaw = Quaternion.Euler(angle);
        }
        else
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, player.transform.rotation * yaw, this.followSpeed * Time.deltaTime);
        }
        if(Input.GetAxis("Vertical") > 0.01)
        {
            //走动时，自由视角后可以回位
            yaw = Quaternion.Lerp(yaw, Quaternion.identity, this.followSpeed * Time.deltaTime);
        }
    }
}
