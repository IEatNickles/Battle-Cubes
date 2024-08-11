using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class UITabButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public bool selected;
	public TabGroup group;

	[Space]

	public GameObject tab;
	public bool interactable = true;

	[Space]

	Image background;
	public Color selectedColor;
	public Color deselectedColor;
	public Color hoverColor;

	public UnityEvent OnSelected;
	public UnityEvent OnDeselected;

	public void Awake()
	{
		background = GetComponent<Image>();
		//group.Subscribe(this);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (interactable)
		{
			group.OpenTab(this);
		}
	}

	public void Select()
	{
		selected = true;
		background.color = selectedColor;
		tab.SetActive(true);
	}

	public void Deselect()
	{
		selected = false;
		background.color = deselectedColor;
		tab.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (interactable)
		{
			background.color = hoverColor;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (interactable)
		{
			background.color = !selected ? deselectedColor : selectedColor;
		}
	}
}
