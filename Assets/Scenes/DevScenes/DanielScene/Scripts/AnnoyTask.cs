using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnnoyTask", menuName = "GMTK2021/AnnoyTask")]
public class AnnoyTask : ScriptableObject
{

    public string Name;
    public float Level;
    public int rageLevel;
    public int playerId;
    public int enemyPlayerId;
    public AudioClip clip;

}
