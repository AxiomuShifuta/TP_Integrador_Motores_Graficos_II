using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    [SerializeField] private Image barImage;

    public IEnumerator FillDashBar(float dashRechargeTime)
    {
        barImage.fillAmount = 0;
        float elapsedTime = 0f;

        while (elapsedTime < dashRechargeTime)
        {
            barImage.fillAmount = Mathf.Lerp(0, 1, elapsedTime / dashRechargeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
