using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    public GameObject boss;
    public Transform objectToFollow;
    public Vector2 offset;
    private RectTransform rectTransform = null;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
       
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(cam, objectToFollow.position) + offset;
    }
}
