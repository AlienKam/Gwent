using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionMano : MonoBehaviour
{

    public bool ocupada;
    GameObject posicion;
    // Start is called before the first frame update
    void Start()
    {
        posicion = this.gameObject;

        if(posicion.transform.childCount != 0)
        {
            ocupada = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(posicion.transform.childCount == 0)
        {
            ocupada = false;
        }
        else
        {
            ocupada = true;
        }
    }
}
