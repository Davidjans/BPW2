using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[System.Serializable]
struct ControlTypes
{
    public List<TMP_Text> m_Texts;
    public InputActionReference m_Action;
}
[System.Serializable]
struct SettingValues
{
    public Slider m_Slider;
    public TextMeshProUGUI m_Text;
}
public enum ActionType
{
    Rotation,
    Movement
}

public class CameraSettings : MonoBehaviour
{
    [SerializeField] private List<SettingValues> m_Settings = new List<SettingValues>();

    [SerializeField] private List<ControlTypes> m_ControlTypes = new List<ControlTypes>();
    [SerializeField] private PlayerInput m_PlayerInput;

    [SerializeField] private ActionType m_ActionType;

    [SerializeField] private GameObject m_OverwriteScreen;

    [SerializeField] private List<GameObject> m_ResumeObject = new List<GameObject>();

    private InputActionRebindingExtensions.RebindingOperation m_RebindingOperation;

    private InputBinding m_NewBindingToChange;
    private InputBinding m_BindingToOverwrite;

    private InputAction m_InputAction;

    public bool m_AreSettingsSaved;

    private int m_TypeIndex;
    private int m_OverwriteTypeIndex;
    private int m_TextIndex;
    private int m_OverwriteTextIndex;

    private void Start()
    {
        m_InputAction = new InputAction();
        m_InputAction.AddBinding();

        string _rebinds = PlayerPrefs.GetString("KeyBinds", string.Empty);

        if (!string.IsNullOrEmpty(_rebinds))
        {
            m_PlayerInput.actions.LoadFromJson(_rebinds);
        }

        for (int i = 0; i < m_ControlTypes[0].m_Texts.Count; i++)
        {
            m_ControlTypes[0].m_Texts[i].text = InputControlPath.ToHumanReadableString(m_ControlTypes[0].m_Action.action.bindings[i + 1].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        for (int i = 0; i < m_ControlTypes[1].m_Texts.Count; i++)
        {
            m_ControlTypes[1].m_Texts[i].text = InputControlPath.ToHumanReadableString(m_ControlTypes[1].m_Action.action.bindings[i + 1].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }

        //This is the only way the sliders work. Don't question it 
        m_Settings[0].m_Slider.onValueChanged.AddListener(delegate { TextChange(0); });
        m_Settings[1].m_Slider.onValueChanged.AddListener(delegate { TextChange(1); });
        m_Settings[0].m_Slider.value = PlayerPrefs.GetFloat("CameraMovementSpeed");
        m_Settings[1].m_Slider.value = PlayerPrefs.GetFloat("CameraRotationSpeed");
        TextChange(0);
        TextChange(1);
    }

    public void TextChange(int _listIndex)
    {
        m_Settings[_listIndex].m_Text.text = string.Format("{0}", m_Settings[_listIndex].m_Slider.value.ToString("0"));
    }

    public void StartRebinding(int _textIndex)
    {
        m_TextIndex = _textIndex;
        if (m_ActionType == ActionType.Rotation)
        {
            m_TypeIndex = 0;
        }
        else if (m_ActionType == ActionType.Movement)
        {
            m_TypeIndex = 1;
        }
        m_ControlTypes[m_TypeIndex].m_Texts[m_TextIndex].text = "Press any key";

        m_PlayerInput.SwitchCurrentActionMap("Menu");

        m_RebindingOperation = InputActionRebindingExtensions.PerformInteractiveRebinding(m_InputAction)
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => RebindOperating())
        .Start();

    }

    private void RebindOperating()
    {
        if (CheckForDoubleKeys())
        {
            RebindComplete();
        }
        else
        {
            AskToOverwrite();
        }
    }

    private void RebindComplete()
    {
        m_ControlTypes[m_TypeIndex].m_Action.action.ApplyBindingOverride(m_TextIndex + 1, m_InputAction.bindings[0]);

        m_ControlTypes[m_TypeIndex].m_Texts[m_TextIndex].text = InputControlPath.ToHumanReadableString(m_ControlTypes[m_TypeIndex].m_Action.action.bindings[m_TextIndex + 1].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        m_RebindingOperation.Dispose();

        m_PlayerInput.SwitchCurrentActionMap("CameraControls");
    }

    private void RebindCompleteOverwrite()
    {
        m_ControlTypes[m_TypeIndex].m_Action.action.ApplyBindingOverride(m_TextIndex + 1, m_InputAction.bindings[0]);
        m_ControlTypes[m_OverwriteTypeIndex].m_Action.action.ApplyBindingOverride(m_OverwriteTextIndex, "Unassigned");

        m_ControlTypes[m_TypeIndex].m_Texts[m_TextIndex].text = InputControlPath.ToHumanReadableString(m_ControlTypes[m_TypeIndex].m_Action.action.bindings[m_TextIndex + 1].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        m_ControlTypes[m_OverwriteTypeIndex].m_Texts[m_OverwriteTextIndex - 1].text = InputControlPath.ToHumanReadableString(m_ControlTypes[m_OverwriteTypeIndex].m_Action.action.bindings[m_OverwriteTextIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        m_RebindingOperation.Dispose();

        m_PlayerInput.SwitchCurrentActionMap("CameraControls");
    }

    private void RebindCompleteCancelOverwrite()
    {
        m_ControlTypes[m_TypeIndex].m_Texts[m_TextIndex].text = InputControlPath.ToHumanReadableString(m_ControlTypes[m_TypeIndex].m_Action.action.bindings[m_TextIndex + 1].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        m_RebindingOperation.Dispose();

        m_PlayerInput.SwitchCurrentActionMap("CameraControls");
    }

    private bool CheckForDoubleKeys()
    {
        for (int i = 0; i < m_ControlTypes[0].m_Action.action.bindings.Count; i++)
        {
            if (m_InputAction.bindings[0].effectivePath == m_ControlTypes[0].m_Action.action.bindings[i].effectivePath && m_TextIndex + 1 != i)
            {
                m_OverwriteTypeIndex = 0;
                m_OverwriteTextIndex = i;
                return false;
            }
        }
        for (int i = 0; i < m_ControlTypes[1].m_Action.action.bindings.Count; i++)
        {
            if (m_InputAction.bindings[0].effectivePath == m_ControlTypes[1].m_Action.action.bindings[i].effectivePath)
            {
                m_OverwriteTypeIndex = 1;
                m_OverwriteTextIndex = i;
                return false;
            }
        }
        return true;
    }

    private void AskToOverwrite()
    {
        m_OverwriteScreen.SetActive(true);
    }

    public void Overwrite()
    {
        RebindCompleteOverwrite();
        m_OverwriteScreen.SetActive(false);
    }

    public void CancelOverwrite()
    {
        RebindCompleteCancelOverwrite();
        m_OverwriteScreen.SetActive(false);
    }

    public void SaveBindings()
    {
        PlayerPrefs.SetFloat("CameraMovementSpeed", m_Settings[0].m_Slider.value);
        PlayerPrefs.SetFloat("CameraRotationSpeed", m_Settings[1].m_Slider.value);

        CameraControls.m_CameraControls.ApplySettings();

        string _rebinds = m_PlayerInput.actions.ToJson();

        PlayerPrefs.SetString("KeyBinds", _rebinds);

        m_AreSettingsSaved = true;
    }

    public void Resume()
    {
        if(m_AreSettingsSaved)
        {
            //Disable RebindControls and PauseButton
            m_ResumeObject[0].SetActive(false);
            m_ResumeObject[1].SetActive(true);
        }
        else
        {
            //Disable RebindControls and ApplySettings
            m_ResumeObject[0].SetActive(false);
            m_ResumeObject[2].SetActive(true);

            //Make sure the value of the sliders are correct
            m_Settings[0].m_Slider.value = PlayerPrefs.GetFloat("CameraMovementSpeed");
            m_Settings[1].m_Slider.value = PlayerPrefs.GetFloat("CameraRotationSpeed");
        }
    }

    public void SettingsAreNotSaved()
    {
        m_AreSettingsSaved = false;
    }

    public void SetRotate()
    {
        m_ActionType = ActionType.Rotation;
    }
    public void SetMove()
    {
        m_ActionType = ActionType.Movement;
    }
}
