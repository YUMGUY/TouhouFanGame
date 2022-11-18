using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuInfo : MonoBehaviour
{
    [Header("Damage System")]
    public GameObject reimuBullet;
    public float fireRate_Reimu;
    public float fireCooldown;
    public Transform firingPos_1;
    public Transform firingPos_2;

    public float reimuDmg;
    public bool canBeDamaged;
    public bool canShoot;
    public float dmgTimer;
    public float collisionCoolDown;

   
    



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
        if(dmgTimer <= 0 && canBeDamaged == true)
        {
            print("Reimu Collided");
            dmgTimer = collisionCoolDown;
            
            GameManager.instance.DecreaseLife();
        }
       
    }

    // add timer
    public void ShootBullets()
    {
        GameObject b1 = Instantiate(reimuBullet);
        GameObject b2 = Instantiate(reimuBullet);
        b1.transform.position = firingPos_1.position;
        b2.transform.position = firingPos_2.position;
    }
}
