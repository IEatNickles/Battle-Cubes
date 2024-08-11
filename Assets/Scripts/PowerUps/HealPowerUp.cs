using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : PowerUp
{
	public int healAmount = 100;

	protected override void OnActivate(PlayerMovement user)
	{
		user.health.TakeDamage(-healAmount);
	}

	protected override void OnDeactivate(PlayerMovement user)
	{

	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			PlayerMovement user = collision.gameObject.GetComponent<PlayerMovement>();
			if (user.health.currentHealth < user.health.maxHealth)
				Activate(user);
		}
	}
}
