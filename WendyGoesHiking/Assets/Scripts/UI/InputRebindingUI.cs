using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class InputRebindingUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This is the button prefab that will be used to create rebinding buttons.")]
    private GameObject buttonPrefab;
    [SerializeField]
    [Tooltip("This is the GameObject that the buttons for each player action will go in to.")]
    private GameObject buttonContainer;

    private PlayerControlActions playerControlActions;
    private string saveData;
    private List<Button> bindingButtons = new List<Button>();

    private void Start()
    {
        playerControlActions = PlayerControlActions.CreateWithDefaultBindings();
        // Set up the UI
        SetUpInputBindingUI();
    }

    private void OnEnable()
    {
        //playerControlActions = PlayerControlActions.CreateWithDefaultBindings();

    }

    private void SetUpInputBindingUI()
    {
        int _actionCount = playerControlActions.Actions.Count;
        for(int i = 0; i < _actionCount; i++)
        {
            var _action = playerControlActions.Actions[i];
            var _actionBindings = _action.Bindings;

            GameObject _actionButton = Instantiate(buttonPrefab, buttonContainer.transform);
            _actionButton.name = _action.Name + "Button";
            Button _button = _actionButton.GetComponent<Button>();
            int _buttonIndex = i;
            _button.onClick.AddListener(delegate {
                InitiateInputRebind(_buttonIndex);
            });
            Text _buttonText = _actionButton.gameObject.GetComponentInChildren<Text>();
            _buttonText.text = _actionBindings[0].Name;
            bindingButtons.Add(_button);
        }
    }

    private void InitiateInputRebind(int _index)
    {
        Debug.Log($"{bindingButtons[_index].gameObject.name}");
    }

    private void OnDisable()
    {
        playerControlActions.Destroy();
    }
}
