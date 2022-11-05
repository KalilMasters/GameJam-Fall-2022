using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PersonAIValues : ScriptableObject
{
    public float ChangeDirectionDistance;
    public float MoveDirectionDistance;
    public float IdleTime;
    public float WarningCheckRadius;
    public float TimeBeforeIdle;
    public float AISpeed;
}
