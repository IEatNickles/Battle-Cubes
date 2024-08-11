using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ProgressBar : MonoBehaviour
{
	public RectTransform rectTransform;

	public TMP_Text valueText;
    public string valueFormat;
    float previousValue;

	public float width
	{
		get
		{
			return rectTransform.rect.width;
		}
	}
	public float height
	{
		get
		{
			return rectTransform.rect.height;
		}
	}


	[Space]

    [Min(0), SerializeField] float value;

    [Space]

    [Min(0), SerializeField] float min;
    [Min(0), SerializeField] float max;

    [Space]

    public Image fill;

    public UnityEvent OnValueChanged;

	public void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnValidate()
	{
        LateUpdate();
	}
    
	public void LateUpdate()
	{
        if(previousValue != value)
		{
            value = Mathf.Clamp(value, min, max);
            fill.fillAmount = Mathf.InverseLerp(min, max, value);
			if (valueText)
			{
                valueText.SetText(string.Format(valueFormat, value, min, max));
			}
		}

        previousValue = value;
	}

    public void SetValue(float value)
	{
        value = Mathf.Clamp(value, min, max);
        this.value = value;
	}
    public float GetValue()
	{
        return value;
	}
    public float GetNormalizedValue()
	{
        return Mathf.InverseLerp(min, max, value);
	}

    public void SetMax(float max)
	{
        this.max = max;
	}
    public void SetMin(float min)
	{
        this.min = min;
	}
}
