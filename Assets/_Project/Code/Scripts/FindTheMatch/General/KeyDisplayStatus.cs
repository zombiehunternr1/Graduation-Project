using UnityEngine;

public class KeyDisplayStatus : MonoBehaviour
{
    [SerializeField] Animator _keyAnimator;
    public void DisplayStatus(string animation)
    {
        _keyAnimator.Play(animation);
    }
}
