using UnityEngine;

public class CustomSkinObject : SkinObject
{
	CustomSkin _skin;
    
    public override void Initialize()
    {
        _skin = FileWriter.ReadFromJson<CustomSkin>(SkinCreator.GetSkinPath(Name));

        for (int i = 0; i < 8; i++)
        {
            SkinPart part = _skin.GetSkinPart((PartCategory)i);

            if (part.Index >= 0)
            {
                GameObject newPart = 
                    Instantiate(Resources.LoadAll<GameObject>($"CustomParts/Parts/{(PartCategory)i}")[part.Index], transform);
                newPart.transform.localPosition = Vector3.zero;
                newPart.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
