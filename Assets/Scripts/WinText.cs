using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WinText : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
    void Update()
    {
        if(Time.frameCount % 10 == 0)
            GetComponent<TMP_Text>().color = Random.ColorHSV(0,1,1,1,1,1);
    }
}
