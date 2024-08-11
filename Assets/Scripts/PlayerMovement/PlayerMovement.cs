using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
	GameManager gm;

    public float speed;
    public float jumpHeight;

	public float attackSpeed = 1f;
	bool canAttack;

	Rigidbody rb;
	BoxCollider bc;

	[Space]

	public int pInd;

	[Space]

	public Bounds attackBounds;

	[Space]

	public TMP_Text playerName;
	public SkinsSO skins;

	Transform graphics;

	Vector3 direction;

	[Space]

	public Health health;

	[Space]

	public ParticleSystem jumpFX;

	private void Start()
	{
		AsignVariables();
	}

	public void Update()
	{
		direction = new Vector3(Input.GetAxis("Horizontal" + pInd), 0f, 0f);

		if (canAttack)
		{
			if (Input.GetButtonDown("Attack" + pInd))
			{
				Attack(attackBounds);
			}
		}

		if (Input.GetButtonDown("Jump" + pInd))
		{
			Jump();
		}

		if (Input.GetButtonDown("Crouch" + pInd))
		{
			Crouch();
		}
		else if (Input.GetButtonUp("Crouch" + pInd))
		{
			StopCrouch();
		}
	}

	public void FixedUpdate()
	{
		Move();
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

	void AsignVariables()
	{
		gm = GameManager.instance;

		playerName.SetText(PlayerPrefs.GetString("Gamertag" + pInd));
		if (string.IsNullOrEmpty(playerName.text)) playerName.text = skins.Skins[PlayerPrefs.GetInt("Character" + pInd)].Name;

		graphics = transform.GetChild(transform.childCount - 1);
		rb = GetComponent<Rigidbody>();
		bc = GetComponent<BoxCollider>();

		health.healthBar.rectTransform.anchoredPosition = new Vector2(15f, -15f - ((15 + health.healthBar.height) * pInd));

		canAttack = true;
	}

	void Jump()
	{
		rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
		jumpFX.Play();
	}

	void Crouch()
	{
		graphics.localScale = new Vector3(1f, 1f, 0.5f);
		bc.size = new Vector3(1f, 0.5f, 1f);
	}

	void StopCrouch()
	{
		graphics.localScale = new Vector3(1f, 1f, 1f);
		bc.size = Vector3.one;
	}

	void Move()
	{
		rb.AddForce(direction * speed);
	}

	void Attack(Bounds b)
	{
		health.Attack(b);
		StartCoroutine(AttackCoroutine());
	}

	IEnumerator AttackCoroutine()
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
