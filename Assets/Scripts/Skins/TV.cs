using UnityEngine;

public class TV : SkinObject
{
    public Renderer dvdVideoObj;

    public Material dvdMat;
    public Material rgbMat;

    public float width = 0.36785f;
    public float height = 0.4185f;

    public float xSpeed = 15f;
    public float ySpeed = 15f;

	public override void Initialize()
    {
        dvdVideoObj.material.color = Color.white;
        dvdVideoObj.material = dvdMat;
    }

	void Update()
    {
        Vector3 lerp = new Vector3(dvdVideoObj.transform.localPosition.x + xSpeed, 0f, dvdVideoObj.transform.localPosition.z + ySpeed);
        dvdVideoObj.transform.localPosition = lerp;

        if(dvdVideoObj.transform.localPosition.x >= width || dvdVideoObj.transform.localPosition.x <= -width)
		{
            xSpeed = -xSpeed;
            dvdVideoObj.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            dvdVideoObj.material = dvdMat;
            if (dvdVideoObj.transform.localPosition.z >= height)
            {
                dvdVideoObj.material = rgbMat;
            }
        }
        if(dvdVideoObj.transform.localPosition.z >= height || dvdVideoObj.transform.localPosition.z <= -height)
		{
            ySpeed = -ySpeed;
            dvdVideoObj.material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            dvdVideoObj.material = dvdMat;
            if (dvdVideoObj.transform.localPosition.x >= width)
            {
                dvdVideoObj.material = rgbMat;
            }
        }
    }
}
