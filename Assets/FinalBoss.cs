using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBoss : MonoBehaviour
{
    void OnDestroy()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
