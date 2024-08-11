using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] GameObject _window;

    [Space]

	[SerializeField] Button _confirm;
	[SerializeField] Button _decline;
	[SerializeField] Button _alt;

    [Space]

    public UnityEvent OnShow;
    public UnityEvent OnConfirm;
    public UnityEvent OnDecline;
    public UnityEvent OnAlt;

	public void Awake()
	{
        _confirm?.onClick.AddListener(() => OnConfirm?.Invoke());
        _decline?.onClick.AddListener(() => OnDecline?.Invoke());
        _alt?.onClick.AddListener(() => OnAlt?.Invoke());
	}

	public void Show()
	{
        OnShow?.Invoke();
        _window.SetActive(true);
	}

    public void Hide()
	{
        _window.SetActive(false);
    }
}
