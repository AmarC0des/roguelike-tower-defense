using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    bool a;
    EnemySO enemySO;
    float angle;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(ChangeAnimation), 4, 4);
        InvokeRepeating(nameof(ChangeDirection), 1f, 1f);
        EnemySO stats = GetComponent<EnemyController>().stats;
        GetComponent<Animator2D>().animations[0] = stats.anim1;
        GetComponent<Animator2D>().animations[1] = stats.anim2;
        
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0,angle,0);
    }
    void ChangeAnimation()
    {
        a = !a;
        GetComponent<Animator2D>().SetAnimation(a?1:0);
    }
    void ChangeDirection()
    {
        angle += 90;
    }
}
