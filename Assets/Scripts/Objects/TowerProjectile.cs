using System;
using Cinemachine;
using UnityEngine;
public class TowerProjectile : MonoBehaviour
{
    [NonSerialized] public Transform target;
    [SerializeField] float speed;
    [SerializeField] GameObject particle;
    [SerializeField] int damage;
    [SerializeField] float sizeRadius;

    void Start()
    {
        Invoke(nameof(Die), 3f); //auto die if we miss.
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        if ((transform.position - target.transform.position).magnitude < sizeRadius)
        {
            //Todo: Damage Enemy
            target.GetComponent<EnemyController>().TakeDamage(damage);
            Die();
        }
    }
    void Die()
    {
        if (particle) Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject); //done
    }
}