using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.AI;


public class WeaponManager : MonoBehaviour
{

    public Gun[] loadout;    //array of gun inventory

    public Transform weaponParent;  //transform of empty gameobject

    public float recoverySpeed = 2;

    public GameObject[] bulletHolePrefabs;
    public LayerMask shootableLayers;
    public AudioSource sfx;
    public AudioSource sfx2;

    private GameObject equippedWeapon;
    private int currentIndex;
    private float currentCooldown;

    private bool isReloading;

    private Text uiAmmo;

    [SerializeField] GameObject bulletshell;


    [SerializeField] GameObject bloodSplat, bodyBlood;
    [SerializeField] LayerMask mask;



    private void Start()
    {
        // Equip(0);


        foreach (Gun guns in loadout)
        {
            guns.Initialize();
        }

        uiAmmo = GameObject.Find("Player HUD/Main Player Canvas/Ammo/Text").GetComponent<Text>();

    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {     //if the number key '1' is pressed, equip first weapon
            Equip(0);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {     //if the number key '1' is pressed, equip first weapon
            Equip(1);

        }

        if (equippedWeapon != null)
        {
            Aim(Input.GetMouseButton(1));

            if (Input.GetMouseButtonDown(0) && currentCooldown <= 0)
            {
                if (loadout[currentIndex].CanFireBullet())
                {
                    Shoot();
                }
                else if (loadout[currentIndex].GetAmmo() > 0)
                {
                    StartCoroutine(Reload(loadout[currentIndex].reloadSpeed));
                }
                else
                {
                    sfx.clip = loadout[currentIndex].emptyMagSound;
                    sfx.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
                    sfx.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))    //RELOAD
            {
                if (loadout[currentIndex].GetMagazine() < loadout[currentIndex].magazineSize && loadout[currentIndex].GetAmmo() > 0)
                {
                    StartCoroutine(Reload(loadout[currentIndex].reloadSpeed));
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

    void Aim(bool isAiming)
    {
        Transform anchorTransform = equippedWeapon.transform.Find("Anchor");
        Transform stateADS = equippedWeapon.transform.Find("States/ADS");
        Transform stateHip = equippedWeapon.transform.Find("States/Hip");

        if (isAiming)
        {
            //ADS
            anchorTransform.position = Vector3.Lerp(anchorTransform.position, stateADS.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
        else
        {
            //Hip
            anchorTransform.position = Vector3.Lerp(anchorTransform.position, stateHip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
        }
    }

    void Equip(int loadoutIndex)
    {
        if (equippedWeapon != null)
        {
            if (isReloading)
            {
                StopCoroutine("Reload");
            }
            Destroy(equippedWeapon);
        }

        currentIndex = loadoutIndex;

        GameObject newGun = Instantiate(loadout[loadoutIndex].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newGun.transform.localPosition = Vector3.zero;      //zeros out the local postion
        newGun.transform.localEulerAngles = Vector3.zero;   //zeros out the local rotation (euler angles is rotation in vector form)

        equippedWeapon = newGun;
    }


    void Shoot()
    {
        Transform spawn = transform.Find("PlayerCamera");

        //Cooldown
        currentCooldown = loadout[currentIndex].fireRate;

        SpawnBulletShell();

        //bloom
        Vector3 bloom = spawn.position + spawn.forward * 1000f;
        //raycast
        RaycastHit tempHit = new RaycastHit();

        for (int i = 0; i < Mathf.Max(1, loadout[currentIndex].pellets); i++)
        {

            bloom = spawn.position + spawn.forward * 1000f;

            bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * spawn.up;
            bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * spawn.right;
            bloom -= spawn.position;
            bloom.Normalize();




            if (Physics.Raycast(spawn.position, bloom, out tempHit, 1000f, shootableLayers))
            {

                GameObject newBulletHole = Instantiate(bulletHolePrefabs[Random.Range(0, bulletHolePrefabs.Length - 1)], tempHit.point + tempHit.normal * 0.001f, Quaternion.FromToRotation(Vector3.up, tempHit.normal));

                newBulletHole.transform.LookAt(tempHit.point + tempHit.normal);

                newBulletHole.transform.Rotate(0, 0, Random.Range(0, 360));

                newBulletHole.transform.SetParent(tempHit.transform);
                //Destroy(newBulletHole, 5f);
            }
        }


        //Audio
        sfx.clip = loadout[currentIndex].gunshotSound;
        sfx.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
        sfx.Play();

        // Gun FX
        equippedWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
        equippedWeapon.transform.position -= equippedWeapon.transform.forward * loadout[currentIndex].kickback;

        if (loadout[currentIndex].recovery)
        {
            sfx2.clip = loadout[currentIndex].recoverySound;
            sfx2.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
            sfx2.Play();
            equippedWeapon.GetComponent<Animator>().Play("Recovery", 0, 0);
        }


    }

    void SpawnBulletShell()
    {
        if (equippedWeapon.name == loadout[0].name + "(Clone)")
        {
            GameObject bullet = Instantiate(bulletshell, equippedWeapon.transform.Find("Anchor/Model/Shotgun/Cartridge_Release").position + new Vector3(0, 0.05f, 0), Quaternion.Euler(90, 0, 0));
            bullet.GetComponent<Rigidbody>().AddForce(200 * transform.up);

        }
    }

    IEnumerator Reload(float waitTime)
    {
        isReloading = true;
        equippedWeapon.SetActive(false);
        if (loadout[currentIndex].reloadSound != null)
        {
            sfx2.clip = loadout[currentIndex].reloadSound;
            sfx2.pitch = 1 - loadout[currentIndex].pitchRandomization + Random.Range(-loadout[currentIndex].pitchRandomization, loadout[currentIndex].pitchRandomization);
            sfx2.Play();
        }
        yield return new WaitForSeconds(waitTime);

        loadout[currentIndex].Reload();
        equippedWeapon.SetActive(true);
        isReloading = false;

    }


    public void RefreshAmmo(Text uiText)
    {
        int clip = loadout[currentIndex].GetMagazine();
        int ammo = loadout[currentIndex].GetAmmo();

        uiText.text = clip.ToString("D2") + " / " + ammo.ToString("D2");

    }



}
