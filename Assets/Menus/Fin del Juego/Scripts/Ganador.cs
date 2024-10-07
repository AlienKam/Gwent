using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ganador : MonoBehaviour
{
    public TMP_Text text;
    public string playername;
    // Start is called before the first frame update
    public void Start()
    {
        playername = GameObject.FindGameObjectWithTag("Winner").name;
        text.text = $"Felicidades {playername} eres el ganador del juego !";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
