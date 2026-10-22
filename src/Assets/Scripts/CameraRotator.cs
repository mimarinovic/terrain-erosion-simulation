using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public Transform Target;
    public float speed = 30f;
    public float radius = 50f;
    public float height = 20f;
    private float angle = 0f;
    private bool isDragging = false;
    private Vector3 meshCenter;
    private float verticalOffset = 0f;
    public float verticalSpeed = 10f;
    public float verticalMin = -30f;
    public float verticalMax = 50f;
    private Vector2 lookOffset = Vector2.zero;
    public float lookMoveSpeed = 2f;
    private bool isLooking = false;
    private Vector2 lastMousePos;

    void Start()
    {
        if (Target == null)
        {
            Debug.LogWarning("CameraRotator: No target assigned!");
            enabled = false;
            return;
        }
        meshCenter = Target.position;
        MeshRenderer mr = Target.GetComponentInChildren<MeshRenderer>();
        if (mr != null)
            meshCenter = mr.bounds.center;
        angle = Mathf.Atan2(transform.position.x - meshCenter.x, transform.position.z - meshCenter.z);
    }

    void Update()
    {
        meshCenter = Target.position;
        MeshRenderer mr = Target.GetComponentInChildren<MeshRenderer>();
        if (mr != null)
            meshCenter = mr.bounds.center;

        if (Input.GetMouseButtonDown(1))
        {
            isLooking = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isLooking = false;
        }
        if (isLooking)
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - lastMousePos;
            lookOffset.x += mouseDelta.x * lookMoveSpeed * 0.01f;
            lookOffset.y -= mouseDelta.y * lookMoveSpeed * 0.01f;
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            angle += mouseX * speed * 0.002f;
            verticalOffset -= mouseY * verticalSpeed * 0.02f;
            verticalOffset = Mathf.Clamp(verticalOffset, verticalMin, verticalMax);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            float zoomFactor = 1f - scroll * 0.2f;
            radius *= zoomFactor;
            height *= zoomFactor;
            radius = Mathf.Clamp(radius, 5f, 500f);
            height = Mathf.Clamp(height, 2f, 200f);
        }

        float x = meshCenter.x + Mathf.Sin(angle) * radius;
        float z = meshCenter.z + Mathf.Cos(angle) * radius;
        float y = meshCenter.y + height + verticalOffset;
        transform.position = new Vector3(x, y, z);
        
        Vector3 lookAt = meshCenter + new Vector3(lookOffset.x, lookOffset.y, 0);
        transform.LookAt(lookAt);
    }
}
