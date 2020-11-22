using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttivatoreBotole : MonoBehaviour
{

    public GameObject entrata;
    public GameObject uscita;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EnableCunicolo()
    {
        Debug.Log("printone");


        entrata.SetActive(true);
        uscita.SetActive(true);

    }
}
