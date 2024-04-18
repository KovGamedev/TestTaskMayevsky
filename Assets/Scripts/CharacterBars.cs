using UnityEngine;
using UnityEngine.UI;

public class CharacterBars : MonoBehaviour
{
    [SerializeField] private Slider _armorBar;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Color _allyHealthColor;
    [SerializeField] private Color _enemyHealthColor;

    public void ResetBars(bool isAlly)
    {
        _armorBar.value = 1f;
        _healthBar.value = 1f;
        _healthBar.fillRect.GetComponent<Image>().color = isAlly ? _allyHealthColor : _enemyHealthColor;
    }
}
