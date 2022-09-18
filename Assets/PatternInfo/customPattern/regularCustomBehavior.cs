using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regularCustomBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float bSpeed;
    public float blifeTime;
    public Vector2 direction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        blifeTime -= Time.deltaTime;
        if(blifeTime <= 0)
        {
            this.gameObject.SetActive(false);
        }
        transform.Translate(bSpeed * direction * Time.deltaTime);
    }
}
