using System;
using System.IO;
using UnityEngine.Events;
using UnityEngine;
using MilkShake;

public class Health : MonoBehaviour
{
	public UnityEvent<Health> OnDeath;
	public bool endless;

	[Space]

	public int maxHealth = 100;
    public int currentHealth;
	public ProgressBar healthBar;

	[Space]

	public bool isPlayer;

	AudioSource aud;

	[Space]

	public AudioClip[] hurtSounds;
	public AudioClip[] oldHurtSounds;
	public AudioClip rareSound;
	public float rareSoundChance;

	[Space]

	public ParticleSystem hurtParticles;
	public ParticleSystem dieParticles;

	[Space]

	public Shaker shaker;
	public ShakePreset shakePreset;

	[Space]

	public LayerMask enemy = 1 >> 10 >> 9;
	public int damage;

	PlayerMovement pm;

	MenuSettings settings;

	bool useDeathParticles;
	bool useHurtParticles;
	bool useOldSounds;

	private void Start()
	{
		SetupValues();
	}

	public void SetupValues()
	{
		settings = FileWriter.ReadFromJson<MenuSettings>
			($"{Application.persistentDataPath}/Settings/settings.{MainMenu.SETTINGS_EXTENTION}");

		useDeathParticles = settings.DeathParticles;
		useHurtParticles = settings.HurtParticles;
		useOldSounds = settings.OldSounds;

		if(isPlayer)
		{
			pm = transform.root.GetComponent<PlayerMovement>();

			hurtParticles = transform.Find("Effects/Hurt").GetComponent<ParticleSystem>();
			dieParticles = transform.Find("Effects/Die").GetComponent<ParticleSystem>();

			if (PlayerPrefs.GetInt("Character" + pm.pInd) == 25)
			{
				maxHealth = 10;
				pm.speed = 1f;
				pm.attackSpeed = 1f;
			}
		}

		shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shaker>();

		aud = GetComponent<AudioSource>();

		currentHealth = maxHealth;
		healthBar.SetMax(maxHealth);
		TakeDamage(0);
	}

	public void Attack(Bounds bounds)
	{
		Collider[] cols = Physics.OverlapBox(transform.position, bounds.size, Quaternion.identity, enemy);
		foreach (Collider c in cols)
		{
			if (c.transform != transform && ((c.CompareTag("Player") && ((endless && !transform.CompareTag("Player")) || !endless)) || (c.CompareTag("Enemy") && !transform.CompareTag("Enemy"))))
				c.GetComponent<Health>().TakeDamage(damage);
		}
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		healthBar.SetValue(currentHealth);
		if (currentHealth <= 0)
		{
			Die();
		}

		if (shaker != null)
		{
			shaker.Shake(shakePreset);
		}

		aud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		int chance = (int)UnityEngine.Random.Range(0, rareSoundChance);
		if (chance > 0)
		{
			if (hurtSounds != null && aud != null)
			{
				if (!useOldSounds)
				{
					aud.PlayOneShot(hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)]);
				}
				else
				{
					aud.PlayOneShot(oldHurtSounds[UnityEngine.Random.Range(0, oldHurtSounds.Length)]);
				}
			} 
		}
		else if (chance == 0)
		{
			aud.PlayOneShot(rareSound);
		}

		if (useHurtParticles)
		{
			ParticleSystem hurt = Instantiate(hurtParticles, transform.position, Quaternion.identity);

			Destroy(hurt.gameObject, 1f);
		}
	}

	public void Die()
	{
		Destroy(gameObject);

		if (useDeathParticles)
		{
			ParticleSystem die = Instantiate(dieParticles, transform.position, Quaternion.identity);

			Destroy(die.gameObject, 5f);
		}

		OnDeath?.Invoke(this);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Attack"))
		{
			TakeDamage(5);
		}
	}
}
