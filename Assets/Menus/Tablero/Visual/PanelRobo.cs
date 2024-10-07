using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PanelRobo : PanelBase
{
    public List<DevolverCartas> posiciones;
    RondaVisual rondavisual;
    public TMP_Text text;
    public bool[] robar;
    string texto;
    public GameObject scrollView;
    bool nosepuede;

    // Start is called before the first frame update
    public void Start()
    {
        nosepuede = false;
        rondavisual = GameObject.Find("Controlador de Ronda").GetComponent<RondaVisual>();
        robar = new bool[2];
    }
    /// <summary>
    /// Actualiza el texto del panel de robo para que muestre si el jugador
    /// actual ya no puede robar mas cartas.
    /// </summary>
    public void Update()
    {
        if (rondavisual.turnos.turno.current == 1)
        {
            nosepuede = true;
        }
    }

    public override void Aceptar()
    {
        if(nosepuede)
        {
            texto = "Ya no se puede robar mas";
            scrollView.SetActive(false);
            return;
        }
        if (robar[rondavisual.turnos.turno.current])
        {
            text.text = "Ya no puedes robar mas";
            scrollView.SetActive(false);
            return;
        }
        int cartas = 0;
        for (int i = 0; i < posiciones.Count; i++)
        {
            if (posiciones[i]._item == null)
            {
                continue;
            }
            cartas++;
            if (cartas > 2)
            {
                text.text = "No se puede robar mas de 2";
                return;
            }
            rondavisual.Robar(posiciones[i]._item);
            posiciones[i]._item = null;
        }
        Debug.Log("Robo Terminado");
        robar[rondavisual.turnos.turno.current] = true;
        base.Aceptar();
    }
}
