using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipment : Equipment
{
    public Transform transform { get; set; }

    private GameObject bulletPrefab;

    private GameObject bulletSpawnPoint;


    public float damage { get; private set; }
    public float inaccuracy { get; private set; }
    public float fireRate { get; private set; }
    public float range { get; private set; }

    private float sensitivity = 2f;

    public override void Initialize(string name, int cost, int level, float damage, float inaccuracy, float fireRate, float range)
    {
        base.Initialize(name, cost, level);
        this.damage = damage;
        this.inaccuracy = inaccuracy;
        this.fireRate = fireRate;
        this.range = range;
        sensitivity = .5f * FindObjectOfType<AppManager>().mouseSensitivity;
    }

    public override void AddUpgrade(Upgrade upgrade)
    {
        if (upgrade is WeaponUpgrade)
        {
            upgrades.Add(upgrade);
            WeaponUpgrade weaponUpgrade = (WeaponUpgrade)upgrade;

            level = weaponUpgrade.level;
            damage = weaponUpgrade.damageIncrease;
            inaccuracy = weaponUpgrade.accuracyIncrease;
            fireRate = weaponUpgrade.fireRateIncrease;
            range = weaponUpgrade.rangeIncrease;
        }
    }

    void Start()
    {
        // Assigns the appropraite objects for the bullet and the spawn point
        bulletPrefab = (GameObject)Resources.Load("Bullet");
        bulletSpawnPoint = GameObject.Find("Bullet Spawn Point");
    }

    // Function for rotating the weapon
    public void Rotate(float rotationAmount = -10000f)
    {
        if (rotationAmount == -10000f)
        {
            //not controlled by AI so use mouse input
            transform.Rotate(Vector3.forward * Input.GetAxis("Mouse X") * sensitivity);
        }
        else
        {
            transform.Rotate(Vector3.forward * rotationAmount);
        }
    }

    // Function for shooting the weapon
    public void Shoot()
    {
        GameObject bullet;
        Vector3 velocity = -transform.up;

        if (inaccuracy != 0)
        {
            Vector2 rand = Random.insideUnitCircle;
            velocity += new Vector3(rand.x, rand.y, 0) * inaccuracy;
        }

        velocity = velocity.normalized * range;
        bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation) as GameObject;
        bullet.GetComponent<Bullet>().velocity = velocity;


        //Instantiate(bullet.transform, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
    }
}
