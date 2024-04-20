using UnityEngine;
using UnityEngine.UI;

public class CharacterBars : MonoBehaviour
{
    [SerializeField] private Slider _armorBar;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Color _allyHealthColor;
    [SerializeField] private Color _enemyHealthColor;

    public void ResetBars(Faction faction)
    {
        _armorBar.value = 1f;
        _healthBar.value = 1f;
        _healthBar.fillRect.GetComponent<Image>().color = faction == Faction.Ally ? _allyHealthColor : _enemyHealthColor;
    }

    public void SetValues(float armor, float health)
    {
        _armorBar.value = armor;
        _healthBar.value = health;
        if (health <= 0f)
            gameObject.SetActive(false);
    }

    public void RotateToCamera()
    {
        var relativePositoin = transform.position - Camera.main.transform.position;
        var targetRotation = Quaternion.LookRotation(relativePositoin, Vector3.up);
        transform.eulerAngles = new Vector3(targetRotation.eulerAngles.x, 0, targetRotation.eulerAngles.z);
    }
}
