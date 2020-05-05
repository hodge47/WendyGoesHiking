using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using Sirenix.OdinInspector;


public class WeaponManager : MonoBehaviour
{

    public static WeaponManager current;

    public List<Gun> loadout;    //array of gun inventory

    public Transform weaponParent;  //transform of empty gameobject

    public float recoverySpeed = 2;

    public GameObject[] bulletHolePrefabs;
    public LayerMask shootableLayers;
    public AudioSource sfx;
    public AudioSource sfx2;

    public bool isAiming;

    private GameObject equippedWeapon;
    private int currentIndex;
    private float currentCooldown;

    private bool isReloading;

    [SerializeField] GameObject ammoOne;
    [SerializeField] GameObject ammoTwo;
    [SerializeField] Text uiAmmo;



    [SerializeField] LayerMask mask;

    [Button(ButtonSizes.Medium), GUIColor(0, 1, 0)]
    private void GiveAmmo()
    {
        if(loadout[currentIndex].isEquipped)
        {
            loadout[currentIndex].GiveAmmo(50);
        }
    }

    //Input
    private PlayerControlActions playerControlActions;


    private void Start()
    {
        // Equip(0);

        current = this; //The current weapon Manager is assigned so you can access it whenever you need to.

        foreach (Gun guns in loadout)
        {
            guns.Initialize();
        }

        // Initialize input
        playerControlActions = PlayerControlActions.CreateWithDefaultBindings();
        // Load the player's bindings
        if (PlayerPrefs.HasKey("InputBindings"))
        {
            playerControlActions.Load(PlayerPrefs.GetString("InputBindings"));
        }
    }

    private void Update()
    {
        isAiming = playerControlActions.Zoom.IsPressed;

        //Debug.Log("Current Ammo: " + loadout[currentIndex].GetAmmo());

        // Compass, shotgun, flashlight
        if (playerControlActions.CompassSwitch.WasPressed )
        {     //if the number key '1' is pressed, equip first weapon            && !loadout[currentIndex].isEquipped
            Equip(0);
            //jpost Audio
            PlayCompassEquip();

        }
        if (playerControlActions.ShotgunSwitch.WasPressed)
        {     //if the number key '1' is pressed, equip first weapon
            Equip(1);
        }

        if (playerControlActions.FlashlightSwitch.WasPressed)
        {     //if the number key '1' is pressed, equip first weapon
            Equip(2);
            //jpost Audio
            PlayFlashlightTurnOn();
        }

        if (equippedWeapon != null)
        {
            Aim(isAiming);

            if (playerControlActions.Shoot.IsPressed && currentCooldown <= 0 && !GlideController.current.isSprinting)
            {
                if (loadout[currentIndex].CanFireBullet() && !isReloading)
                {
                    Shoot();
                }
                else if (loadout[currentIndex].GetAmmo() > 0 && !isReloading)
                {
                    StartCoroutine(Reload());
                }
                else
                {
                    // I was annoyed with the out of bounds error of the empty mag sound so I just added this check for uninitialized array/list lol -Michael
                    if(loadout[currentIndex].emptyMagSounds.Length > 0)
                    {
                        sfx.clip = loadout[currentIndex].emptyMagSounds[Random.Range(0, loadout[currentIndex].emptyMagSounds.Length)];
                        sfx.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
                        sfx.Play();
                    } 
                }
            }

            if (playerControlActions.Reload.WasPressed)    //RELOAD
            {
                if (loadout[currentIndex].GetMagazine() < loadout[currentIndex].magazineSize && loadout[currentIndex].GetAmmo() > 0)
                {
                    StartCoroutine(Reload());
                }
            }
            // weapon position return
            equippedWeapon.transform.localPosition = Vector3.Lerp(equippedWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * recoverySpeed);

            //Cooldown
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }

        }

