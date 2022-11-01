using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ButtonTesterList", menuName ="Scriptable Objects/Testing/Button Tester list")]
public class ButtonsListSO : ScriptableObject
{
    [SerializeField] private List<ButtonSO> _buttons;
    public List<ButtonSO> Buttons
    {
        get
        {
            return _buttons;
        }
    }
}
