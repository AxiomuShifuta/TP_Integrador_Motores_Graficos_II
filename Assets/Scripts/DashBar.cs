using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public void FillDashBar(float dashRechargeTime)
    {
        barImage.fillAmount = 0;
        barImage.fillAmount = Mathf.Lerp(0,1,dashRechargeTime);
    }
}
