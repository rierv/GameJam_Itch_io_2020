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
    public GameObject enemy=null;
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
        else if(enemy)
        {
            Vector2 coord = new Vector2(Mathf.Sin(counter), Mathf.Cos(counter));
            Vector3 MouseOffset = enemy.transform.position + new Vector3(coord.x, coord.y / 1.5f, 0);
            if (coord.y < 0) GetComponent<SpriteRenderer>().sortingOrder = 10;
            else GetComponent<SpriteRenderer>().sortingOrder = -10;
            transform.position = Vector3.Lerp(transform.position, MouseOffset, Time.deltaTime * 5);

        }
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
        if (collision.gameObject.tag == "Enemy")
        {
            gameObject.layer = 11;
            collision.gameObject.GetComponentInChildren<EnemyController>().stunned=true;
            enemy = collision.gameObject;
        }
    }
}


