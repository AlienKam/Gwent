using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Logica;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DeckPositions : MonoBehaviour, IPointerClickHandler
{
    public GameObject panel;
    public GameObject Text;
    Turnos turno;

    // Start is called before the first frame update
    public void Start()
    {
        turno = GameObject.Find("Controlador de Turno").GetComponent<Turnos>();
    }

    // Update is called once per frame
    public void Update()
    { 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("DNBDBNDFBUDU");
        GameObject gameObject = turno.iniciorondaturno ? panel : Text;
        gameObject.SetActive(true);
    }
}
