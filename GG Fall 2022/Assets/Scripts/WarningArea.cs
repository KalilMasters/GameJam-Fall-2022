using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningArea : MonoBehaviour
{
    public float Radius;
    public static WarningArea Instance;
    public LayerMask FloorMask;
    bool isActive;
    Renderer visual;
    Collider col;
    List<PersonController> activePersons = new List<PersonController>();
    void SetPersonActive(PersonController person, bool isActive)
    {
        person.ToggleRunningState(isActive);
    }
    void ClearAllActivePersons()
    {
        while (activePersons.Count > 0)
        {
            SetPersonActive(activePersons[0], false);
            activePersons.RemoveAt(0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PersonController>(out PersonController person))
        {
            SetPersonActive(person, true);
            activePersons.Add(person);
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PersonController>(out PersonController person))
        {
            SetPersonActive(person, false);
            activePersons.Remove(person);
        }
    }
    private void Update()
    {
        isActive = Input.GetMouseButton(0);
        Vector3 inWorldPosition = GetInWorldPosition();
        if (inWorldPosition == Vector3.zero)
            isActive = false;
        transform.localScale = Vector3.one * Radius * 2;
        visual.enabled = isActive;
        col.enabled = isActive;
        if (!isActive)
        {
            ClearAllActivePersons();
        }
        transform.position = inWorldPosition;
    }
    private Vector3 GetInWorldPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, FloorMask))
        {
            return hit.point + Vector3.up * 0.1f;
        }
        return Vector3.zero;
    }
    private void Awake()
    {
        visual = GetComponent<Renderer>();
        col = visual.GetComponent<Collider>();
        Instance = this;    
    }
}
