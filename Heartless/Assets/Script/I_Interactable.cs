﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Interactable
{
    void PlayerEnterRange();
    void PlayerExitRange();
    void Interact();
}
