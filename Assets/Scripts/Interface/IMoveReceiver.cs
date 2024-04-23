using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public interface IMoveReceiver
{
    public void KeyDown(KeyCode keyCode);
    public void KeyUp(KeyCode keyCode);
    public void KeyReset(KeyCode keyCode);
}