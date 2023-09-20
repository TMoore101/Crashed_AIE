using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

namespace SimplicitySuite.FirstPersonController
{
    //Keybind variable
    [Serializable]
    public struct Keybind
    {
        public string name;
        public KeyCode key;
    }

    [Serializable]
    public struct JoystickBind
    {
        public string name;
        public string key;
    }

    public class SS_Keybinds : MonoBehaviour
    {
        //Keybinds variables
        public List<Keybind> keybinds;

        //Joystick variables
        public List<JoystickBind> joystickBinds;
    }
}