using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Slider FPS_Slider;
    public TextMeshProUGUI FPS_Counter;

    public void UpdateFPSCounter(float value)
    {
        FPS_Counter.text = value.ToString();
        Animator.current.SetAnimDelay(value);
    }

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

    public void FramePerFrame(int d)
    {
        Animator.current.ManualAnimate(d);
    }
}
