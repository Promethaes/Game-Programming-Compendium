using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] float downtime = 0.25f;
    public UnityEvent OnChangeWeapon;
    public UnityEvent OnBeginAttack;
    public UnityEvent OnFinishAttack;

    [Header("References")]
    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    [SerializeField] GameObject ownerEntity = null;
    [HideInInspector] public bool canAttack = true;

    Weapon _currentWeapon = null;
    float _internalDowntime = 0.0f;

    int _weaponIndex = 0;

    private void Awake()
    {
        foreach (var w in weapons)
            w.weaponOwner = ownerEntity;

        _weaponIndex = 0;
        _currentWeapon = weapons[0];
    }

    private void Update()
    {
        _internalDowntime += Time.deltaTime;
        if (_internalDowntime >= downtime)
            OnFinishAttack.Invoke();
    }

    public void OnAttack()
    {
        if (!_currentWeapon.CanAttack())
            return;
        _internalDowntime = 0.0f;
        OnBeginAttack.Invoke();
        _currentWeapon.Attack();
    }

    public bool CanAttack()
    {
        return _currentWeapon.CanAttack();
    }

    public void SetWeapon(int index)
    {
        if (index > weapons.Count - 1 || index < 0)
            return;

        _currentWeapon = weapons[_weaponIndex];
        OnChangeWeapon.Invoke();
    }

    public void ScrollWeaponUp()
    {
        SetWeapon((_weaponIndex + 1) % weapons.Count);
    }

    public void ScrollWeaponDown()
    {
        SetWeapon((_weaponIndex - 1) % weapons.Count);
    }
}
