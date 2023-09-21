using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimplicitySuite.FirstPersonController
{
    public class SS_FP_CameraController : MonoBehaviour
    {
        //Camera variables
        [Header("Camera")]
        [SerializeField] private float mouseSensitivity = 3f;
        private float xRotation = 0f;

        //Player variables
        [Header("Player")]
        [SerializeField] private Transform player;

        public float MouseYSum;

        private void Start()
        {
            //Lock mouse to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            //Get mouseX & mouseY values * mouseSensitivity
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            if (mouseX == 0)
                mouseX = -Input.GetAxis("ControllerUp") * mouseSensitivity/2;
            if (mouseY == 0)
                mouseY = Input.GetAxis("ControllerTurn") * mouseSensitivity/2;

            //Reverse xRotation and clamp it to 90 degrees up & down
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -75f, 85f);

            //Rotate camera up & down
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //Rotate player left & right
            player.Rotate(Vector3.up * mouseX);
        }
    }
}