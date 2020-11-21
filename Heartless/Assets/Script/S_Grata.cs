using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Grata : MonoBehaviour, I_Interactable
{
    public void PlayerEnterRange()
    {
        ManagerCunicoli.instance.SelectedGrata = gameObject;
    }

    public void PlayerExitRange()
    {
        ManagerCunicoli.instance.SelectedGrata = null;
    }
}
