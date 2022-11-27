using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorSwitcher : MonoBehaviour
{
    public InputActionProperty switchAction;
    [SerializeField] GameObject handInteractor;
    [SerializeField] GameObject rayInteractor;
    bool interSwitch = false;
    void Update()
    {
        if (switchAction.action.WasPressedThisFrame())
        {
            if (interSwitch)
            {
                handInteractor.SetActive(false);
                rayInteractor.SetActive(true);
            }
            else
            {
                handInteractor.SetActive(true);
                rayInteractor.SetActive(false);
            }
            interSwitch = !interSwitch;
        }
    }
}
