using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyController ec))
        {
            PlayerController playerController = GetComponentInParent<PlayerController>();
            ec.TakeDamage(GameManager.Instance.power);
        }
    }
}
