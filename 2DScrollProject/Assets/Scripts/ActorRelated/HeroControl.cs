using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : MonoBehaviour
{
    [Header("----- Components -----")]
    public GameObject hero;//获取主角的游戏物件
    public IPlayerInput pi;//获取输入组件，从中获得玩家输入信号
    public Rigidbody rigid;//主角刚体
    public Animator anim;//主角动画机
    public StateManager sm;//主角的状态管理组件
    public BattleManager bm;//主角的战斗管理组件
    public Collider col;

    [Header("----- Other Data -----")]
    public float moveSpeed = 3;//移动速度修正参数
    public float jumpForce = 5;
    public float dashForce = 5;

    private Vector3 movingVect;//主角移动向量，用以修正刚体速度
    [HideInInspector]
    public Vector3 thrustVelocity;//冲刺补正向量

    [HideInInspector]
    public bool canHit = true;
    public bool inputEnable = true;
    private Vector3 tempDashDirection;




    void Awake()
    {
        Initialization();
    }

    void Start()
    {

    }


    void Update()
    {
        if (inputEnable)
        {
            movingVect = Mathf.Abs(pi.dMagn) * moveSpeed * pi.dVect;//实时通过接收信号，更新主角移动向量
        }

        SignalsProcessor(anim);
        StateProcessor();

    }

    private void FixedUpdate()
    {
        SetMoveSpeed();
    }

    /// <summary>
    /// 在游戏开始前加载/绑定各组件，完成初始化
    /// </summary>
    private void Initialization()
    {
        hero = this.gameObject;
        pi = this.GetComponent<IPlayerInput>();
        rigid = this.GetComponent<Rigidbody>();
        sm = this.GetComponentInChildren<StateManager>();
        sm.hc = this;//双向绑定两个脚本
        bm = this.GetComponentInChildren<BattleManager>();
        bm.hc = this;
        anim = this.GetComponent<Animator>();
        col = this.GetComponent<CapsuleCollider>();
    }




    /// <summary>
    /// 通过实时调整刚体的速度实现移动
    /// </summary>
    private void SetMoveSpeed()
    {
        rigid.velocity = new Vector3(movingVect.x, rigid.velocity.y, rigid.velocity.z)+thrustVelocity;//实时改变刚体速度
        thrustVelocity = Vector3.zero;
    }




    /// <summary>
    /// 用来判断动画状态机是否处于某一状态上
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="layerName"></param>
    /// <returns></returns>
    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }




    /// <summary>
    /// 动画事件呼叫专用，用来消除多次触发动画机trigger的bug
    /// </summary>
    /// <param name="triggerName"></param>
    public void ClearSignals(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }




    /// <summary>
    /// 对玩家输入的信号作出处理
    /// </summary>
    /// <param name="_anim"></param>
    public void SignalsProcessor(Animator _anim)
    {
        //移动速度
        _anim.SetFloat("DMagn", pi.dMagn);

        //下降速度
        _anim.SetFloat("FallSpeed", rigid.velocity.y);

        //攻击
        if(pi.attack)
        {
            _anim.SetTrigger("Attack");
        }
        //防御
        _anim.SetBool("Defense", pi.defense);
        //跳跃
        if (pi.jump)
        {
            _anim.SetTrigger("Jump");
        }
        //冲刺
        if(pi.dash)
        {
            _anim.SetTrigger("Dash");
        }

        

    }




    /// <summary>
    /// 对StateManager返回的信号作出对应的补充处理
    /// </summary>
    public void StateProcessor()
    {
        //控制转向
        if (sm.isFacingRight)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!sm.isFacingRight)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if(sm.hitOccurred==true&&canHit)
        {
            if(sm.isDefense||sm.isDefenseHit)//如果在防御，就防御受击
            {
                anim.SetTrigger("DefenseHit");
            }
            else if(sm.isDefenseHit==false&&sm.isDefense==false)
            {
                anim.SetTrigger("Hit");
                //伤害处理
            }
            canHit = false;
        }
        anim.SetBool("IsGround", sm.isGround);//更新isGround状态
    }




    #region 动画机呼叫的信息的处理中心

    public void OnIdleEnter()
    {
        rigid.velocity = new Vector3(movingVect.x, 0, 0);
    }

    public void OnWalkEnter()
    {
        rigid.velocity = new Vector3(movingVect.x, 0, 0);
    }

    public void OnJumpEnter()
    {
        thrustVelocity = new Vector3(0, jumpForce , 0);
    }

    public void OnDashUpdate()
    {
        //thrustVelocity = new Vector3(Mathf.Lerp(0,tempDashDirection.x*dashForce,0.1f),Mathf.Lerp(0,tempDashDirection.y*dashForce,0.1f), 0);
        thrustVelocity = new Vector3(Mathf.Lerp(0, (sm.isFacingRight ? dashForce : -dashForce), 0.1f), 0, 0);//三元运算符判定冲量方向
    }

    public void OnDashEnter()
    {
        //与冲刺方向相关
        //Vector3 temp1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 temp2 = new Vector3(temp1.x, temp1.y, 0);
        //tempDashDirection = (temp2 - this.transform.position).normalized;
        inputEnable = false;
    }
    public void OnDashExit()
    {
        inputEnable = true;
    }

    public void AttackColOn()
    {
        bm.atkCol.enabled = true;
    }
    public void AttackColOff()
    {
        bm.atkCol.enabled = false;
    }

    public void DefenseColOn()
    {
        sm.defCol.enabled = true;
        sm.physicsCol.enabled = true;
    }

    public void DefenseColOff()
    {
        sm.defCol.enabled = false;
        sm.physicsCol.enabled = false;
    }
    #endregion

}
