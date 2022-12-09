using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuInfo : MonoBehaviour
{
    [Header("Damage System")]
    public simpleBulletPool reimuBulletPool;
    public simpleBulletPool reimuBulletPool2;
    public GameObject reimuBullet;
    public float fireRate_Reimu;
    public float fireCooldown;
    public Transform firingPos_1;
    public Transform firingPos_2;

    public float reimuDmg; // needs to be connected to sakuya info
    public bool canBeDamaged;
    public bool canShoot;
    public float dmgTimer;
    public float collisionCoolDown;




    private void Awake()
    {
       // reimuBulletPool = GameObject.FindGameObjectWithTag("ReimuBulletPool").GetComponent<simpleBulletPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fireRate_Reimu -= Time.deltaTime;
        
        if(Input.GetKey(KeyCode.Z) && fireRate_Reimu <= 0 && canShoot == true)
        {
            ShootBullets();
            fireRate_Reimu = fireCooldown;
        }
        dmgTimer -= Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(dmgTimer <= 0 && canBeDamaged == true && !other.CompareTag("HungerPointBullet")) // ADD HUNGER BULLET COLLISION, ADD POINTS AND audioclip
        {
            print("Reimu Collided");
            dmgTimer = collisionCoolDown;
            
            GameManager.instance.DecreaseLife();
        }
        else if(other.CompareTag("HungerPointBullet"))
        {
            GameManager.instance.IncreaseHunger(20);
        }
       

        // add tag conditional for green bullets
    }

    // add timer
    public void ShootBullets()
    {
        GameObject b1 = reimuBulletPool.GetPooledObject();
        GameObject b2 = reimuBulletPool2.GetPooledObject();
        b1.transform.position = firingPos_1.position;
        b2.transform.position = firingPos_2.position;
        b1.GetComponent<ReimuBulletInfo>().RbulletSpeed = 10f;
        b2.GetComponent<ReimuBulletInfo>().RbulletSpeed = 10f;
        b1.SetActive(true);
        b2.SetActive(true);
    }
}
