using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BasicBehaviour
{
    public class ClickableUnit : MonoBehaviour
    {
        public event Action MouseDown;

        public void OnMouseDown()
        {
            MouseDown?.Invoke();
        }
    }

    public class Selectable : MonoBehaviour
    {

    }
}
