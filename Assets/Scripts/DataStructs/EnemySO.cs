/* Enemy Scriptable Object Class
 * -----------------------------
 * This class is used to create a data container for 
 * enemies. It holds the stats and functions 
 * that are needed for the enemies to work in-game.
 * 
 * 
 * Modified 2/6/25:
 * -Basic set up for the class.
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "ScriptableObjects/EnemyScriptableObject")]
public class EnemySO : ScriptableObject
{
    public int atk; 
}
