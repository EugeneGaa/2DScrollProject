using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons
{
    public bool isPressing = false;
    public bool onPress = false;
    public bool onRelease = false;
    private bool currentState = false;
    private bool lastState = false;

    public bool isExtending = false;//是否在计时延长段中
    public bool isDelaying = false;
    public float extendingDuration = 0.18f;
    public float delayingDuration = 0.18f;

    public Timers extTimer =new Timers();
    public Timers delayTimer = new Timers();



    public void PressFunction(bool input)
    {
        extTimer.Tick();//常时调用timer里面的功能，因为在playerinput里面是update里面执行的
        delayTimer.Tick();
        currentState = input;//按下按钮就是true，不按就是false；
        isPressing = currentState;//onbutton，也就是和按下保持一致就行了

        onPress = false;//初始化一次性的按钮
        onRelease = false;
        isDelaying = false;
        isExtending = false;

        if(currentState !=lastState)//如果不相等，就继续推进。换而言之，onPress和onRelease都是长时间false的。但只有一瞬间，currentstate！=lastState的那一帧，这个函数会推进，从而实现一次true，相当于trigger
        {
            if(currentState==true)
            {
                onPress = true;
                StartTimer(delayTimer, delayingDuration);
            }
            else
            {
                onRelease = true;
                StartTimer(extTimer, extendingDuration);//在松开的时候，启动延长段计时
            }

        }
        lastState = currentState;//这一部分是为了保持在currentstate不变的情况下保持laststate和currentstate的一致，从而阻止推进。唯一改变的方法就是玩家的输入变动。会出现一帧的变动，从而可以实现推进一次。

        if(extTimer.state==Timers.State.Running)//在running段中，extending信号持续有效。当timer结束，信号变为负数。这也是一个常时判断的功能。他的常时性来源于joystick对button的持续引用，即update
        {
            isExtending = true;
        }
        if(delayTimer.state==Timers.State.Running)
        {
            isDelaying = true;
        }


    }

    private void StartTimer(Timers timer, float durationTime)//这个功能是用来设置延长段的时间，并且启动timer-》详见timer的starttime
    {
        timer.durationTime = durationTime;
        timer.StartTImer();
    }

}
