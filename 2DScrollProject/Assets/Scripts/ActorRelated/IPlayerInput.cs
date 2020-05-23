using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 用来获取用户输入的信号
/// </summary>
public class IPlayerInput : MonoBehaviour
{
    [Header("----- Signals -----")]
    public float dUp;//Y轴
    public float dRight;//X轴
    public float dMagn;//移动的速度
    public Vector3 dVect;//移动的方向
    public bool walkLeft;//移动方向-用于动画机

    public bool jump;//跳跃信号
    public bool dash;//冲刺信号
    public bool defense;//防御信号
    public bool attack;//进攻信号

    [Header("----- Others -----")]
    public float dUpVelocity;
    public float dRightVelocity;
    public float regularDRight;
    public float regularDUp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void SetUpVirtualAxis()
    {

    }
}
