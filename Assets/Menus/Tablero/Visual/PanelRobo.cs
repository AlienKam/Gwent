using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRobo : PanelBase
{
    public List<DevolverCartas> posiciones;

    RondaVisual rondavisual;

    // Start is called before the first frame update
    public void Start()
    {
        rondavisual = GameObject.Find("Controlador de Ronda").GetComponent<RondaVisual>();
    }

    public override void Aceptar()
    {
        for (int i = 0; i < posiciones.Count; i++)
        {
            if (posiciones[i]._item == null)
            {
                continue;
            }
            rondavisual.Robar(posiciones[i]._item);
            posiciones[i]._item = null;
        }
        base.Aceptar();
    }
}
