using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    public GameObject panel;
    public CanvasGroup canvas;
    public List<CanvasGroup> actives;

    public void OnEnable() => ChangeCast(false);
    public void OnDisable() => ChangeCast(true);

    private void ChangeCast(bool enable)
    {
        canvas.blocksRaycasts = enable;
        foreach (var item in actives)
        {
            item.ignoreParentGroups = !enable;
        }
    }

    public virtual void Aceptar()
    {
        panel.SetActive(false);
    }
}