/* Tower Scriptable Object Class
 * -----------------------------
 * This class is used to create a data container for 
 * towers in the game. It holds the stats and functions 
 * that are needed for the towers to work in-game.
 * 
 * 
 * Modified 2/6/25:
 * -Basic set up for the class.
 * 
 * 
 * Modified 4/24/25
 * - added entries
 */




using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTower", menuName = "ScriptableObjects/TowerScriptableObject")]
[Serializable]
public class TowerSO : ScriptableObject
{
    public GameObject tower;
    public float aimRadius;
    public float shootRateSecs;
    public GameObject projectile;

    //omitting effects for time
}
