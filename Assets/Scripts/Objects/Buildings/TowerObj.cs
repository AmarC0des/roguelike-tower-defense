using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TowerObj : MonoBehaviour
{
    public TowerSO towerInfo;
    public Transform shootPoint; //custom place to shoot pjs from.
    void Start()
    {
        InvokeRepeating(nameof(ShootProjectile), Random.Range(0f, 2f), towerInfo.shootRateSecs); //random range helps shooting to look non-synchronous with other towers.
    }
    void ShootProjectile()
    {
        if (!gameObject.activeSelf) return; //dont shoot when placing!
        Transform target = GetNearestEnemyPosition();
        if (target == null) return; // dont shoot if no enemies nearby!

        TowerProjectile projectile = Instantiate(towerInfo.projectile, shootPoint.position, shootPoint.rotation).GetComponent<TowerProjectile>(); //create pj

        projectile.target = target; //tell pj where to go
    }

    //returns Vec3.zero to use as a "cant find anything, dont shoot!"
    Transform GetNearestEnemyPosition()
    {
        var everythingInRange = Physics.OverlapSphere(transform.position, towerInfo.aimRadius);
        foreach (var item in everythingInRange)
        {
            //may rework to check an actual enemy script or something, no enemy script has been made at this time.
            if (item.TryGetComponent(out CinemachineDollyCart enemy))
            {
                return item.transform; //predict enemy movement so it actually hits
            }
        }
        return null;
    }
}
