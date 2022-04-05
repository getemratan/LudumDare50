using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = default;
    [SerializeField] private float fastSpeed = default;
    [SerializeField] private float rotAmount = default;
    [SerializeField] private float movementTime = default;
    [SerializeField] private Transform camTransform = default;
    [SerializeField] private Vector3 zoomAmount = default;
    [SerializeField] private Vector2 panLimit = default;

    private float movementSpeed;
    private Vector3 newPos;
    private Quaternion newRot;
    private Vector3 newZoom;

    private void Awake()
    {
        newPos = transform.position;
        newRot = transform.rotation;
        newZoom = camTransform.localPosition;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            newPos += transform.forward * movementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPos += transform.forward * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPos += transform.right * -movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPos += transform.right * movementSpeed;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
            newZoom = new Vector3(0, Mathf.Clamp(newZoom.y, 10, 60), Mathf.Clamp(newZoom.z, -100f, -20));
        }

        newPos.x = Mathf.Clamp(newPos.x, -panLimit.x, panLimit.x);
        newPos.z = Mathf.Clamp(newPos.z, -panLimit.y, panLimit.y);

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * movementTime);

        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
}