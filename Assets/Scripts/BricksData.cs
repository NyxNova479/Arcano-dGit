using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BricksCfg", menuName = "Arkanoïd/BricksConfiguration")]

public class BricksData : ScriptableObject
{
    [System.Serializable]
    public class BrickType
    {
        public string name;
        public int points;
        public int hitToBreak;
        public Color color;
        public GameObject prefab;
        public bool isTranslucid;
    }

    public List<BrickType> bricksTypes;

}
