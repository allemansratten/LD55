using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClickableUnit : MonoBehaviour
{
    public event Action MouseDown;

    public void OnMouseDown()
    {
        MouseDown?.Invoke();
    }
}
