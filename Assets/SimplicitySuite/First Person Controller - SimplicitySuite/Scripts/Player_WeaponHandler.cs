using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player_WeaponHandler : MonoBehaviour
{
    //Weapon manager variable
    private WeaponManager weaponManager;

    //Weapon data
    private WeaponClasses weaponList;
    [SerializeField] public GameObject currentWeapon;
    [SerializeField] public Weapons currentWeaponData;
    [HideInInspector] public Transform bulletSpawn;
    private bool hasWeaponEquipped;
    [SerializeField] private Transform eyeSight;
    [SerializeField] private Transform weaponPosition;
    [SerializeField] private Transform ADSPosition;

    //Weapon index variables
    [HideInInspector] public int weaponIndex;
    [HideInInspector] public int currentWeaponType;

    //Individual variables
    [SerializeField] private Material playerBulletMat;

    //Weapon heat slider variable
    public Slider Heat;
    public Slider HeatCrosshair;

    //Recoil variables
    public SimplicitySuite.FirstPersonController.SS_FP_CameraController cameraController;

    //Audio
    [SerializeField]
    [Tooltip("The audio to play when the player has taken an ar shot")]
    private AudioClip[] audioClipsAR;
    [SerializeField]
    [Tooltip("The audio to play when the player has taken a shotgun shot")]
    private AudioClip[] audioClipsShotgun;
    [SerializeField]
    private AudioSource audioSource;

    public bool isMobile;

    private void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<WeaponManager>();
        weaponList = new WeaponClasses(weaponManager.weapons);

        //Equip the first available weapon
        if (!hasWeaponEquipped)
        {
            //If there is a primary weapon available, spawn with that
            foreach (Weapons weapon in weaponList.primaryWeapons)
            {
                //If the weapon in the primary weapons list is available and unlocked, start with that weapon
                if (weapon.weaponPrefab && weapon.bulletPrefab && weapon.unlocked)
                {
                    //Create weapon
                    currentWeapon = Instantiate(weapon.weaponPrefab, weaponPosition, false);
                    //Set weapon data
                    currentWeaponData = weapon;
                    weaponIndex = Array.IndexOf(weaponList.primaryWeapons, weapon);
                    bulletSpawn = currentWeapon.transform.GetChild(0);
                    currentWeapon.layer = 11;
                    foreach (Transform child in currentWeapon.transform)
                        child.gameObject.layer = 11;

                    hasWeaponEquipped = true;
                    currentWeaponType = 1;
                    return;
                }
            }
            //If there is a secondary weapon available, spawn with that
            foreach (Weapons weapon in weaponList.secondaryWeapons)
            {
                //If the weapon in the secondary weapons list is available and unlocked, start with that weapon
                if (weapon.weaponPrefab && weapon.bulletPrefab && weapon.unlocked)
                {
                    //Create weapon
                    currentWeapon = Instantiate(weapon.weaponPrefab, weaponPosition, false);
                    //Set weapon data
                    currentWeaponData = weapon;
                    weaponIndex = Array.IndexOf(weaponList.secondaryWeapons, weapon);
                    bulletSpawn = currentWeapon.transform.GetChild(0);
                    currentWeapon.layer = 11;
                    foreach (Transform child in currentWeapon.transform)
                        child.gameObject.layer = 11;

                    hasWeaponEquipped = true;
                    currentWeaponType = 2;
                    return;
                }
            }
            //If there is a tertiary weapon available, spawn with that
            foreach (Weapons weapon in weaponList.tertiaryWeapons)
            {
                //If the weapon in the tertiary weapons list is available and unlocked, start with that weapon
                if (weapon.weaponPrefab && weapon.bulletPrefab && weapon.unlocked)
                {
                    //Create weapon
                    currentWeapon = Instantiate(weapon.weaponPrefab, weaponPosition, false);
                    //Set weapon data
                    currentWeaponData = weapon;
                    weaponIndex = Array.IndexOf(weaponList.tertiaryWeapons, weapon);
                    bulletSpawn = currentWeapon.transform.GetChild(0);
                    currentWeapon.layer = 11;
                    foreach (Transform child in currentWeapon.transform)
                        child.gameObject.layer = 11;

                    hasWeaponEquipped = true;
                    currentWeaponType = 3;
                    return;
                }
            }
        }
    }

    private void Update()
    {
        //Cooldowns
        weaponManager.Cooldowns(weaponList);

        if (!isMobile)
        {
            if (currentWeapon)
            {
                //Fire weapon
                if (currentWeaponData.fireMode == 1 && !currentWeaponData.isCooling && !currentWeaponData.hasShot && Time.timeScale != 0)
                {
                    if (Input.GetMouseButton(0) || Input.GetAxis("Shoot") > 0)
                    {
                        weaponManager.Fire(bulletSpawn, eyeSight, currentWeaponData, eyeSight, playerBulletMat, true, cameraController);
                        audioSource.pitch = UnityEngine.Random.Range(2, 3.5f);
                        audioSource.clip = currentWeaponType == 1 ? audioClipsAR[UnityEngine.Random.Range(0, audioClipsAR.Length)] : audioClipsShotgun[UnityEngine.Random.Range(0, audioClipsShotgun.Length)];
                        audioSource.Play();
                    }
                }
                else if (currentWeaponData.fireMode == 2 && !currentWeaponData.isCooling && !currentWeaponData.hasShot && Time.timeScale != 0)
                {
                    if (Input.GetMouseButtonDown(0) || Input.GetAxis("Shoot") > 0)
                    {
                        weaponManager.Fire(bulletSpawn, eyeSight, currentWeaponData, eyeSight, playerBulletMat, true, cameraController);
                        audioSource.pitch = UnityEngine.Random.Range(2, 3.5f);
                        audioSource.clip = currentWeaponType == 1 ? audioClipsAR[UnityEngine.Random.Range(0, audioClipsAR.Length)] : audioClipsShotgun[UnityEngine.Random.Range(0, audioClipsShotgun.Length)];
                        audioSource.Play();
                    }
                }

                //Aim weapon
                if (Input.GetMouseButton(1) || Input.GetAxis("Aim") > 0)
                    weaponManager.Aim(currentWeapon, weaponPosition, ADSPosition, currentWeaponData, true, true);
                else
                    weaponManager.Aim(currentWeapon, weaponPosition, ADSPosition, currentWeaponData, false, true);
            }
        }
        else
        {
            if (isAiming)
            {
                weaponManager.Aim(currentWeapon, weaponPosition, ADSPosition, currentWeaponData, true, true);
            }
            else
            {
                weaponManager.Aim(currentWeapon, weaponPosition, ADSPosition, currentWeaponData, false, true);
            }
            if (isShooting)
            {
                //Fire weapon
                if (currentWeaponData.fireMode == 1 && !currentWeaponData.isCooling && !currentWeaponData.hasShot && Time.timeScale != 0)
                {
                    weaponManager.Fire(bulletSpawn, eyeSight, currentWeaponData, eyeSight, playerBulletMat, true, cameraController);
                    audioSource.pitch = UnityEngine.Random.Range(2, 3.5f);
                    audioSource.clip = currentWeaponType == 1 ? audioClipsAR[UnityEngine.Random.Range(0, audioClipsAR.Length)] : audioClipsShotgun[UnityEngine.Random.Range(0, audioClipsShotgun.Length)];
                    audioSource.Play();
                }
                else if (currentWeaponData.fireMode == 2 && !currentWeaponData.isCooling && !currentWeaponData.hasShot && Time.timeScale != 0)
                {
                    weaponManager.Fire(bulletSpawn, eyeSight, currentWeaponData, eyeSight, playerBulletMat, true, cameraController);
                    audioSource.pitch = UnityEngine.Random.Range(2, 3.5f);
                    audioSource.clip = currentWeaponType == 1 ? audioClipsAR[UnityEngine.Random.Range(0, audioClipsAR.Length)] : audioClipsShotgun[UnityEngine.Random.Range(0, audioClipsShotgun.Length)];
                    audioSource.Play();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetAxis("DPadHorizontal") < 0)
        {
            weaponManager.SwapWeapon(currentWeapon, 1, weaponList, weaponIndex, weaponPosition, currentWeaponData, bulletSpawn, gameObject.GetComponent<Player_WeaponHandler>(), currentWeaponType);
            currentWeapon.layer = 11;
            foreach (Transform child in currentWeapon.transform)
                child.gameObject.layer = 11;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis("DPadVertical") > 0)
        {
            weaponManager.SwapWeapon(currentWeapon, 2, weaponList, weaponIndex, weaponPosition, currentWeaponData, bulletSpawn, gameObject.GetComponent<Player_WeaponHandler>(), currentWeaponType);
            currentWeapon.layer = 11;
            foreach (Transform child in currentWeapon.transform)
                child.gameObject.layer = 11;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetAxis("DPadHorizontal") > 0)
        {
            weaponManager.SwapWeapon(currentWeapon, 3, weaponList, weaponIndex, weaponPosition, currentWeaponData, bulletSpawn, gameObject.GetComponent<Player_WeaponHandler>(), currentWeaponType);
            currentWeapon.layer = 11;
            foreach (Transform child in currentWeapon.transform)
                child.gameObject.layer = 11;
        }


        //Adjust overheat slider to the current weapon's heat level
        Heat.value = currentWeaponData.coolingCDTimer / currentWeaponData.coolingCooldown;

        HeatCrosshair.value = 0.75f * (currentWeaponData.coolingCDTimer / currentWeaponData.coolingCooldown) + 0.25f;
    }


    private bool isAiming;
    public void MobileAim()
    {
        if (currentWeapon)
        {
            isAiming = !isAiming;
        }
    }

    private bool isShooting;
    public void MobileShootDown()
    {
        if (currentWeapon)
        {
            isShooting = true;
        }
    }
    public void MobileShootUp()
    {
        if (currentWeapon)
        {
            isShooting = false;
        }
    }
}