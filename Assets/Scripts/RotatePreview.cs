using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatePreview : MonoBehaviour
{
	public float speed = 0.45f;

    Vector3 prevPos;
    Vector3 posDelta;

	bool clicking;
	GameObject go;

	Camera cam;

	public void Awake()
	{
		cam = Camera.main;
	}

	public void OnMouseDown()
	{
		prevPos = Input.mousePosition;
		clicking = true;
		go = gameObject;
	}

	public void OnMouseDrag()
	{
		if (clicking)
		{
			posDelta = Input.mousePosition - prevPos;
			float dot = Vector3.Dot(posDelta, cam.transform.right) * speed;
			go.transform.Rotate(cam.transform.up, -dot, Space.World);

			dot = Vector3.Dot(posDelta, cam.transform.up) * speed;
			go.transform.Rotate(transform.right, dot, Space.World);

			prevPos = Input.mousePosition;
		}
	}

	public void OnMouseUp()
	{
		go.transform.DORotate(Vector3.zero, 0.25f).SetEase(Ease.OutSine);
		clicking = false;
		go = null;
	}

	/*void Update()
    {
		if (Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) || clicking)
			{
				if(hit.collider != null)
				{
					go = hit.collider.gameObject;
				}
				if (go == gameObject || go.transform.IsChildOf(transform))
				{
					posDelta = Input.mousePosition - prevPos;
					float dot = Vector3.Dot(posDelta, Camera.main.transform.right) * speed;
					transform.Rotate(Camera.main.transform.up, -dot, Space.World);

					dot = Vector3.Dot(posDelta, Camera.main.transform.up) * speed;
					transform.Rotate(transform.right, dot, Space.World);

					clicking = true;
				}
			}
		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);
			go = null;
			clicking = false;
		}

        prevPos = Input.mousePosition;
    }*/
}
