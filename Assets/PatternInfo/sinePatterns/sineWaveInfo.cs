using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineWaveInfo : MonoBehaviour
{
    [Header("Sine Wave Variables")]
    public float distanceTravelled;
    public float compression;
    public float amplitude;

    [Header("bullet variables")]
    public float speed;
    public float lifeTime;

    public bool willSineSpeedAccel;
    public AnimationCurve sineBulletCurve;
    public float timerSCurve;

    [SerializeField]
    private float delta;
    private Vector2 sineVector;

    public Vector2 origin;
    // Start is called before the first frame update
    void Start()
    {

        origin = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            this.gameObject.SetActive(false);
        }
        // make sure to reset distanceTravelled
        delta = Time.deltaTime;
        // add acceleration later
        distanceTravelled += speed * delta;
       
        sineVector = new Vector2(distanceTravelled, amplitude * Mathf.Sin(compression * distanceTravelled));
        // to be able to rotate the bullet, can either pass in the parent's euler angles or provide euler angles by itself
        sineVector = Quaternion.Euler(transform.eulerAngles) * sineVector;

        transform.position = origin + sineVector;


    }


}
