using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public GameObject target;
    public float cameraSmooth = 1;
    public float cameraMovementonDirection = 3;
    Vector3 offSet;
    // Start is called before the first frame update
    void Start()
    {
        offSet = Camera.main.ScreenToViewportPoint(new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight) / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = Camera.main.ScreenToViewportPoint(Input.mousePosition)-offSet;
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, -10)+
            new Vector3(movement.x, movement.y, -10)*cameraMovementonDirection, Time.deltaTime * cameraSmooth);
    }
}
