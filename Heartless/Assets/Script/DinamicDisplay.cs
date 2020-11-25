using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinamicDisplay : MonoBehaviour
{

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
    }
    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("collisiion");
        if(other.tag == "Player")
        {
            sprite.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            sprite.enabled = false;
        }
    }
}
