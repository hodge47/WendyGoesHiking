using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using InControl;

class InputRebindingUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This is the button prefab that will be used to create rebinding buttons.")]
    private GameObject buttonPrefab;
    [SerializeField]
    [Tooltip("This is the GameObject that the buttons for each player action will go in to.")]
    private GameObject buttonContainer;

    private PlayerControlActions playerControlActions;
    private List<Button> bindingButtons = new List<Button>();
    private string saveData;

    private void Start()
    {
        // Create the player action set
        playerControlActions = PlayerControlActions.CreateWithDefaultBindings();
        // Load the player's bindings
        LoadBindings();
        // Set up the UI
        SetUpInputBindingUI();
    }

    private void OnEnable()
    {
        //playerControlActions = PlayerControlActions.CreateWithDefaultBindings();

    }

    private void OnGUI()
    {
        // Update binding UI - NOT PERFORMANT
        int _iterations = playerControlActions.Actions.Count;

        for(int i = 0; i < _iterations; i++)
        {
            var _action = playerControlActions.Actions[i];
            var _bindings = _action.Bindings;

            // Block indexes 4,5,6,7,8
            if(!CheckIfLookAction(_action))
            {
                if (_action.IsListeningForBinding)
                {
                    bindingButtons[i].gameObject.GetComponentInChildren<Text>().text = $"(Listening)";
                }
                else
                {
                    bindingButtons[i].gameObject.GetComponentInChildren<Text>().text = _bindings[0].DeviceName + ": " + _bindings[0].Name;
                }
            }
        }
    }

    private void SetUpInputBindingUI()
    {
        int _actionCount = playerControlActions.Actions.Count;
        int _lastgivenIndex = 0;

        for(int i = 0; i < _actionCount; i++)
        {
            var _action = playerControlActions.Actions[i];
            var _bindings = _action.Bindings;

            GameObject _actionButton = Instantiate(buttonPrefab, buttonContainer.transform);
            _actionButton.name = _action.Name + "Button";
            Button _button = _actionButton.GetComponent<Button>();
            int _buttonIndex = _lastgivenIndex;
            _button.onClick.AddListener(delegate {
                InitiateInputRebind(_buttonIndex, _action);
            });
            Text _buttonText = _actionButton.gameObject.GetComponentInChildren<Text>();
            _buttonText.text = _bindings[0].DeviceName + ": " + _bindings[0].Name;
            bindingButtons.Add(_button);
            _lastgivenIndex++;

            if (CheckIfLookAction(_action))
            {
                // Free spacebar key 
                if (_action.Name == "Jump")
                    _action.ClearBindings();
                    _action.AddDefaultBinding(Key.End);
                // Disbale buttons for input not being used
                _actionButton.SetActive(false);
            }
        }
    }

    private bool CheckIfLookAction(PlayerAction _action)
    {
        bool _lookAction = false;
        switch(_action.Name)
        {
            case "LookRight":
                _lookAction = true;
                break;
            case "LookLeft":
                _lookAction = true;
                break;
            case "LookUp":
                _lookAction = true;
                break;
            case "LookDown":
                _lookAction = true;
                break;
            case "Jump":
                _lookAction = true;
                break;
            default:
                _lookAction = false;
                break;
        }
        return _lookAction;
    }

    private void InitiateInputRebind(int _index, PlayerAction _action)
    {
        // Since the number of bindings per action is 2 the look axes are hidden because we need 
        //controller and keyboard support, we need to set the first binding in the action
        _action.ListenForBindingReplacing(_action.Bindings[0]);
    }

    private void OnDisable()
    {
        playerControlActions.Destroy();
    }

    private void SaveBindings()
    {
        saveData = playerControlActions.Save();
        PlayerPrefs.SetString("InputBindings", saveData);
    }

    private void LoadBindings()
    {
        if(PlayerPrefs.HasKey("InputBindings"))
        {
            saveData = PlayerPrefs.GetString("InputBindings");
            playerControlActions.Load(saveData);
        }
    }

    private void OnDestroy()
    {
        // Save the player's bindings
        SaveBindings();
        PlayerPrefs.Save();
        // Destroy the player action set
        playerControlActions.Destroy();
    }
}
