using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularCustomBehavior : MonoBehaviour
{
    //************** This is a script for a bullet **********************//

    // Start is called before the first frame update
    public float bSpeed;
    public float blifeTime;
    public Vector2 direction;

    [Header("Optional Acceleration")]
    public AnimationCurve speedCurve;
    public bool willAccel;
    private float timeAcceleration;

    [Header("Gravity Properties")]
    public bool hasGravity;
    public float gravityAccelModifer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P)) // this code is temporary, wont need parent gameobject this if the sprite is properly aligned
        {
            // point object towards player
            print("pointed bullet towards player");
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector2 dir = player.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        blifeTime -= Time.deltaTime;
        if(blifeTime <= 0)
        {
            this.gameObject.SetActive(false);
        }

        // implement falling


        // implement acceleration
        if(willAccel == true && speedCurve != null)
        {
            timeAcceleration += Time.deltaTime;
            bSpeed = speedCurve.Evaluate(timeAcceleration);
        }



        transform.Translate(bSpeed * direction * Time.deltaTime);
    }


    private void GravityAccleration()
    {

    }
}
