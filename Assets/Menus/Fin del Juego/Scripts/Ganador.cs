using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ganador : MonoBehaviour
{
    public TMP_Text text;
    // Start is called before the first frame update
    public void Start()
    {
        PlayersVisual winner = GameObject.FindObjectOfType<PlayersVisual>();
        text.text = $"Felicidades {winner.nameplayer} eres el ganador del juego !";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
