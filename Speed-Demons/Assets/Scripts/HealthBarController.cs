using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider healthBar;
    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    public void UpdateHP(int health)
    {
        healthBar.value = health; 
    }
}
