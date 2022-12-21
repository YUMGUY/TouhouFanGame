using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuBulletInfo : MonoBehaviour
{
    public float RbulletSpeed;
    public float bulletDMG;
    public float reimuBulletLF;
    public bool timeStopped;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeStopped == false)
        {
            reimuBulletLF -= Time.deltaTime;
        }
        
        if(reimuBulletLF <= 0f)
        {
            this.gameObject.SetActive(false);
        }
        // rn just translate
        transform.Translate(Time.deltaTime * RbulletSpeed * Vector2.up);
    }

    private void OnEnable()
    {
        RbulletSpeed = 10.0f;
        reimuBulletLF = 2.1f;
    }
}
