using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class PersonAIValues : ScriptableObject
{
    public float ChangeDirectionDistance;
    public float IdleTime;
    public float TimeBeforeIdle;
    public float AIWalkSpeed, AIRunSpeed;
}
