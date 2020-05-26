using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来收集并更新玩家状态的类
/// </summary>
public class StateManager : MonoBehaviour
{
    [Header("----- Components -----")]
    public HeroControl hc;
    public BattleManager bm;
    public Collider defCol;//保持常开，闪现关闭
    public Collider physicsCol;//保持常开，闪现关闭


    [Header("----- First Order State Flags -----")]
    public bool isJump;
    public bool isGround;
    public bool isWall;
    public bool isDash;
    public bool isHit;
    public bool isAttack;
    public bool isDefense;
    public bool isFall;
    public bool isFacingRight=true;

    [Header("----- Second Order State Flags -----")]
    public bool isDefenseHit;
    public bool canDealDamage;

    [Header("----- Others -----")]
    public bool hitOccurred;
    public bool canHit = true;

    private void Awake()
    {

    }
    void Start()
    {
        //和battlemanager双向绑定
        bm = hc.bm;
        bm.sm = this;
        defCol = this.GetComponent<CapsuleCollider>();
        physicsCol = this.GetComponent<BoxCollider>();
    }


    void Update()
    {
        ChangeDirection();
        StateUpdate();
    }

    /// <summary>
    /// 用以更新一级旗标
    /// </summary>
    public void StateUpdate()
    {
        UpdateIsGroundState();
        UpdateIsWallState();
        isJump = hc.CheckState("jump");
        isDefense = hc.CheckState("defense");
        isDefenseHit = hc.CheckState("isDefenseHit");
        isAttack = hc.CheckState("attack");
        isHit = hc.CheckState("hit");
        isFall = hc.CheckState("fall");
        isDash = hc.CheckState("dash");
    }

    /// <summary>
    /// 根据移动输入，实时调整角色的面对方向，输出方向的状态
    /// </summary>
    public void ChangeDirection()
    {
        if(hc.inputEnable)
        {
            if (hc.pi.regularDRight > 0)
            {
                isFacingRight = true;
            }
            else if (hc.pi.regularDRight < 0)
            {
                isFacingRight = false;
            }
        }
    }

    /// <summary>
    /// 用以更新isGround状态的补充方法
    /// </summary>
    public void UpdateIsGroundState()
    {
        isGround = Physics.Raycast(this.transform.position, new Vector3(0, -1, 0), 0.6f, LayerMask.GetMask("ground"));
    }

    public void UpdateIsWallState()
    {
        isWall = Physics.Raycast(this.transform.position, new Vector3(-1, 0, 0), 1f, LayerMask.GetMask("ground"))|| Physics.Raycast(this.transform.position, new Vector3(1, 0, 0), 1f, LayerMask.GetMask("ground"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9&&canHit)
        {
            hitOccurred = true;//为了把受击的信号曝露到HC，由HC作判断
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            hitOccurred = false;
            hc.canHit = true;//HC的canHit保护一下，避免连续受击
        }
    }
}
