using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyArea : MonoBehaviour
{
    public Color SafetyColor;
    public int PeopleSaved;
    public bool TrySavePerson(Color personColor)
    {
        if(SafetyColor == personColor)
        {
            PeopleSaved++;
            return true;
        }
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PersonController person))
        {
            if (TrySavePerson(person.myColor))
                person.SavePerson(transform.position);
        }
    }
}
