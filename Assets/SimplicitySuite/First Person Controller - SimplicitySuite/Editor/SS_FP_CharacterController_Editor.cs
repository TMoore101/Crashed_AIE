using PlasticPipe.PlasticProtocol.Messages.Serialization;
using SimplicitySuite.FirstPersonController;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SS_FP_CharacterController))]
public class SS_FP_CharacterController_Editor : Editor
{
    //Movement speed variables
    private SerializedProperty walkSpeed;
    private SerializedProperty crouchSpeed;
    private SerializedProperty sprintSpeedMultiplier;
    //Movement control variables
    private SerializedProperty acceleration;
    private SerializedProperty deceleration;
    //Camera control variables
    private SerializedProperty headBobHeight;
    //Physics variables
    private SerializedProperty gravity;
    private SerializedProperty rigidbody;
    //Jumping variables
    private SerializedProperty jumpHeight;
    private SerializedProperty airControl;
    private SerializedProperty canAirJump;
    private SerializedProperty jumpCount;
    private SerializedProperty staminaDrainJumping;
    //Stamina variables
    private SerializedProperty hasStamina;
    private SerializedProperty maxStamina;
    private SerializedProperty staminaDrain;
    private SerializedProperty staminaRegen;
    private SerializedProperty staminaHoldTime;
    private SerializedProperty mustWaitMaxStamina;
    //Wall interaction variables
    private SerializedProperty canWallInteract;
    private SerializedProperty wallJumpHeight;
    private SerializedProperty wallRunTime;
    //Collision detection variables
    private SerializedProperty groundLayer;
    private SerializedProperty groundCheckDistance;
    private SerializedProperty groundCheck;
    //Miscellaneous variables
    private SerializedProperty stepHeight;
    private SerializedProperty maxFallHeight;
    //UI variables
    private SerializedProperty staminaBar;

    private void OnEnable()
    {
        //Get movement speed variables
        walkSpeed = serializedObject.FindProperty("walkSpeed");
        crouchSpeed = serializedObject.FindProperty("crouchSpeed");
        sprintSpeedMultiplier = serializedObject.FindProperty("sprintSpeedMultiplier");
        //Get movement control variables
        acceleration = serializedObject.FindProperty("acceleration");
        deceleration = serializedObject.FindProperty("deceleration");
        //Get camera control variables
        headBobHeight = serializedObject.FindProperty("headBobHeight");
        //Get physics variables
        gravity = serializedObject.FindProperty("gravity");
        rigidbody = serializedObject.FindProperty("rb");
        //Get jumping variables
        jumpHeight = serializedObject.FindProperty("jumpHeight");
        airControl = serializedObject.FindProperty("airControl");
        canAirJump = serializedObject.FindProperty("canAirJump");
        jumpCount = serializedObject.FindProperty("jumpCount");
        staminaDrainJumping = serializedObject.FindProperty("staminaDrainJumping");
        //Get stamina variables
        hasStamina = serializedObject.FindProperty("hasStamina");
        maxStamina = serializedObject.FindProperty("maxStamina");
        staminaDrain = serializedObject.FindProperty("staminaDrain");
        staminaRegen = serializedObject.FindProperty("staminaRegen");
        staminaHoldTime = serializedObject.FindProperty("staminaHoldTime");
        mustWaitMaxStamina = serializedObject.FindProperty("mustWaitMaxStamina");
        //Get wall interaction variables
        canWallInteract = serializedObject.FindProperty("canWallInteract");
        wallJumpHeight = serializedObject.FindProperty("wallJumpHeight");
        wallRunTime = serializedObject.FindProperty("wallRunTime");
        //Get collision detection variables
        groundLayer = serializedObject.FindProperty("groundLayer");
        groundCheckDistance = serializedObject.FindProperty("groundCheckDistance");
        groundCheck = serializedObject.FindProperty("groundCheck");
        //Get miscellaneous variables
        stepHeight = serializedObject.FindProperty("stepHeight");
        maxFallHeight = serializedObject.FindProperty("maxFallHeight");
        //Get UI variables
        staminaBar = serializedObject.FindProperty("staminaBar");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //Initialize movement speed variables
        EditorGUILayout.PropertyField(walkSpeed);
        EditorGUILayout.PropertyField(crouchSpeed);
        EditorGUILayout.PropertyField(sprintSpeedMultiplier);
        //Initialize movement control variables
        EditorGUILayout.PropertyField(acceleration);
        EditorGUILayout.PropertyField(deceleration);
        //Initialize camera control variables
        EditorGUILayout.PropertyField(headBobHeight);
        //Initialize physics variables
        EditorGUILayout.PropertyField(gravity);
        EditorGUILayout.PropertyField(rigidbody);
        //Initialize jumping variables
        EditorGUILayout.PropertyField(jumpHeight);
        EditorGUILayout.PropertyField(airControl);
        EditorGUILayout.PropertyField(canAirJump);
        if (canAirJump.boolValue)
            EditorGUILayout.PropertyField(jumpCount);
        if (hasStamina.boolValue)
            EditorGUILayout.PropertyField(staminaDrainJumping);
        //Initialize stamina variables
        EditorGUILayout.PropertyField(hasStamina);
        if (hasStamina.boolValue)
        {
            EditorGUILayout.PropertyField(maxStamina);
            EditorGUILayout.PropertyField(staminaDrain);
            EditorGUILayout.PropertyField(staminaRegen);
            EditorGUILayout.PropertyField(staminaHoldTime);
            EditorGUILayout.PropertyField(mustWaitMaxStamina);
        }
        //Initialize wall interaction variables
        EditorGUILayout.PropertyField(canWallInteract);
        if (canWallInteract.boolValue)
        {
            EditorGUILayout.PropertyField(wallJumpHeight);
            EditorGUILayout.PropertyField(wallRunTime);
        }
        //Initialize collision detection variables
        EditorGUILayout.PropertyField(groundLayer);
        EditorGUILayout.PropertyField(groundCheckDistance);
        EditorGUILayout.PropertyField(groundCheck);
        //Initialize miscellaneous variables
        EditorGUILayout.PropertyField(stepHeight);
        EditorGUILayout.PropertyField(maxFallHeight);
        //Initialize UI variables
        EditorGUILayout.PropertyField(staminaBar);

        //Apply inspector modifications
        serializedObject.ApplyModifiedProperties();
    }
}
