using System.Collections;
using System.Collections.Generic;
using Logica;
using UnityEngine;
using TMPro;

// Kam si funciona no lo toques 
public class Turnos : MonoBehaviour
{
    //Variables de la logica
    public Player player;
    public Turno turno;

    //Booleanos
    public bool termino;
    public bool iniciorondaturno;
    public bool[] pasados;

    //Objetos
    public GameObject panel;

    //Clases
    RondaVisual ronda;
    
    //Textos
    public TMP_Text textpanel;
    public List<TMP_Text> textospunt;

    //Numeros
    public int current;
    public int playerspasados;
    public float tiempo;
    // Start is called before the first frame update

    public void Awake()
    {
        ronda = GameObject.Find("Controlador de Ronda").GetComponent<RondaVisual>();
    }
    public void Start()
    {   
        termino = false;
        tiempo = 6f;
        playerspasados = 0;
    }

    public void Update()
    {
        if(!ronda.inicioronda && turno.GetCurrent() == ronda.listdecks.Count)
        {
            Debug.Log("Entro Aqui");
            ronda.inicioronda = false;
            iniciorondaturno = ronda.inicioronda;
        }
        if (!ronda.terminoronda && termino)
        {
            TerminarTurno();
        }
        // Hacer un metodo 
        if (!ronda.terminoronda && termino && !ronda.isPanelActive)
        {
            InicioTurno();
        }

    }

    public void InstanciarTurnos(List<Player> player)
    {
        turno = new Turno(player);
        current = turno.GetCurrent();
        pasados = new bool[player.Count];
    }


    public void ReinicioTurnos(RondaVisual rondaVisual)
    {
        termino = false;
        iniciorondaturno = rondaVisual.inicioronda;
        turno.ReinicioTurno();
        pasados = turno.pasados;
        playerspasados = turno.jugadorespas;
        current = turno.GetCurrent();
    }

    public void InicioTurno(int current = -1)
    {
        termino = false;
        player = turno.BegingTurn(turno.pasados, current);
        //sections[turno.GetCurrent()].blocksRaycasts = true;
        StartCoroutine(ActivatePanel(panel, tiempo));
        textpanel.text = $"Es el turno de :  {player.nombreplayer}";
    }

    public void PasarTurno()
    {
        // hacer un metodo en la logica  con todo esto 
        current = turno.GetCurrent();
        pasados[current] = true;
        turno.pasados[current] = true;
        playerspasados++;
        termino = true;
    }

    public void TerminarTurno()
    {
        textospunt[turno.GetCurrent()].text = $"{player.puntos}";
        turno.EndTurn();
    }

    public IEnumerator ActivatePanel(GameObject panel, float activeTime)
    {
        panel.SetActive(true); // Activa el panel
        yield return new WaitForSeconds(activeTime); // Espera el tiempo especificado
        panel.SetActive(false); // Desactiva el panel
    }
}
