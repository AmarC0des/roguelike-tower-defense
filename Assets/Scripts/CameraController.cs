using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will eventually be absorbed by GameManager or PlayerController
public class CameraContoller : MonoBehaviour
{
    public GameObject topView;
    public GameObject frontView;

    private void Start()
    {
        ChangeToFrontView();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeToTopView();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeToFrontView();
        }
        
    }

    void ChangeToTopView()
    {
       topView.SetActive(true);
       frontView.SetActive(false);

    }

    void ChangeToFrontView()
    {
        topView.SetActive(false);
        frontView.SetActive(true);

    }

}
