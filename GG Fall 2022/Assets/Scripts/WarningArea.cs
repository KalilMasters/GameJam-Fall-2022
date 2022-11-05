using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningArea : MonoBehaviour
{
    public float Radius;
    public LayerMask FloorMask;
    bool isActive;
    Renderer visual;
    Collider col;
    private void Update()
    {
        isActive = Input.GetMouseButton(0);
        Vector3 inWorldPosition = GetInWorldPosition();
        if (inWorldPosition == Vector3.zero)
            isActive = false;
        transform.localScale = Vector3.one * Radius * 2;
        visual.enabled = isActive;
        col.enabled = isActive;
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
    }
}
