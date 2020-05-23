using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("----- Components -----")]
    public HeroControl hc;
    public StateManager sm;
    public Collider atkCol;

    [Header("----- Battle Related Data -----")]
    public float damageDeal;

    private void Awake()
    {
        atkCol = this.GetComponent<BoxCollider>();
        atkCol.enabled = false;//关闭攻击触发器，通过动画事件来控制其开关。打开即视为展开攻击
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //trigger处理攻击信号
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.position.x>this.transform.position.x)//怪物在右边
        {
            if(sm.isFacingRight)
            {
                Debug.Log("attackSuccess"+other.name);
                //攻击成功，曝露攻击信号
            }
            else
            {
                //攻击失败，不做处理
            }
        }
        else if(other.transform.position.x < this.transform.position.x)//怪物在左边
        {
            if (sm.isFacingRight)
            {
                //攻击失败，不做处理
            }
            else
            {
                Debug.Log("attackSuccess" + other.name);
                //攻击成功，曝露攻击信号
            }
        }
    }

    //collider处理防御信号
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
