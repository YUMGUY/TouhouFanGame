using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuBulletInfo : MonoBehaviour
{
    public float RbulletSpeed;
    public float bulletDMG;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rn just translate
        transform.Translate(Time.deltaTime * RbulletSpeed * Vector2.up);
    }
}
