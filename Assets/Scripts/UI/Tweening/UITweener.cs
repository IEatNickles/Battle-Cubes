using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using UnityEngine.Events;
using DG.Tweening.Core;
using UnityEngine;
using DG.Tweening;

public class UITweener : MonoBehaviour
{
    public GameObject objectToAnimate;
    CanvasGroup group;
    public TweenType tweenType;
    public Ease ease;
    public float duration;
    public float delay;
    public int loops;
    public LoopType loopType;

	public Space space;

    public Vector3 start;
    public Vector3 end;

    public bool showOnEnable;
	public bool restartOnShow;
	public bool disableOnHide;

	public UnityEvent OnComplete;

	RectTransform rectTransform;

	public void Awake()
	{
		rectTransform = objectToAnimate.GetComponent<RectTransform>();

		if (tweenType == TweenType.Fade)
		{
			group = objectToAnimate.GetComponent<CanvasGroup>();
			if (group == null) group = objectToAnimate.AddComponent<CanvasGroup>();
		}
	}

	public void OnEnable()
	{
		if (showOnEnable)
		{
			Show();
		}
	}

    public void Show()
	{
		objectToAnimate.SetActive(true);

        HandleTween();
	}

    void HandleTween(Action callback = null)
	{
		if (restartOnShow)
		{
			switch (tweenType)
			{
				case TweenType.Fade:
					group.alpha = start.x;
					break;
				case TweenType.Scale:
					transform.localScale = start;
					break;
				case TweenType.Move:
					if(space == Space.World)
						transform.position = start;
					else
						transform.localPosition = start;
					break;
				case TweenType.Rotate:
					if (space == Space.World)
						transform.rotation = Quaternion.Euler(start);
					else
						transform.localRotation = Quaternion.Euler(start);
					break;
			}
		}

		switch (tweenType)
		{
            case TweenType.Fade:
				Fade().OnComplete(() => callback());
                break;
			case TweenType.Scale:
				Scale().OnComplete(() => callback());
				break;
			case TweenType.Move:
				Move().OnComplete(() => callback());
				break;
			case TweenType.Rotate:
				Rotate().OnComplete(() => callback());
				break;
		}
	}

	TweenerCore<float, float, FloatOptions> Fade()
	{
		return group.DOFade(end.x, duration).SetDelay(delay).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => OnComplete?.Invoke());
	}

	TweenerCore<Vector3, Vector3, VectorOptions> Scale()
	{
		return rectTransform.DOScale(end, duration).SetDelay(delay).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => OnComplete?.Invoke());
	}

	TweenerCore<Vector2, Vector2, VectorOptions> Move()
	{
		//if (space == Space.World)
			return rectTransform.DOAnchorPos(end, duration).SetDelay(delay).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => OnComplete?.Invoke());
		/*else
			return rectTransform.DOAnchorPos(end, duration).SetDelay(delay).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => OnComplete?.Invoke());*/
	}

	TweenerCore<Quaternion, Vector3, QuaternionOptions> Rotate()
	{
		if(space == Space.World)
			return rectTransform.DORotate(end, duration).SetDelay(delay).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => OnComplete?.Invoke());
		else
			return rectTransform.DOLocalRotate(end, duration).SetDelay(delay).SetEase(ease).SetLoops(loops, loopType).OnComplete(() => OnComplete?.Invoke());
	}

	void Flip()
	{
		Vector3 flip = start;
		start = end;
		end = flip;
	}

	public void Hide()
	{
		Flip();

		HandleTween(() => { if(disableOnHide) objectToAnimate.SetActive(false); Flip(); });
	}
}
public enum TweenType
{
    Fade,
    Scale,
    Move,
    Rotate
}
