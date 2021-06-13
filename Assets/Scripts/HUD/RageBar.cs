using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        this.slider.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRage(int rage)
    {
        this.slider.value = rage;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public float GetRage()
    {
        return this.slider.value;
    }

    public void AddRage(int rage)
    {
        this.SetRage((int)Mathf.Round(this.slider.value) + rage);
    }

    public void SetMaxRage (int rage)
    {
        fill.color = gradient.Evaluate(1f);
        slider.maxValue = rage;
    }
}
