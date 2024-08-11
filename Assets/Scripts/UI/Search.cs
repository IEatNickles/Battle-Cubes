using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Search : MonoBehaviour
{
    public GameObject button;
	public string[] list;
	public Transform results;
	public CanvasGroup resultArea;
	List<GameObject> itemButtons = new List<GameObject>();

	public void Start()
	{
		DisableResultArea();
	}

	public void SearchForItem(string name)
	{
		ResetResults();

		for (int i = 0; i < list.Length; i++)
		{
			if (list[i].ToLower().Contains(name.ToLower()))
			{
				itemButtons[i].SetActive(true);
			}
		}
	}

	public void Populate(string[] list)
	{
		this.list = list;
		for (int i = 0; i < list.Length; i++)
		{
			GameObject item = Instantiate(button, results);
			item.name = list[i];
			item.GetComponentInChildren<TMP_Text>().SetText(list[i]);
			itemButtons.Add(item);
			item.SetActive(false);
		}
	}

	public void Populate(string[] list, UnityAction[] actions)
	{
		this.list = list;
		for (int i = 0; i < list.Length; i++)
		{
			GameObject item = Instantiate(button, results);
			item.GetComponent<Button>().onClick.AddListener(actions[i]);
			item.name = list[i];
			item.GetComponentInChildren<TMP_Text>().SetText(list[i]);
			itemButtons.Add(item);
			item.SetActive(false);
		}
	}

	public void EnableResultArea()
	{
		resultArea.DOFade(1f, 0.25f);
		resultArea.blocksRaycasts = true;
	}

	public void DisableResultArea()
	{
		resultArea.DOFade(0f, 1f);
		resultArea.blocksRaycasts = false;
	}

	void ResetResults()
	{
		for (int i = 0; i < itemButtons.Count; i++)
		{
			itemButtons[i].SetActive(false);
		}
	}
}
