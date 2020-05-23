using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject hero;//主角
    public HeroControl hc;//主角的控制组件
    public StateManager sm;//主角的状态组件

    [Header("----- Related Data -----")]
    public float followSpeed = 0.02f;
    public float minCamPosX = -10000;
    public float maxCamPosX = 10000;
    public float stillOffset = 2;

    public float smoothDampVelocity;


    private void Awake()
    {
        //获取主角的游戏物体
        hero = GameObject.FindGameObjectWithTag("hero");
        hc = hero.GetComponent<HeroControl>();

    }


    void Start()
    {
        sm = hc.sm;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        AdjustCameraPos();
    }

    public void AdjustCameraPos()
    {
        float pPosX = hero.transform.position.x;//主角 x轴方向 时实坐标值
        float cPosX = transform.position.x;//相机 x轴方向 时实坐标值
        if (Mathf.Abs(hc.rigid.velocity.x) > 0.05f)//主角在移动
        {
            if (Mathf.Abs(pPosX - cPosX) > stillOffset)
            {
                //transform.position = new Vector3(Mathf.Lerp(cPosX, pPosX, 0.04f), transform.position.y, transform.position.z);
                transform.position = new Vector3(Mathf.SmoothDamp(cPosX, pPosX, ref smoothDampVelocity,0.8f), transform.position.y, transform.position.z);

            }
            float realPosX = Mathf.Clamp(transform.position.x, minCamPosX, maxCamPosX);//相机X轴方向 限制移动区间，防止超过背景边界
            transform.position = new Vector3(realPosX, transform.position.y, transform.position.z);
        }
        else if (Mathf.Abs(hc.rigid.velocity.x) < 0.05f && Mathf.Abs(hc.rigid.velocity.y) < 0.05)//静止
        {

            if (sm.isFacingRight)//面向右边
            {
                transform.position = new Vector3(Mathf.Lerp(cPosX, pPosX + stillOffset, followSpeed), transform.position.y, transform.position.z);
            }
            else//面向左边
            {
                transform.position = new Vector3(Mathf.Lerp(cPosX, pPosX - stillOffset, followSpeed), transform.position.y, transform.position.z);
            }

        }

    }
}
