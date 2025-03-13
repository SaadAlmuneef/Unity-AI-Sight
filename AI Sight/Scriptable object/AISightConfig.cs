using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AISightConfig", menuName = "AI/SightConfig")]
public class AISightConfig : ScriptableObject
{
    [Range(0,360)] public  float viewAngle = 90f;
    [Range(0,1000)] public float viewRadius = 10f;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    [Range(0,180)] public float AgingTime = 6;
}
