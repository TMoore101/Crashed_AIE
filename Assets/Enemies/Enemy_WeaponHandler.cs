using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WeaponHandler : MonoBehaviour
{
    //Weapon Manager variable
    private WeaponManager weaponManager;

    //Audio
    private AudioSource audioSource;
    public AudioClip[] shotClips;

    //Weapon daata
    private WeaponClasses weaponList;
    private GameObject currentWeapon;
    public Weapons currentWeaponData;
    public Transform bulletSpawn;
    private bool hasWeaponEquipped;

    public bool shoot;
    [SerializeField] private Transform eyeSight;

    //Individual variables
    [SerializeField] private Material enemyBulletMat;
    [SerializeField] private Transform arms;

    private void Start()
    {
        weaponManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<WeaponManager>();
        weaponList = new WeaponClasses(weaponManager.weapons);

        currentWeaponData = weaponList.primaryWeapons[0];
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Cooldowns
        weaponManager.Cooldowns(weaponList);
    }

    public void Fire()
    {
        if (!currentWeaponData.isCooling && !currentWeaponData.hasShot)
        {
            audioSource.clip = shotClips[UnityEngine.Random.Range(0, shotClips.Length - 1)];
            audioSource.Play();
            weaponManager.Fire(bulletSpawn, arms, currentWeaponData, eyeSight, enemyBulletMat, false, null);
        }
    }
}
