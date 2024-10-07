using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logica;
using Unity.VisualScripting;
using System.Linq;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Cosas de la logica
    public List<Player> playerlog;
    public Tablero tablero;
    public FuncionesTablero funcionesTablero;

    public Rondas rondalogica;
    public bool terminojuego;

    // Cosas Visuales
    Turnos turnos;
    public Transform canvasTransform;
    public List<GameObject> players;
    public List<List<GameObject>> decks;
    public List<List<GameObject>> hands;
    public GameObject[] cementerio;
    public RondaVisual rondavisual;
    public List<GameObject> listdeck1;
    public List<GameObject> listdeck2;
    public List<GameObject> hand1;
    public List<GameObject> hand2;
    public GameObject posiciondeck1;
    public GameObject posiciondeck2;
    public GameObject panel;
    public GameObject ganador;
    private Player winner;
    private bool isPanelActive;

    public GameObject winnerpanel;
    public TMP_Text text;
    public void Awake()
    {
        // Inicializar las listas
        playerlog = new List<Player>();

        decks = new List<List<GameObject>>();
        players = new List<GameObject>();
        hands = new List<List<GameObject>>();
        hand1 = new List<GameObject>();
        hand2 = new List<GameObject>();

        terminojuego = false;
        tablero = new Tablero();
        funcionesTablero = new FuncionesTablero(tablero);

        //Esto busca al objeto en la escena 
        GameObject deck1 = GameObject.Find("Deck 1");
        GameObject deck2 = GameObject.Find("Deck 2");

        deck1.GetComponent<RectTransform>().sizeDelta = new Vector2(89.168f, 125.2989f);
        deck2.GetComponent<RectTransform>().sizeDelta = new Vector2(89.168f, 125.2989f);

        GameObject player1 = GameObject.Find("Player 1");
        GameObject player2 = GameObject.Find("Player 2");

        posiciondeck1 = GameObject.Find("Posicion Deck 1");
        posiciondeck2 = GameObject.Find("Posicion Deck 2");

        GameObject seccion1 = GameObject.Find("Seccion 1");
        GameObject seccion2 = GameObject.Find("Seccion 2");

        cementerio = GameObject.FindGameObjectsWithTag("Cementerio");
        rondavisual = GameObject.Find("Controlador de Ronda").GetComponent<RondaVisual>();
        turnos = GameObject.Find("Controlador de Turno").GetComponent<Turnos>();

        //Crea las instancias
        Player player3 = new Player(player1.GetComponent<PlayersVisual>().nameplayer, player1.GetComponent<PlayersVisual>().faccionplayer, 0);
        Player player4 = new Player(player2.GetComponent<PlayersVisual>().nameplayer, player2.GetComponent<PlayersVisual>().faccionplayer, 1);
        funcionesTablero.InicializarTablero(tablero);

        //Agrega y crea referencias a listas
        listdeck1 = deck1.GetComponent<Decks>().deck;
        listdeck2 = deck2.GetComponent<Decks>().deck;

        decks.Add(listdeck1);
        decks.Add(listdeck2);

        players.Add(player1);
        players.Add(player2);

        hands.Add(hand1);
        hands.Add(hand2);

        playerlog.Add(player3);
        playerlog.Add(player4);

        //Este le asigna posicion deck como padre del deck 
        deck1.transform.SetParent(posiciondeck1.transform);
        deck2.transform.SetParent(posiciondeck2.transform);

        //Lo pone en la posicion del padre 
        deck1.transform.position = new Vector3(deck1.transform.parent.position.x, deck1.transform.parent.position.y, deck1.transform.parent.position.z + 12);
        deck2.transform.position = new Vector3(deck2.transform.parent.position.x, deck2.transform.parent.position.y, deck2.transform.parent.position.z + 12);

        //Le asigna seccion como padre del player
        player1.transform.SetParent(seccion1.transform, false);
        player2.transform.SetParent(seccion2.transform, false);

        //Swapea los decks
        for (int i = 0; i < listdeck1.Count; i++)
        {
            SwapValues(listdeck1);
            SwapValues(listdeck2);
        }

        List<string> tags = new List<string>() { "Posiciones 1", "Posiciones 2" };
        for (int i = 0; i < cementerio.Length; i++)
        {
            cementerio[i].GetComponent<Cementerio>().Posiciones(tags[i]);
        }

    }

    //Hace las instancias de cada cosa antes de comenzar el juego y le pasa la informacion a la parte logica
    public void OnEnable()
    {
        rondavisual.InstanciarRondas(playerlog, decks, hands, cementerio, terminojuego);
        turnos.InstanciarTurnos(playerlog);
        rondavisual.IniciarRonda();
    }
    public void Start() { }

    //Pregunta constantemente si el juego termino
    public void Update()
    {
        if (rondavisual.terminoronda && Termino())
        {
            Debug.Log("Termino Ronda");
            TerminarJuego(); // TODO implementar ver el panel de final de juego
            GameObject winnergame = players.Find(x => x.GetComponent<PlayersVisual>().nameplayer == winner.nombreplayer);
            ganador.name = winnergame.GetComponent<PlayersVisual>().nameplayer;
            ganador.tag = "Winner";
            DontDestroyOnLoad(ganador);
            SceneManager.LoadScene("Fin del Juego 1");
        }
        // Detecta si se presiona la tecla Enter
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            panel.SetActive(false); // Desactiva el panel
            isPanelActive = false; // Actualiza el estado del panel
        }
    }

    private Player GetWinner()
    {
        Player winner = null;
        foreach (var item in playerlog)
        {
            if (winner == null || item.puntosronda > winner.puntosronda)
            {
                winner = item;
            }
        }
        return winner;
    }

    private bool Termino()
    {
        winner = GetWinner();
        int count = playerlog.Count(x => x.puntosronda == winner.puntosronda);

        return count == 1 && winner.puntosronda > 1;
    }

    private void TerminarJuego()
    {
        winnerpanel.SetActive(true);
        text.text = $"Felicidades eres el {winner.nombreplayer}";
    }


    public void SwapValues<T>(List<T> deck)
    {
        int indexA = Random.Range(0, deck.Count);
        int indexB = Random.Range(0, deck.Count);

        T tempcard = deck[indexA];
        deck[indexA] = deck[indexB];
        deck[indexB] = tempcard;
    }



    public void InstanciarLogica()
    {
        rondalogica = new Rondas(playerlog);
    }
}