        RefreshAmmo(uiAmmo);





    }

    void Aim(bool aiming)
    {

        Transform anchorTransform = equippedWeapon.transform.Find("Anchor");
        Transform stateADS = equippedWeapon.transform.Find("States/ADS");
        Transform stateHip = equippedWeapon.transform.Find("States/Hip");
        Transform stateRunning = equippedWeapon.transform.Find("States/Running");

        if (aiming && !GlideController.current.isSprinting)
        {
            //ADS
            anchorTransform.position = Vector3.Lerp(anchorTransform.position, stateADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            anchorTransform.rotation = Quaternion.Lerp(anchorTransform.rotation, stateADS.rotation, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else if (!aiming && !GlideController.current.isSprinting)
        {
            //Hip
            anchorTransform.position = Vector3.Lerp(anchorTransform.position, stateHip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            anchorTransform.rotation = Quaternion.Lerp(anchorTransform.rotation, stateHip.rotation, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else if (GlideController.current.isSprinting)
        {
            //sprinting
            if (stateRunning != null)
            {
                anchorTransform.position = Vector3.Lerp(anchorTransform.position, stateRunning.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
                anchorTransform.rotation = Quaternion.Lerp(anchorTransform.rotation, stateRunning.rotation, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
        }
    }

    public void Equip(int loadoutIndex)
    {

        if (loadoutIndex != currentIndex)
        {
            loadout[currentIndex].isEquipped = false;
        }

        if (equippedWeapon != null)
        {
            if (isReloading)
            {
                StopCoroutine("Reload");
            }
            Destroy(equippedWeapon);
        }

        

        if (loadout[loadoutIndex] != null)
        {
            currentIndex = loadoutIndex;
            GameObject newGun = Instantiate(loadout[loadoutIndex].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            newGun.transform.localPosition = Vector3.zero;      //zeros out the local postion
            newGun.transform.localEulerAngles = Vector3.zero;   //zeros out the local rotation (euler angles is rotation in vector form)
            loadout[currentIndex].isEquipped = true;
            equippedWeapon = newGun;


        }
    }


    void Shoot()
    {
        Transform spawn = transform.Find("PlayerCamera");

        //Cooldown
        currentCooldown = loadout[currentIndex].fireRate;


        //bloom
        Vector3 bloom = spawn.position + spawn.forward * 1000f;
        //raycast
        RaycastHit tempHit = new RaycastHit();

        for (int i = 0; i < Mathf.Max(1, loadout[currentIndex].pellets); i++)
        {

            bloom = spawn.position + spawn.forward * 1000f;

            if (isAiming)
            {
                bloom += Random.Range(-loadout[currentIndex].adsBloom, loadout[currentIndex].adsBloom) * spawn.up;
                bloom += Random.Range(-loadout[currentIndex].adsBloom, loadout[currentIndex].adsBloom) * spawn.right;
            }
            else
            {
                bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * spawn.up;
                bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * spawn.right;
            }
            bloom -= spawn.position;
            bloom.Normalize();




            if (Physics.Raycast(spawn.position, bloom, out tempHit, 1000f, shootableLayers))
            {

                GameObject newBulletHole = Instantiate(bulletHolePrefabs[Random.Range(0, bulletHolePrefabs.Length - 1)], tempHit.point + tempHit.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, tempHit.normal));

                newBulletHole.transform.LookAt(tempHit.point + tempHit.normal);

                newBulletHole.transform.Rotate(0, 0, Random.Range(0, 360));

                newBulletHole.transform.SetParent(tempHit.transform);
                //Destroy(newBulletHole, 5f);
                GameObject _hitObject = tempHit.collider.gameObject;
                // Damage the wendigo - check raycast hit AI GameObject
                if(_hitObject.tag == "AI")
                {
                    Debug.Log("Hit AI");
                    // Get the AIHealth script from the AI GameObject
                    AIHealth _aiHealth = _hitObject.GetComponent<AIHealth>();
                    // Check to make sure the AIHealth script isn't null
                    if(_aiHealth != null)
                    {
                        // Call DamageWendigo(AIHealth, (int)amount) function
                        DamageWendigo(_aiHealth, loadout[currentIndex].damage);
                    }
                }
            }
        }

        //jpost Audio
        //play shotgun fire single barrel
        if (!isAiming)
        {
            PlayFireSingleBarrel();
        }
        

        //unity audio deprecated DO NOT USE
        ////Audio
        //sfx.clip = loadout[currentIndex].gunshotSounds[Random.Range(0, loadout[currentIndex].gunshotSounds.Length - 1)];
        //sfx.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
        //sfx.Play();

        // Gun FX

        Transform muzzleFlashLocation = equippedWeapon.transform.Find("Anchor/Resources/MuzzleFlash");

        Vector3 position = new Vector3(muzzleFlashLocation.position.x, muzzleFlashLocation.position.y, muzzleFlashLocation.position.z);
        Quaternion rotation = new Quaternion(muzzleFlashLocation.rotation.x, muzzleFlashLocation.rotation.y, muzzleFlashLocation.rotation.z, muzzleFlashLocation.rotation.w);


        GameObject muzzleFlash = Instantiate(loadout[currentIndex].muzzleFlashes[Random.Range(0, loadout[currentIndex].muzzleFlashes.Length - 1)], position, rotation); 

        Destroy(muzzleFlash, 4);

        if (isAiming)
        {
            equippedWeapon.transform.Rotate(-loadout[currentIndex].adsRecoil, 0, 0);
            equippedWeapon.transform.position -= equippedWeapon.transform.forward * loadout[currentIndex].adsKickback;
            //jpost Audio
            PlayFireDoubleBarrel();
        }
        else
        {
            equippedWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
            equippedWeapon.transform.position -= equippedWeapon.transform.forward * loadout[currentIndex].kickback;

        }

        if (loadout[currentIndex].recovery)
        {
            sfx2.clip = loadout[currentIndex].recoverySounds[Random.Range(0, loadout[currentIndex].recoverySounds.Length-1)];
            sfx2.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
            sfx2.Play();
            equippedWeapon.GetComponent<Animator>().Play("Recovery", 0, 0);
        }


    }

    /// <summary>
    /// Damages the wendigo's health by calling the RemoveHealth() function in AIHealth
    /// </summary>
    /// <param name="_healthScript"></param>
    /// <param name="_amount"></param>
    private void DamageWendigo(AIHealth _healthScript, int _amount)
    {
        _healthScript.RemoveHealth(_amount);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        equippedWeapon.GetComponent<Animator>().Play("Reload", 0, 0);


        if (loadout[currentIndex].reloadSounds[0] != null)
        {
            sfx2.clip = loadout[currentIndex].reloadSounds[Random.Range(0, loadout[currentIndex].reloadSounds.Length-1)];
            sfx2.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
            if (loadout[currentIndex].GetMagazine() == 0)
            {
                //jpost Audio
                //PlayShotgunReload();

                //unity audio deprecated DO NOT USE
                //sfx2.Play();
                //yield return new WaitForSeconds(sfx2.clip.length);
                //sfx2.clip = loadout[currentIndex].reloadSounds[Random.Range(0, loadout[currentIndex].reloadSounds.Length - 1)];
                //sfx2.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
                //sfx2.Play();

            }
            else
            {
                //PlayShotgunReload();
                //unity audio deprecated DO NOT USE
                //sfx2.Play();

            }
        }

        yield return new WaitForSeconds(sfx2.clip.length * 1.5f);


        //yield return new WaitForSeconds(equippedWeapon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        loadout[currentIndex].Reload();
        isReloading = false;

    }


    public void RefreshAmmo(Text uiText)
    {
        int clip = loadout[currentIndex].GetMagazine();
        int ammo = loadout[currentIndex].GetAmmo();

        if (clip == 2)
        {
            ammoOne.SetActive(true);
            ammoTwo.SetActive(true);
        } 

        if (clip == 1)
        {
            ammoOne.SetActive(false);
            ammoTwo.SetActive(true);
        }

        if (clip == 0)
        {
            ammoOne.SetActive(false);
            ammoTwo.SetActive(false);
        }

        if (currentIndex == 1)
        {
          //  uiText.text = ammo.ToString("D2");
        }
        else
        {
           // uiText.text = "";
        }


    }

    //jpost Audio
    private void PlayFireSingleBarrel()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_shotgun_fire_singlebarrel", gameObject.transform.position);
    }
    //jpost Audio
    private void PlayFireDoubleBarrel()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_shotgun_fire_doublebarrel", gameObject.transform.position);
    }
    //jpost Audio
    private void PlayShotgunReload()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_shotgun_reload", gameObject.transform.position);
    }
    //jpost Audio
    private void PlayFlashlightTurnOn()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_flashlight_turnon", gameObject.transform.position);
    }
    //jpost Audio
    private void PlayCompassEquip()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Interactibles/sx_wgh_game_int_compass_equip", gameObject.transform.position);
    }

    private void OnDestroy()
    {
        // Destroy the player action set
        playerControlActions.Destroy();
    }
}
