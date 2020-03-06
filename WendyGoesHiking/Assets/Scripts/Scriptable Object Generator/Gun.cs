using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string name;
    public int ammoSize;    //total ammo
    public int magazineSize;
    public int pellets;     //number of shots fired at once
    public float fireRate;
    public float bloom;     //accuracy --- higher bloom = less accurate
    public float adsBloom;  //accuracy while aiming the gun
    public float recoil;    //vertical rotation intensity when gun is fired
    public float adsRecoil;
    public float kickback;  //z axis movement intensity when gun is fired (local z position decreases as the gun moves toward the player/camera)
    public float adsKickback;
    public float aimSpeed;
    public float reloadSpeed;   //higher number = longer time = slower reload
    public bool recovery;   //whether or not you hava a recovery (like pumping a pump shotgun)
    public AudioClip[] gunshotSounds;
    public AudioClip[] reloadSounds;
    public AudioClip[] emptyMagSounds;
    public AudioClip[] recoverySounds;
    public float pitchRandomization;
    public GameObject prefab;
    public bool isEquipped;

    public GameObject[] muzzleFlashes;


    private int ammo;       // current stored ammo
    private int magazine;   //current ammo in magazine


    

    public void Initialize()
    {
        ammo = ammoSize;
        magazine = magazineSize;
    }

    public bool CanFireBullet()
    {

        if (magazine > 0)
        {
            magazine -= 1;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Reload()
    {
        ammo += magazine;
        magazine = Mathf.Min(magazineSize, ammo);
        ammo -= magazine;
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public int GetMagazine()
    {
        return magazine;
    }
   
}

