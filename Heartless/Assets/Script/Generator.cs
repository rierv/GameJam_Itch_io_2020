using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public bool broken = false;
    float timer = 0;
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer > 1) broken = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            timer += Time.deltaTime;
            if (timer > 1) broken = false;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        timer = 0;
    }
}
