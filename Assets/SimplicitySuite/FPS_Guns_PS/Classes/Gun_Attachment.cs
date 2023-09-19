using System;

namespace FPS_Guns_PS
{
    [Serializable]
    public class Gun_Attachment
    {
        public string attachment;

        //Constructor
        public Gun_Attachment() { }
    }

    //Attachment types
    public enum AttachmentTypes
    {
        Scope,
        Magazine,
        Muzzle,
        Handle,
        Stock,
        Barrel
    }
}
