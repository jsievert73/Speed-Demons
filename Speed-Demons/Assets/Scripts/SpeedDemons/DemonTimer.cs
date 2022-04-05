using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonTimer : MonoBehaviour
{
    public GameObject DemonParent;
    private EnemyController thisDemon;
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
        CurrentFill = maxFill;
        thisDemon = DemonParent.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        thisDemon.currentSpeed += Time.deltaTime * 0.25f;
        CurrentFill -= Time.deltaTime * 0.25f;
        if(thisDemon.finished)
        {
            thisDemon.UpdateHealth();
            DemonParent.SetActive(false);
        }
    }
}
