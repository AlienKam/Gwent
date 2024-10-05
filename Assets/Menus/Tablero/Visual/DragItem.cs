using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logica;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
  //Logica
  public BaseCard carta;
  //Booleanos
  private bool _dontmove;
  public bool valido;
  private bool _isDropped;
  private bool _enableDrop;
  private bool _inGame;

  //GameObjects y sus cosas
  private GameObject _dragParent;
  Image cardSprite;

  //Posiciones
  private Vector3 _startingPosition;
  private Transform _startingParent;

  //Canvas Group
  private CanvasGroup canvas;

  //Clases
  private MostrarCartas displayManager;
  private Turnos turnoactual;
  private GameManager controlador;

    public void Start()
  {
    _dontmove = false;
    _isDropped = false;
    _enableDrop = false;
    _dragParent = GameObject.Find("DragParent");
    canvas = _dragParent.GetComponent<CanvasGroup>();
    displayManager = FindObjectOfType<MostrarCartas>();
    turnoactual = GameObject.Find("Controlador de Turno").GetComponent<Turnos>();
    controlador = GameObject.Find("Controlador de juego ").GetComponent<GameManager>();
  }

  // Cosas del OnDrag
  public void OnBeginDrag(PointerEventData eventData)
  {
    Valido();
    if (!valido) return;
    _startingPosition = transform.position;
    _startingParent = transform.parent;
    transform.SetParent(_dragParent.transform);
    canvas.blocksRaycasts = false;
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (!valido) return;
    transform.position = eventData.position;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    canvas.blocksRaycasts = true;
    if (!valido) return;
    if (_isDropped)
    {
      _dontmove = true;
      turnoactual.termino = true;
      return;
    }

    ReturnToStartingPosition();
  }

  public void ReturnToStartingPosition()
  {
    transform.position = _startingPosition;
    transform.SetParent(_startingParent);
  }

  public void IsDropped(bool dropped)
  {
    _isDropped = dropped;
  }

  public bool IsEnableDrop()
  {
    return _enableDrop;
  }

  //Estos son los metodos para mostrar una carta en el panel de mostrar cartas
  public void OnPointerEnter(PointerEventData eventData)
  {
    //Esto es para el 2do proyecto

    /*Transform childTransform = transform.GetChild(0);
    cardSprite = childTransform.GetComponent<Image>();
    displayManager.ShowCardImage(cardSprite.sprite);*/

    // cardSprite = GetComponent<Image>();
    // displayManager.ShowCardImage(cardSprite.sprite);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    displayManager.HideCardImage();
  }

  public void SetInGame(bool inGame)
  {
    _inGame = inGame;
  }


  //Cosas que funcionaraban
  public void Valido()
  {
    GameObject player = controlador.players[turnoactual.turno.GetCurrent()];
    valido =  !_dontmove && GetPathParent(gameObject, player);
  }
  public bool GetPathParent(GameObject newitem, GameObject player)
  {
    string tags = "Seccion";
    GameObject parent1 = FindParentWithTag(newitem, tags);
    GameObject parent2 = FindParentWithTag(player, tags);

    bool ok1 = parent1.Equals(parent2);
    return ok1;
  }
  public GameObject FindParentWithTag(GameObject child, string tag)
  {
    Transform parentTransform = child.transform.parent;
    while (parentTransform != null)
    {
      if (parentTransform.CompareTag(tag))
      {
        return parentTransform.gameObject;
      }
      parentTransform = parentTransform.parent;
    }
    return null;
  }
}