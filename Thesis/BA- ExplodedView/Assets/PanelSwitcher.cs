using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    [SerializeField] List<ExploderControls> panels = new List<ExploderControls>();

    public void SwitchPanel(int i)
    {
        panels.ForEach(p => p.panel.SetActive(false));
        foreach (var p in panels)
        {
            p.panel.SetActive(false);
            p.controlObjects.ForEach(c => c.SetActive(false));
        }


        panels[i].panel.SetActive(true);
        panels[i].controlObjects.ForEach(c => c.SetActive(true));
    }
}

[System.Serializable]
class ExploderControls
{
    public GameObject panel;
    public List<GameObject> controlObjects = new List<GameObject>();

}
