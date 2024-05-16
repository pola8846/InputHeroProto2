using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public interface IMoveReceiver
{
    public void KeyDown(InputType inputType);
    public void KeyUp(InputType inputType);
    public void KeyReset();
}