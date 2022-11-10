using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuInfo : MonoBehaviour
{
    [Header("Damage System")]
    public float reimuDmg;
    public bool canBeDamaged;
    public float dmgTimer;
    public float collisionCoolDown;
   
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
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
}
