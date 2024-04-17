using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObjects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] private float _damage;
    [SerializeField] private float _accuracy;
    [SerializeField] private float _fireRate;
    [SerializeField] private uint _clipSize;
    [SerializeField] private float _reloadTime;

    public float GetDamage() => _damage;

    public float GetAccuracy() => _accuracy;

    public float GetFireRate() => _fireRate;

    public uint GetClipSize() => _clipSize;

    public float GetReloadTime() => _reloadTime;
}
