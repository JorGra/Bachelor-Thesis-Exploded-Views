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
    public InputActionProperty resetControlsAction;
    [SerializeField] List<Transform> targetTrans;
    [SerializeField] List<Transform> controlTrans;
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
        if (resetControlsAction.action.WasPressedThisFrame())
        {
            if (targetTrans.Count != controlTrans.Count)
                return;

            for (int i = 0; i < targetTrans.Count; i++)
            {
                controlTrans[i].position = targetTrans[i].position;
            }
        }
    }

    void SayHi() => Debug.Log("HI!");
}
