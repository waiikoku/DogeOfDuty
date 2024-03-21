using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class SlickUI
{
    public static void SetText(TextMeshProUGUI textComponent, string message)
    {
        textComponent.text = message;
    }
    public static void SetText(Text textComponent, string message)
    {
        textComponent.text = message;
    }

    public static void SetSliderValue(Slider slider, float value)
    {
        slider.value = value;
    }

    public static void SetFillAmount(Image image, float fillAmount)
    {
        image.fillAmount = fillAmount;
    }

    public static void SetToggleState(Toggle toggle, bool isOn)
    {
        toggle.isOn = isOn;
    }
}