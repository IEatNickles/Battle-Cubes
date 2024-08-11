using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TabGroup : MonoBehaviour
{
	EventSystem _es;
	
	[SerializeField] UITabButton _firstSelected;

	[SerializeField] InputAction _tabNext;
	[SerializeField] InputAction _tabPrevious;

	[SerializeField] List<UITabButton> _tabs = new List<UITabButton>();
	int _currentTabIndex;

	public void Start()
	{
		_es = EventSystem.current;

		OpenTab(_firstSelected);
	}

	void OnEnable()
	{
		_tabNext.Enable();
		_tabPrevious.Enable();

		_tabNext.performed += TabNext;
		_tabPrevious.performed += TabPrevious;
	}

	void OnDisable()
	{
		_tabNext.performed -= TabNext;
		_tabPrevious.performed -= TabPrevious;
	}

	public void Subscribe(UITabButton tab)
	{
		_tabs.Add(tab);
	}

	void TabPrevious(InputAction.CallbackContext context)
	{
		if (!CanTab()) return;

		if (_currentTabIndex > 0)
		{
			OpenTab(_tabs[_currentTabIndex - 1]);
		}
		else
		{
			OpenTab(_tabs[_tabs.Count - 1]);
		}
	}
	void TabNext(InputAction.CallbackContext context)
	{
		if (!CanTab()) return;

		if (_currentTabIndex < _tabs.Count - 1)
		{
			OpenTab(_tabs[_currentTabIndex + 1]);
		}
		else
		{
			OpenTab(_tabs[0]);
		}
	}

	bool CanTab()
	{
		if (!_es.currentSelectedGameObject) return true;

		if (_es.currentSelectedGameObject.TryGetComponent(out TMP_InputField field) && field.isFocused) return false;

		return true;
	}

	public void OpenTab(UITabButton tab)
	{
		foreach(UITabButton t in _tabs)
		{
			if (t != tab) t.Deselect();
		}
		tab.Select();
		_currentTabIndex = _tabs.IndexOf(tab);
	}
}
