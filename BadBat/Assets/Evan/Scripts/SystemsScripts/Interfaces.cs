using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbility {
	public void Equip(GameObject player, GameObject gauntlet);
	public void Fire();
	public void UnEquip();
}

public interface IDamageable {
	public void TakeDamage(int amt);
}

public interface IElectrical {
	public void Surge();
}

public interface IConcuss {
	public void Concuss(float length);
}

public interface IFreezeable
{
	public void Freeze();
}
