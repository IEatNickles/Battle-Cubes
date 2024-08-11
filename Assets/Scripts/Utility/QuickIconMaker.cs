using System.Collections;
using ThumbCreator.Core;
using ThumbCreator.Helpers;
using UnityEngine;

public class QuickIconMaker : MonoBehaviour
{
    [SerializeField] ThumbManager _thumbManager;
	[SerializeField] GameObject[] _iconObjects;

	IEnumerator Start()
	{
		foreach (GameObject iconObject in _iconObjects) iconObject.SetActive(false);

		foreach (GameObject iconObject in _iconObjects)
		{
			iconObject.SetActive(true);

			string fileName = "Icons/";
			if(iconObject.transform.parent != null)
			{
				fileName += iconObject.transform.parent.name + "/";
			}

			Screenshot.GeneratePng(fileName + iconObject.name, 
				(int)_thumbManager.ResolutionWidth, (int)_thumbManager.ResolutionHeight, 
				directory:_thumbManager.FileDirectory);

			yield return new WaitForEndOfFrame();

			iconObject.SetActive(false);
		}
	}
}
