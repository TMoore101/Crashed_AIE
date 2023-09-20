using SimplicitySuite.FirstPersonController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mobile_Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystick;
    public Transform joystickHandle;

    private bool isMoving;
    public float maxHandleDistance = 200;
    public float deadZone = 50;

    public SS_FP_CharacterController characterController;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(joystick.GetComponent<RectTransform>(), eventData.position))
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
}
