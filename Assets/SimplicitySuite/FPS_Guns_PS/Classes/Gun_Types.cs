using System;
using UnityEngine;

namespace FPS_Guns_PS
{
    [Serializable]
    public class Gun_Types
    {
        public string gunCategory;

        public KeyCode gunKey;

        //Weapon types
        public FPS_Gun[] guns;

        //Constructor
        public Gun_Types() { }
    }
}