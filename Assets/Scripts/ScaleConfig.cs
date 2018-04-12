using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleConfig : MonoBehaviour {

    public GameObject UpperBound;
    public GameObject UpperMiddleBound;
    public GameObject MiddleBound;
    public GameObject LowerMiddleBound;
    public GameObject LowerBound;
    public GameObject Title;


    public void SetScaleValues(float maxStress, float minStress)
    {
        float conversion = (float)1e09;
        // float toMPA = (float)1e06;

        float maxStressCon = maxStress / conversion;
        float minStressCon = minStress / conversion;

        float range = maxStressCon - minStressCon;

        float UBound = (maxStressCon);
        float UMBound = (float)(minStressCon + (0.75 * range));
        float MBound = (float)(minStressCon + (0.5 * range));
        float LMBound = (float)(minStressCon + (0.25 * range));
        float LBound = (minStressCon);

        String UBoundStr = UBound.ToString("0.00");
        String UMBoundStr = UMBound.ToString("0.00");
        String MBoundStr = MBound.ToString("0.00");
        String LMBoundStr = LMBound.ToString("0.00");
        String LBoundStr = LBound.ToString("0.00");

        UpperBound.GetComponent<TextMesh>().text = ("- " + UBoundStr);
        UpperMiddleBound.GetComponent<TextMesh>().text = ("- " + UMBoundStr);
        MiddleBound.GetComponent<TextMesh>().text = ("- " + MBoundStr); 
        LowerMiddleBound.GetComponent<TextMesh>().text = ("- " + LMBoundStr);
        LowerBound.GetComponent<TextMesh>().text = ("- " +LBoundStr);

        Title.GetComponent<TextMesh>().text = ("Stress (GPa)");
    }
}
