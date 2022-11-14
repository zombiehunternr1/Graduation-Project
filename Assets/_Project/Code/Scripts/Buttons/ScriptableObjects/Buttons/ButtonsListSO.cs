using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ButtonList", menuName ="Scriptable Objects/Buttons/Button list")]
public class ButtonsListSO : ScriptableObject
{
    [SerializeField] private List<ButtonSO> _buttons;
    public List<ButtonSO> buttonList
    {
        get
        {
            return _buttons;
        }
    }
}
