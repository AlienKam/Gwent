using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Logica;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using System.Linq;


public class DropPosition : MonoBehaviour, IDropHandler
{
    public Vector2 position;
    public TipoPosicion clasificacion;
    public GameObject _item;
    private FuncionesTablero tablero;
    private Turnos turnos;
    Cartas card;
    BaseCard cardlog;
    GameObject newitem;
    GameManager control;
    public int seccion;
    GameObject pos;

    public void Start()
    {
        pos = this.gameObject;
        turnos = GameObject.Find("Controlador de Turno").GetComponent<Turnos>();
        control = GameObject.Find("Controlador de juego ").GetComponent<GameManager>();
        tablero = control.funcionesTablero;
    }
    // Si entra al metodo y este metodo hace la funcion de Drop
    public void OnDrop(PointerEventData eventData)
    {
        int player = turnos.turno.current;
        newitem = eventData.pointerDrag;

        card = newitem.GetComponent<Cartas>();
        cardlog = card.CrearCarta();

        if (_item && newitem.GetComponent<Cartas>().tipoCarta != "Señuelo")
        {
            newitem.GetComponent<DragItem>().ReturnToStartingPosition();
            return;
        }

        if (clasificacion == TipoPosicion.Clima || clasificacion == TipoPosicion.Aumento)
        {
            bool validespecial = tablero.IsValidoEspecial(clasificacion, card.tipoCarta, player, seccion);
            if (!validespecial || _item)
            {
                _item.GetComponent<DragItem>().ReturnToStartingPosition();
            }
        }
        else
        {
            uint other = (uint)card.range.Sum(x => (uint)x);
            bool valido = tablero.IsValido((uint)clasificacion, other, player, seccion);

            if (!valido || _item)
            {
                _item.GetComponent<DragItem>().ReturnToStartingPosition();
            }
        }
        _item = newitem;
        DragItem compitem = _item.GetComponent<DragItem>();
        _item.transform.SetParent(transform);
        _item.transform.position = transform.position + new Vector3(0, 0, 1);
        compitem.IsDropped(true);
        tablero.PonerCartas(cardlog, (int)position.x, (int)position.y, turnos.player);
        turnos.termino = true;
    }
}
