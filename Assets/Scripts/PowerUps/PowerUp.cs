using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public abstract class PowerUp : MonoBehaviour
{
	GameManager gm;
	public bool inUse { get; private set; }
	public float duration;
	public float lifeTime;
	public AudioClip activationSound;
	public AudioClip deactivationSound;
	AudioSource aud;

	public void Awake()
	{
		gm = GameManager.instance;
		aud = GetComponent<AudioSource>();
		DOVirtual.DelayedCall(lifeTime, () =>
		{
			transform.DOScale(Vector3.zero, 0.25f).OnComplete(() => Destroy(gameObject));
		});
	}

	public void Activate(PlayerMovement user)
	{
		if (!inUse)
		{
			gm.SetTimer(duration);
			OnActivate(user);
			aud.PlayOneShot(activationSound);
			inUse = true;
			transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InExpo);
			DOVirtual.DelayedCall(duration, () => Deactivate(user));
		}
	}

	void Deactivate(PlayerMovement user)
	{
		if(deactivationSound) aud.PlayOneShot(deactivationSound);
		Destroy(gameObject, deactivationSound ? deactivationSound.length : 0f);
		OnDeactivate(user);
	}

	protected abstract void OnActivate(PlayerMovement user);
	protected abstract void OnDeactivate(PlayerMovement user);
}
