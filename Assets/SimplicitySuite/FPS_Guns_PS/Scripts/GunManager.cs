using UnityEngine;

namespace FPS_Guns_PS
{
    public class GunManager : MonoBehaviour
    {
        public Gun_Types[] gunCategories;

        //Constructor
        public GunManager() { }

        //Fire function
        public void Fire(FPS_Gun currentWeaponData) { }

        //Recoil function
        public void Recoil(FPS_Gun currentWeaponData) { }

        //Aim function
        public void Aim(FPS_Gun currentWeaponData) { }

        //Swap weapon function
        public void SwapWeapon() { }


        //CONDITIONAL FUNCTIONS
        //Cooldowns function
        public void Cooldowns(Gun_Types[] weapons) { }

        //Charge weapon function
        public void Charge(FPS_Gun currentWeaponData) { }
    }
}