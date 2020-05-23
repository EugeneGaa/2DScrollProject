using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timers
{
    public enum State
    {
        Idle,
        Running,
        Finished
    }

    public State state;
    public float durationTime;
    public float elapsedTime = 0f;
    

    public void Tick()//这个功能相当于一个状态机，是长时间执行判断的。Idle和finish其实没啥用，主要是靠进入running来执行
    {
        if(state==State.Idle)
        {

        }
        else if(state==State.Running)//进入running后，计时器开始累加。当累加数值大于设定值，就结束了running的state，切换到finish。
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime>=durationTime)
            {
                state = State.Finished;
            }
        }
        else if(state ==State.Finished)
        {

        }
        else
        {
            Debug.Log("出错了");
        }
    }

    public void StartTImer()//这个是将计时器归零，并通过转换为runningstate，开启计时器。
    {
        elapsedTime = 0f;
        state = State.Running;//这样就会触发tick...?
    }


}
