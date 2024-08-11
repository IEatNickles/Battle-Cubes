using System.Text;
using UnityEngine;

public class DefaultSkin : SkinObject
{
	public override void Initialize()
	{
	}

	[ContextMenu("Name From Object Name")]
	public void NameFromObjectName()
	{
		Name = AddSpacesBeforeCapitals(name, true);
	}

	string AddSpacesBeforeCapitals(string text, bool keepAcronyms)
	{
		StringBuilder newText = new StringBuilder(text.Length * 2);
		newText.Append(text[0]);

		for (int i = 1; i < text.Length; i++)
		{
			if (char.IsUpper(text[i]))
			{
				if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
					(keepAcronyms && char.IsUpper(text[i - 1])) &&
					i < text.Length - 1 && char.IsLower(text[i + 1]))
				{
					newText.Append(' ');
				}
			}
			newText.Append(text[i]);
		}

		return newText.ToString();
	}
}