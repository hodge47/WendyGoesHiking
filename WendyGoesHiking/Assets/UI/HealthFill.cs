using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthFill : MonoBehaviour
{
    public Slider slider;

    public void maxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth (int health)
    {
        slider.value = health;
    }
}
