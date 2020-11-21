using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    Vector3 destination = Vector3.zero;
    Vector3 startingScale;
    float counter = 0;
    float heartAnimationSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        counter = (Time.deltaTime * heartAnimationSpeed + counter)%360;
        if (destination != Vector3.zero) 
            transform.position = Vector3.Lerp(transform.position, transform.position + destination, Time.deltaTime * 3);
        transform.localScale = new Vector3(startingScale.x + Mathf.Sin(counter)/7, startingScale.y + Mathf.Sin(counter)/7);
    }
    public void Shoot(Vector3 origin)
    {
        destination = origin;
        StartCoroutine(recuperateHeart());
    }

    IEnumerator recuperateHeart()
    {
        yield return new WaitForSeconds(1);
        gameObject.layer = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        destination = Vector3.zero;
    }
}


