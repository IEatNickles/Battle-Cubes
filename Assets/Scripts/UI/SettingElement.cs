using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SettingElement : MonoBehaviour
{
    MainMenu menu;

	[SerializeField] SettingType _type;
    
    [Space]

    [SerializeField] string _settingName;
    
    [Space]

    [SerializeField] float _value;
    [SerializeField] string _valueFormat;
    [SerializeField] TMP_Text _valueText;

    [Space]

	[SerializeField] Slider _slider;
	[SerializeField] Toggle _toggle;
	[SerializeField] TMP_Dropdown _dropdown;
	[SerializeField] TMP_InputField _inputField;

    [Space]

    [SerializeField] SettingsType _settingsType;

	public void Start()
	{
        menu = MainMenu.Instance;
        GetValue();

		menu.OnSettingsChanged += GetValue;
	}

	public void OnEnable()
    {
		if (menu)
		{
			menu.OnSettingsChanged += GetValue; 
		}
    }

	public void OnDisable()
    {
		if (menu)
		{
			menu.OnSettingsChanged -= GetValue; 
		}
    }

	public void SetValue()
	{
		switch (_type)
		{
            case SettingType.Slider:
                _value = _slider.value;

                menu.SetSetting(_settingsType, _settingName, _value);

                if (_inputField)
                {
                    _inputField.text = _value.ToString(_valueFormat);
                }
                if (_valueText)
                {
                    _valueText.SetText($"{name} : {_value.ToString(_valueFormat)}");
				}
                break;
            case SettingType.Toggle:
                _value = _toggle.isOn ? 1 : 0;

				menu.SetSetting(_settingsType, _settingName, _toggle.isOn);

				if (_valueText)
                {
                    _valueText.SetText($"{name} : {(_value == 0 ? "False" : "True")}");
                }
                break;
            case SettingType.Dropdown:
                _value = _dropdown.value;

				menu.SetSetting(_settingsType, _settingName, (int)_value);

				if (_valueText)
                {
                    _valueText.SetText($"{name} : {_dropdown.options[(int)_value].text}");
                }
                break;
            case SettingType.InputField:
                float.TryParse(_inputField.text, out _value);

				menu.SetSetting(_settingsType, _settingName, _value);

				if (_slider)
				{
					_slider.value = _value;
                }
                if (_valueText)
                {
                    _valueText.SetText($"{name} : {_value.ToString(_valueFormat)}");
                }
                break;
		}
	}

    public void GetValue()
    {
        _value = menu.GetSetting(_settingsType, _settingName);

        switch (_type)
        {
            case SettingType.Slider:
                _slider.value = _value;
                if (_valueText)
                {
                    _valueText.SetText($"{name} : {_value.ToString(_valueFormat)}");
                }
                break;
            case SettingType.Toggle:
                _toggle.isOn = _value == 1;
                if (_valueText)
                {
                    _valueText.SetText($"{name} : {(_value == 0 ? "False" : "True")}");
                }
                break;
            case SettingType.Dropdown:
                _dropdown.value = (int)_value;
                if (_valueText)
                {
                    _valueText.SetText($"{name} : {_dropdown.options[(int)_value].text}");
                }
                break;
            case SettingType.InputField:
                _inputField.text = _value.ToString(_valueFormat);
                if (_valueText)
                {
                    _valueText.SetText($"{name} : {_value.ToString(_valueFormat)}");
                }
                break;
        }
    }
}
public enum SettingType
{
    Slider,
    Toggle,
    Dropdown,
    InputField
}
