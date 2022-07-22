using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Slider FPS_Slider;
    public TextMeshProUGUI FPS_Counter;

    //when fps slider is changed call this and update UI element and update animation delay
    public void UpdateFPSCounter(float value)
    {
        FPS_Counter.text = value.ToString();
        Animator.current.SetAnimDelay(value);
    }

    //pause/continue animator, triggered by toggle box
    public void PauseAnim(bool paused)
    {
        if (paused)
        {
            Animator.current.SetAnimDelay(0);
        }
        else
        {
            Animator.current.SetAnimDelay(FPS_Slider.value);
        }
    }

    //function thats called by button, showed when paused is on.
    public void FramePerFrame(int d)
    {
        Animator.current.ManualAnimate(d);
    }
}
