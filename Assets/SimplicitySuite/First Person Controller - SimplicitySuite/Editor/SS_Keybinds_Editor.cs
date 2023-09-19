using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.Diagnostics;
using FPS_Guns_PS;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace SimplicitySuite.FirstPersonController
{
    [CustomEditor(typeof(SS_Keybinds))]
    [CanEditMultipleObjects]
    public class SS_Keybinds_Editor : Editor
    {
        //Keybind properties
        private SerializedProperty serializedKeybinds;
        private List<Keybind> keybindDictionary;

        //Directory variables
        private string directory;
        private string keybindsFileName = "keybinds.json";

        private int arraySize;

        private void OnEnable()
        {
            //Get the list of all keybinds
            serializedKeybinds = serializedObject.FindProperty("keybinds");

            arraySize = serializedKeybinds.arraySize;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            //Display list of all keybinds
            EditorGUILayout.PropertyField(serializedKeybinds);

            if (EditorGUI.EndChangeCheck())
            {
                //Reset the keybind dictionary
                keybindDictionary = new List<Keybind>();
                //Get all of the defined keybinds
                for (int i = 0; i < serializedKeybinds.arraySize; i++)
                {
                    SerializedProperty pair = serializedKeybinds.GetArrayElementAtIndex(i);
                    string name = pair.FindPropertyRelative("name").stringValue;
                    KeyCode key = (KeyCode)pair.FindPropertyRelative("key").intValue;
                    keybindDictionary.Add(new Keybind { name = name, key = key });
                }

                //Clear keybind values on a newly created keybind
                if (serializedKeybinds.arraySize > arraySize)
                {
                    serializedKeybinds.GetArrayElementAtIndex(serializedKeybinds.arraySize - 1).FindPropertyRelative("name").stringValue = "";
                    serializedKeybinds.GetArrayElementAtIndex(serializedKeybinds.arraySize - 1).FindPropertyRelative("key").intValue = (int)KeyCode.None;
                }

                // Apply any changes to the serialized object
                serializedObject.ApplyModifiedProperties();
            }

            arraySize = serializedKeybinds.arraySize;

            //Save keybinds button
            if (GUILayout.Button("Save Keybinds"))
                SaveKeybinds();

            GUILayout.BeginHorizontal();
            //Load keybinds from the default file path
            if (GUILayout.Button("Load Keybinds"))
            {
                //Get default file path
                string filePath = directory + "\\" + keybindsFileName;

                LoadKeybinds(filePath);
            }
            //Load keybinds from a custom file path
            if (GUILayout.Button("Load Custom Keybinds"))
            {
                //Get custom file path
                string filePath = EditorUtility.OpenFilePanel("Select a JSON file", "", "json");

                if (!string.IsNullOrEmpty(filePath))
                    LoadKeybinds(filePath);
            }
            GUILayout.EndHorizontal();

            //Reload default keybinds button
            if (GUILayout.Button("Restore Default Keybinds"))
            {
                RestoreKeybinds();
            }
        }

        //Save keybinds function
        private void SaveKeybinds()
        {
            //Get the directory
            directory = Application.persistentDataPath;

            //Check if the keybinds file exists
            bool fileExists = File.Exists(directory + "\\" + keybindsFileName);
            if (fileExists) { }
            else { 
                File.Create(directory + "\\" + keybindsFileName); 
            }

            //Write keybinds to JSON file
            File.WriteAllText(directory + "\\" + keybindsFileName, JsonConvert.SerializeObject(keybindDictionary, Formatting.Indented));
        }

        //Load keybinds function
        private void LoadKeybinds(string filePath)
        {
            //Check if the keybinds file exists
            bool fileExist = File.Exists(filePath);
            if (fileExist) { }
            else { return; }

            //Get all the keybinds on the json file
            string keybinds = File.ReadAllText(filePath);
            try
            {
                //Load the new keybinds
                keybindDictionary = JsonConvert.DeserializeObject<List<Keybind>>(keybinds);
                serializedKeybinds.ClearArray();
                serializedKeybinds.arraySize = keybindDictionary.Count;
                for (int i = 0; i < keybindDictionary.Count; i++)
                {
                    SerializedProperty keybindProperty = serializedKeybinds.GetArrayElementAtIndex(i);
                    keybindProperty.FindPropertyRelative("name").stringValue = keybindDictionary[i].name;
                    keybindProperty.FindPropertyRelative("key").intValue = (int)keybindDictionary[i].key;
                }
            }
            catch
            {
                return;
            }

            //Apply modifications to the editor/inspector
            serializedObject.ApplyModifiedProperties();
        }

        //Restore keybinds function
        private void RestoreKeybinds()
        {
            //Reset keybind dictionary to default values
            keybindDictionary = new List<Keybind> { 
                new Keybind { name="moveForward", key=KeyCode.W},
                new Keybind { name="moveLeft", key=KeyCode.A },
                new Keybind { name="moveBack", key=KeyCode.S },
                new Keybind { name="moveRight", key=KeyCode.D },
                new Keybind { name="sprint", key=KeyCode.LeftShift },
                new Keybind { name="crouch", key=KeyCode.LeftControl },
                new Keybind { name="jump", key=KeyCode.Space },
            };

            //Restore the new keybinds
            serializedKeybinds.ClearArray();
            serializedKeybinds.arraySize = keybindDictionary.Count;
            for (int i = 0; i < keybindDictionary.Count; i++)
            {
                SerializedProperty keybindProperty = serializedKeybinds.GetArrayElementAtIndex(i);
                keybindProperty.FindPropertyRelative("name").stringValue = keybindDictionary[i].name;
                keybindProperty.FindPropertyRelative("key").intValue = (int)keybindDictionary[i].key;
            }

            //Apply modifications to the editor/inspector
            serializedObject.ApplyModifiedProperties();
        }
    }
}