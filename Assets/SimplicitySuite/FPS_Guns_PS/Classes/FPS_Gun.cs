using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPS_Guns_PS
{
    [Serializable]
    public class FPS_Gun
    {
        [Tooltip("Name of the weapon")]
        public string weaponName;                           //Name of the weapon

        public WeaponInformation weaponInfo;

        public SpecDetails specDetails;

        public WeaponStats weaponStats;

        public Conditionals conditionals;
        
        //Attachment variables
        [Header("Attachments")]
        [Tooltip("Possible Attachments")]
        public List<AttachmentTypes> possibleAttachments;   //List of available attachment types | Scope, Magazine, Muzzle
        //public List<Gun_Attachment> defaultAttachments;     //List of default attachments on the weapon

        //Constructor
        public FPS_Gun() { }
    }


    [Serializable]
    public class WeaponInformation
    {
        //Weapon information
        [Tooltip("Weapon description")]
        [TextArea(minLines: 1, maxLines: 4)]
        public string weaponDescription;                    //Weapon Description
        [Tooltip("Must include a valid weapon prefab")]
        public GameObject weaponPrefab;                     //Weapon Prefab | Must be a valid weapon prefab
        [Tooltip("Must include a valid bullet prefab")]
        public GameObject bulletPrefab;                     //Bullet prefab | Must be a valid bullet prefab
        [Tooltip("Must be unlocked to keep weapon")]
        public bool isUnlocked;                             //Is weapon unlocked check
        
    }

    [Serializable]
    public class SpecDetails
    {
        //Spec details
        [Tooltip("Mode of bullet being shot")]
        public BulletTypes bulletType;                      //Bullet type | Hitscan, Projecticle
        [Tooltip("Type of reload function")]
        public ReloadTypes reloadType;                      //Reload type | Ammo, Overheat
        [Tooltip("Charge type")]
        public ChargeTypes chargeType;                      //Charge Type | Instant, Charge
        [Tooltip("Can aim down sights")]
        public bool canADS;                                 //Can ADS | Check if you can aim down sights
    }

    [Serializable]
    public class WeaponStats
    {
        //Weapon stats
        [Tooltip("Fire mode")]
        public FireModes[] fireMode;                          //Fire mode | Auto, Semi-Auto, Burst, Single-Shot
        [Tooltip("Damage per bullet")]
        public float damage;                                //Bullet damage
        [Tooltip("Maximum range for a bullet to travel")]
        public float maxRange;                              //Max bullet range
        [Tooltip("Amount of bullets per shot")]
        public int bulletCount;                             //Bullet count
        [Range(0, 45f)]
        [Tooltip("Spread radius for each bullet in degrees")]
        public float bulletSpreadRadius;                    //Bullet spread radius | Will be converted from Degrees to Radians later
        [Range(0, 90)]
        [Tooltip("Amount of recoil in degrees")]
        public float recoilAmount;                          //Recoil amount | In degrees
        [Tooltip("Speed to reload the weapon in seconds")]
        public float reloadSpeed;                           //Reload speed

        //Hidden variables - For computation
        [HideInInspector] public bool hasShot;              //Has the weapon shot check
        [HideInInspector] public float rateOfFireTimer;     //Timer counting until the rate of fire, in between each shot
        [HideInInspector] public bool isReloading;          //Is the weapon reloading check
        [HideInInspector] public bool isCooling;            //Is the weapon cooling down check
        [HideInInspector] public Dictionary<string, float> attachmentModifiers; //Attachment modifiers | Stat changes due to attachments
    }

    [Serializable]
    public class Conditionals
    {
        //BULLET TYPE
        [Header("Bullet Type | Projectile")]
        [Tooltip("Speed of bullet in meters per second")]
        public float bulletSpeed;                           //Bullet speed

        //FIRE MODE
        [Header("Fire Mode | Auto, Semi-Auto, Burst")]
        [Tooltip("The rate at which bullets fire in seconds")]
        public float rateOfFire;                            //Rate of fire

        //COOLING
        [Header("Reload Type | Heat")]
        [Tooltip("Maximum heat produced before overheating")]
        public float maxHeat;                               //Max heat produced before overheating
        [Tooltip("Heat produced from each shot")]
        public float heatProduced;                          //Heat produced from each shot        

        //CHARGE TYPE
        [Header("Charge Type | Charge")]
        [Tooltip("Time to charge the weapon before shooting")]
        public float chargeTime;                            //Charge time

        //ADS
        [Header("Can Aim Down Sights(ADS) | True")]
        [Tooltip("Speed to aim down sights in seconds")]
        public float adsSpeed;                              //Speed to aim down sights

    }

    //Bullet types
    public enum BulletTypes
    {
        Hitscan,        //Raycast bullet
        Projectile,     //Physical bullet
        Continuous      //Continuous firing | Akin to flame throwers or beam attacks
    }

    //Fire modes
    public enum FireModes
    {
        Auto,           //Constant shooting
        SemiAuto,       //Tap fire
        Burst,          //Burst fire
        SingleShot      //1 Shot till reload
    };

    //Reload types
    public enum ReloadTypes
    {
        Ammo,           //Ammunition based reload
        Heat            //Heat based based recharge
    }

    //Charge types
    public enum ChargeTypes
    {
        Instant,        //Instant fire
        Charge          //Charge to fire
    }
}