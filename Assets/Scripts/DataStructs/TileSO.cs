using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewTile", menuName = "ScriptableObjects/TileScriptableObject")]
public class TileSO : ScriptableObject {

    public Vector3 startPos;
    public Vector3 endPos;
    public GameObject tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
