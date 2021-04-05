using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityBar : MonoBehaviour
{
    //when the sensitivity bar appears, it will grab the saved value
    public void OnEnable()
    {
        gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensitivity");
    }
}
