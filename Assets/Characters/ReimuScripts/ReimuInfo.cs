using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuInfo : MonoBehaviour
{
    [Header("Damage System")]
    public float reimuDmg;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Reimu Collided");
    }
}
