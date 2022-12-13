using UnityEngine;
[CreateAssetMenu(fileName ="StarInfoSO", menuName ="Scriptable Objects/First Puzzle/Star info")]
public class StarInfoSO : ScriptableObject
{
    [SerializeField] private bool _isSelected;
    public bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
        }
    }
}
