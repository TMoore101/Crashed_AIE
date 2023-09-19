using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;

namespace SimplicitySuite.FirstPersonController
{
    //Keybind variable
    [Serializable]
    public struct Keybind
    {
        public string name;
        public KeyCode key;
    }

    public class SS_Keybinds : MonoBehaviour
    {
        //Keybinds variables
        public List<Keybind> keybinds;
    }
}