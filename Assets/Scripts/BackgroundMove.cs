using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector3 lerp = new Vector3((-Input.mousePosition.x + (Screen.width * 0.5f)) / Screen.width, (-Input.mousePosition.y + (Screen.height * 0.5f)) / Screen.height, transform.parent.position.z);
        transform.position = Vector3.Lerp(transform.position, lerp, Time.deltaTime * speed);
    }
}
