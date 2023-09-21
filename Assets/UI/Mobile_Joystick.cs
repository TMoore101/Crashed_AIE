using JetBrains.Annotations;
using SimplicitySuite.FirstPersonController;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mobile_Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystick;
    public Transform joystickHandle;

    private bool isMoving;
    public float maxHandleDistance = 200;
    public float deadZone = 50;

    public float rotationSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cam;

    private Vector2 touchStartPos;
    private bool isRotating;
    private Quaternion initialRotation;


    public SS_FP_CharacterController characterController;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(joystick, eventData.position))
            {
                isMoving = true;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            joystickHandle.localPosition = Vector3.zero;
            characterController.joystickX = 0;
            characterController.joystickZ = 0;
            isMoving = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isMoving)
            {
                float posX = eventData.position.x - joystick.position.x;
                float posY = eventData.position.y - joystick.position.y;
                Vector2 pos = new Vector2(posX, posY);
                Vector3 clampedDelta = Vector2.ClampMagnitude(pos, maxHandleDistance);
                joystickHandle.position = joystick.position + clampedDelta;

                if (clampedDelta.x > deadZone || clampedDelta.x < -deadZone)
                    characterController.joystickX = (clampedDelta.x);
                else
                    characterController.joystickX = 0;
                if (clampedDelta.y > deadZone || clampedDelta.y < -deadZone)
                    characterController.joystickZ = (clampedDelta.y);
                else
                    characterController.joystickZ = 0;
            }
        }
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (touch.phase == TouchPhase.Began)
            {
                if (results.Count == 0)
                {
                    touchStartPos = touch.position;
                    isRotating = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 touchDelta = touch.position - touchStartPos;

                // Rotate the camera locally around the X-axis based on touch input
                cam.transform.localRotation *= Quaternion.Euler(-touchDelta.y * rotationSpeed, 0, 0);
                //Rotate player left & right
                player.Rotate(Vector3.up * touchDelta.x * rotationSpeed);

                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && isRotating)
            {
                isRotating = false;
            }
        }
    }
}
