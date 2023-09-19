using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;

namespace FPS_Guns_PS
{
    [CustomEditor(typeof(GunManager))]
    [CanEditMultipleObjects]
    public class FPS_Gun_Editor : Editor
    {
        //Properties of the WeaponManager
        SerializedProperty gunCategories;

        //Information groups
        Dictionary<string, Dictionary<string, SerializedProperty>> gunInformation;

        private void OnEnable()
        {
            //Get the list of all guns
            gunCategories = serializedObject.FindProperty("gunCategories");
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            //Display list of all guns
            EditorGUILayout.PropertyField(gunCategories);

            if (EditorGUI.EndChangeCheck())
            {
                gunInformation = new Dictionary<string, Dictionary<string, SerializedProperty>>();

                //Get information on all gun categories
                for (int i = 0; i < gunCategories.arraySize; i++)
                {
                    gunInformation.Add(gunCategories.GetArrayElementAtIndex(i).FindPropertyRelative("gunCategory").stringValue, new Dictionary<string, SerializedProperty>());

                    //Get information on all guns
                    for (int x = 0; x < gunCategories.GetArrayElementAtIndex(i).FindPropertyRelative("guns").arraySize; x++)
                    {
                        gunInformation.ElementAt(i).Value.Add(gunCategories.GetArrayElementAtIndex(i).FindPropertyRelative("guns").GetArrayElementAtIndex(x).FindPropertyRelative("weaponName").stringValue, gunCategories.GetArrayElementAtIndex(i).FindPropertyRelative("guns").GetArrayElementAtIndex(x));
                    }
                }

                //Run conditional checks
                foreach (KeyValuePair<string, Dictionary<string, SerializedProperty>> category in gunInformation)
                {
                    foreach (KeyValuePair<string, SerializedProperty> gun in category.Value)
                    {
                        //Bullet type checks | Bullet speed
                        if (gun.Value.FindPropertyRelative("specDetails").FindPropertyRelative("bulletType").enumValueIndex == 0)
                            gun.Value.FindPropertyRelative("conditionals").FindPropertyRelative("bulletSpeed").floatValue = 0;
                        //Reload type checks | Cooling
                        if (gun.Value.FindPropertyRelative("specDetails").FindPropertyRelative("reloadType").enumValueIndex == 0)
                        {
                            gun.Value.FindPropertyRelative("conditionals").FindPropertyRelative("maxHeat").floatValue = 0;
                            gun.Value.FindPropertyRelative("conditionals").FindPropertyRelative("heatProduced").floatValue = 0;
                        }
                        //Charge type checks | Charge speed
                        if (gun.Value.FindPropertyRelative("specDetails").FindPropertyRelative("chargeType").enumValueIndex == 0)
                            gun.Value.FindPropertyRelative("conditionals").FindPropertyRelative("chargeTime").floatValue = 0;
                        //Can ADS checks | ADS speed
                        if (gun.Value.FindPropertyRelative("specDetails").FindPropertyRelative("canADS").boolValue == false)
                            gun.Value.FindPropertyRelative("conditionals").FindPropertyRelative("adsSpeed").floatValue = 0;

                        //Fire mode checks
                        if (gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").arraySize == 0)
                            gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").InsertArrayElementAtIndex(0);
                        else if (gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").arraySize > Enum.GetNames(typeof(FireModes)).Length)
                            gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").DeleteArrayElementAtIndex(Enum.GetNames(typeof(FireModes)).Length);
                        List<FireModes> fireModes = new List<FireModes>();
                        //Make sure there are no duplicate fire modes
                        for (int i = 0; i < gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").arraySize; i++)
                        {
                            int counter = 0;
                            //If the fireModes already contain the specified fireMode, change it's value to a possible option
                            while (fireModes.Contains((FireModes)gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").GetArrayElementAtIndex(i).enumValueIndex))
                            {
                                gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").GetArrayElementAtIndex(i).intValue = counter;

                                counter++;
                            }
                            fireModes.Add((FireModes)gun.Value.FindPropertyRelative("weaponStats").FindPropertyRelative("fireMode").GetArrayElementAtIndex(i).enumValueIndex);
                        }

                        //Cap possible attachments to the max number of possible attachments
                        if (gun.Value.FindPropertyRelative("possibleAttachments").arraySize > Enum.GetNames(typeof(AttachmentTypes)).Length)
                            gun.Value.FindPropertyRelative("possibleAttachments").DeleteArrayElementAtIndex(Enum.GetNames(typeof(AttachmentTypes)).Length);
                        List<AttachmentTypes> attachments = new List<AttachmentTypes>();
                        //Make sure there are no duplicate possible attachments
                        for (int i = 0; i < gun.Value.FindPropertyRelative("possibleAttachments").arraySize; i++)
                        {
                            int counter = 0;
                            //If the attachments already contain the specified attachment, change it's value a possible option
                            while (attachments.Contains((AttachmentTypes)gun.Value.FindPropertyRelative("possibleAttachments").GetArrayElementAtIndex(i).enumValueIndex))
                            {
                                gun.Value.FindPropertyRelative("possibleAttachments").GetArrayElementAtIndex(i).intValue = counter;

                                counter++;
                            }
                            attachments.Add((AttachmentTypes)gun.Value.FindPropertyRelative("possibleAttachments").GetArrayElementAtIndex(i).enumValueIndex);
                        }

                    }
                }
            }
            
            //Apply the inspector changes
            serializedObject.ApplyModifiedProperties();
        }
    }
}