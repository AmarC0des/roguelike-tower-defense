using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    public Transform topView;
    public Transform frontView;

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
        this.gameObject.transform.position = topView.position;
        this.gameObject.transform.rotation = topView.rotation;

    }

    void ChangeToFrontView()
    {
        this.gameObject.transform.position = frontView.position;
        this.gameObject.transform.rotation = frontView.rotation;
   
    }

}
