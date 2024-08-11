using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    public Color RGBColor
    {
        get
        {
            Color color = new Color(_redSlider.value, _blueSlider.value, _greenSlider.value);

			Color.RGBToHSV(color, out float h, out float s, out float v);
            
			_hueSlider.SetValueWithoutNotify(h);
			_saturationSlider.SetValueWithoutNotify(s);
			_valueSlider.SetValueWithoutNotify(v);

			color.a = _alphaSlider.value;

			return color;
		}
        set
        {
            _redSlider.value = value.r;
            _greenSlider.value = value.g;
            _blueSlider.value = value.b;
            _alphaSlider.value = value.a;
		}
	}
    public Color HSVColor
    {
        get
        {
            Color color = Color.HSVToRGB(_hueSlider.value, _saturationSlider.value, _valueSlider.value);

			_redSlider.SetValueWithoutNotify(color.r);
			_greenSlider.SetValueWithoutNotify(color.g);
			_blueSlider.SetValueWithoutNotify(color.b);

            color.a = _alphaSlider.value;

			return color;
		}
        private set
		{
            Color.RGBToHSV(RGBColor, out float h, out float s, out float v);

			_hueSlider.value = h;
			_saturationSlider.value = s;
			_valueSlider.value = v;
            _alphaSlider.value = value.a;
		}
    }

    public float Metalic
    {
        get => _metalicSlider.value;
    }
    public float Roughness
    {
        get => _roughnessSlider.value;
    }

    [SerializeField] Color _startColor;

    [Space]

    [SerializeField] Image _opaquePreview;
    [SerializeField] Image _transparentPreview;

    [Space]

    [SerializeField] Slider _redSlider;
    [SerializeField] Slider _blueSlider;
    [SerializeField] Slider _greenSlider;
    [SerializeField] Slider _hueSlider;
    [SerializeField] Slider _saturationSlider;
    [SerializeField] Slider _valueSlider;
    [SerializeField] Slider _alphaSlider;

    [Space]

	[SerializeField] Slider _metalicSlider;
	[SerializeField] Slider _roughnessSlider;

	public UnityEvent<Color> OnColorChanged;
    public UnityEvent<float, float> OnSurfaceChanged;

	void Awake()
	{
        OnColorChanged.AddListener(color => _opaquePreview.color = new Color(color.r, color.g, color.b));
        OnColorChanged.AddListener(color => _transparentPreview.color = color);

        _redSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(RGBColor));
        _blueSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(RGBColor));
        _greenSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(RGBColor));

        _hueSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(HSVColor));
        _saturationSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(HSVColor));
        _valueSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(HSVColor));

        _alphaSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(RGBColor));
        _alphaSlider.onValueChanged.AddListener(value => OnColorChanged?.Invoke(HSVColor));

        _metalicSlider.onValueChanged.AddListener(value => OnSurfaceChanged?.Invoke(value, _roughnessSlider.value));
        _roughnessSlider.onValueChanged.AddListener(value => OnSurfaceChanged?.Invoke(_metalicSlider.value, value));

        RGBColor = _startColor;
	}

    public void SetHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            RGBColor = color;
            HSVColor = color;
        }
    }
}
