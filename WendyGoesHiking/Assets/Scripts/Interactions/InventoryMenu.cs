using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    private static InventoryMenu instance;
    //variable for canvasgroup or other canvas item
    //private CanvasGroup canvasGroup;
    //TODO: set up Menu design

    public static InventoryMenu Instance
    {
        get
        {
            if (instance == null)
                throw new System.Exception("There is currently no InventoryMenu instance, make sure InventoryMenu script is on an object in the scene.");
            return instance;
        }
        private set { instance = value; }
    }

    //private void ShowMenu()
    //{
    //    canvasGroup.alpha = 0;
    //    canvasGroup.interactable = true;
    //}
    //private void HideMenu()
    //{
    //    canvasGroup.alpha = 0;
    //    canvasGroup.interactable = false;
    //}

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            throw new System.Exception("There is already an instance of the InventoryMenu and there can only be one.");

        //canvasGroup = GetComponent<CanvasGroup>();
        //HideMenu();
    }
}
