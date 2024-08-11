using UnityEngine.EventSystems;
using UnityEngine;

public class Tab : MonoBehaviour
{
	[SerializeField] GameObject _firstSelected;
    EventSystem es;

	public void Awake()
	{
		es = EventSystem.current;
	}

	public void OnEnable()
	{
		if(_firstSelected)
			es.SetSelectedGameObject(_firstSelected);
	}
}
