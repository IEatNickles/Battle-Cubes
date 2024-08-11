using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public float speed;
	public float jumpForce;

	Rigidbody rb;

	[Space]

	public Health health;
	public Bounds attackBounds;

	GameManager gm;

	[Space]

	public List<PlayerMovement> players;
	PlayerMovement target;

	float horizontal;

	[Space]

	public ParticleSystem jumpEffect;
	public float jumpDelay = 0.5f;
	bool canJump = true;

	[Space]

	bool canAttack;
	public float attackSpeed = 0.5f;

    private void Start()
	{
		canJump = true;
		canAttack = true;
		rb = GetComponent<Rigidbody>();
		gm = GameManager.instance;
		players = new List<PlayerMovement>(gm.activePlayers);

		for (int i = 0; i < players.Count; i++)
		{
			players[i].health.OnDeath.AddListener(h => PlayerDeath(players[i]));
		}

		GetTarget();
	}

	void PlayerDeath(PlayerMovement pm)
	{
		players.Remove(pm);
		GetTarget();
	}

	void GetTarget()
	{
		if (players.Count <= 0) return;

		target = players[Random.Range(0, players.Count)];
	}

	void Update()
    {
		if (target != null)
		{
			horizontal = (target.transform.position - transform.position).normalized.x;
			rb.AddForce(new Vector3(horizontal, 0f, 0f) * speed);

			if (target.transform.position.y > transform.position.y	&& canJump)
				Jump();

			if (canAttack && Vector3.Distance(transform.position, target.transform.position) <= 2f)
			{
				Attack(attackBounds);
			}
		}

		if (gm.score >= short.MaxValue)
			health.Die();
    }

	public void LateUpdate()
	{
		Bounds bounds = gm.bounds;
		if (bounds.PointInBounds(transform.position + (rb.velocity.normalized * 0.5f)))
		{
			rb.velocity = Vector3.zero;

			float x = (bounds.size.x * 0.5f) + bounds.center.x - 0.5f;
			float y = (bounds.size.y * 0.5f) + bounds.center.y - 0.5f;
			float z = (bounds.size.z * 0.5f) + bounds.center.z - 0.5f;

			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, -x, x),
				Mathf.Clamp(transform.position.y, -y, y),
				Mathf.Clamp(transform.position.z, -z, z));
		}
	}

	void Jump()
	{
		rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		jumpEffect.Play();
		canJump = false;
		Invoke("ResetJump", Random.Range(jumpDelay - 0.1f, jumpDelay + 0.1f));
	}

	void ResetJump()
	{
		canJump = true;
	}

	void Attack(Bounds bounds)
	{
		health.Attack(bounds);
		StartCoroutine(AttackC());
	}

	IEnumerator AttackC()
	{
		canAttack = false;

		yield return new WaitForSeconds(attackSpeed);

		canAttack = true;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, attackBounds.size);
	}
}
