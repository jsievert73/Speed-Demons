using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonTimer : MonoBehaviour
{
    public GameObject DemonParent;
    public Image radialFill;
    private float maxFill = 2f;
    private float minFill = 0f;
    private float currentValue = 2f;
    public float CurrentFill {
        get {
            return currentValue;
        }
        set {
            currentValue = Mathf.Clamp(value, minFill, maxFill);
            float fillPercentage = currentValue/maxFill;
            radialFill.fillAmount = fillPercentage;
            if(currentValue <= 0f)
            {
                DemonParent.SetActive(false);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentFill = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentFill -= 0.0032f;
    }
}
