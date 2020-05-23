using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : IPlayerInput
{
    [Header("===== Key Settings =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyRight = "d";
    public string keyLeft = "a";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    void Start()
    {
        
    }


    void Update()
    {
        SetUpVirtualAxis();
        GiveMovementSignals();
    }

    /// <summary>
    /// 搭建一个虚拟轴，并获取移动向量，输出移动信号（方向与基础速度）
    /// </summary>
    public override void SetUpVirtualAxis()
    {
        regularDUp = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);//用三元表达式搭建虚拟轴
        regularDRight = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        dUp = Mathf.SmoothDamp(dUp, regularDUp, ref dUpVelocity, 0.1f);//添加一个平滑的惯性效果
        dRight = Mathf.SmoothDamp(dRight, regularDRight, ref dRightVelocity, 0.1f);

        //制作成速度与方向两个信号输出
        dMagn = dRight;
        dVect = dRight * Vector3.right;

    }

    /// <summary>
    /// 输出行为信号
    /// </summary>
    public void GiveMovementSignals()
    {
        jump = Input.GetKeyDown(keyA);//这里用buttons可以组出二级信号，但暂时没有必要好像
        dash = Input.GetKeyDown(keyB);
        defense = Input.GetMouseButton(1);//右键防御
        attack = Input.GetMouseButtonDown(0);//左键进攻
    }
}
