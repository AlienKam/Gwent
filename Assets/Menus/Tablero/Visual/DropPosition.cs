using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Logica;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using Parser.Language;
using System;
using static Parser.Language.CallMethods;


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

        IContext context = GetContext(player);
        tablero.PonerCartas(cardlog, (int)position.x, (int)position.y, turnos.player, context);
        turnos.termino = true;
    }

    public IContext GetContext(int playerId)
    {
        var hand = new Func<int, IEnumerable<IContextCard>>(id => control.hands[id].
            Select(x => new ContextCard(x.GetComponent<Cartas>().baseCard, id)));
        var Deck = new Func<int, IEnumerable<IContextCard>>(id => control.decks[id].
            Select(x => new ContextCard(x.GetComponent<Cartas>().baseCard, id)));
        var Graveyard = new Func<int, IEnumerable<IContextCard>>(id => control.cementerio[id].transform.GetComponentsInChildren<Cartas>().
            Select(x => new ContextCard(x.GetComponent<Cartas>().baseCard, id)));
        var Field = new Func<int, IEnumerable<IContextCard>>(id => GameObject.FindGameObjectsWithTag($"Posociones {id + 1}").
            Select(x => new ContextCard(x.GetComponentInChildren<Cartas>().baseCard, id)));
        var Board = GameObject.FindGameObjectsWithTag($"Posociones 1").
            Select(x => new ContextCard(x.GetComponentInChildren<Cartas>().baseCard, 0)).
            Concat(Field(1));

        var ShuffleHand = new Action<int>((playerId) =>
        {
            var poss = control.rondavisual.GetPositions()[playerId].GetComponentsInChildren<PosicionMano>();
            var CardList = poss.Select(x => x.transform.GetChild(0)).ToList();
            for (int i = 0; i < poss.Length; i++)
            {
                control.SwapValues(CardList);
            }

            for (int i = 0; i < poss.Length; i++)
            {
                var cartFrom = CardList[i];
                if (cartFrom == null) continue;
                cartFrom.parent = poss[i].transform;
            }
        });

        var ShuffleDeck = new Action<int>(id =>
        {
            for (int i = 0; i < control.decks[id].Count; i++)
            {
                control.SwapValues(control.decks[id]);
            }
        });

        var InsertDeck = new Action<IContextCard, int, int>((card, id, index) =>
        {
            var path = $"Prefabs Cartas\\{card.Faction}\\{card.Type}\\{card.Name}.prefab";
            control.decks[id].Insert(index, Resources.Load<GameObject>(path));
        });

        var RemoveDeck = new Action<ICard, int>((card, id) =>
        {
            control.decks[id].RemoveAll(x => x.transform.GetComponent<Cartas>().baseCard == card);
        });

        return new Context(playerId, tablero.tablero,
            Board, hand, Field, Graveyard, Deck,
            ShuffleHand, ShuffleDeck, InsertDeck, RemoveDeck);
    }
}
