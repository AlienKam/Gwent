using System.Collections;
using System.Collections.Generic;
using Logica;
using UnityEngine;

public class Cementeriolog
{
    List<List<BaseCard>> muertas = new List<List<BaseCard>>();

    public void AddCartas(BaseCard card, int index)
    {
        muertas[index].Add(card);
    }

    public void LimpiarTablero(Tablero tablero)
    {
        for (int i = 0; i < tablero.tablero.GetLength(0); i++)
        {
            for (int j = 0; j < tablero.tablero.GetLength(1); j++)
            {
                tablero.tablero[i,j] = null;
                tablero.tablerobool[i,j] = false;
            }
        }

        for (int i = 0; i < tablero.climas.Length; i++)
        {
            tablero.climas[i] = null;
            tablero.climasbool[i] = false;
            tablero.aumentos[i] = null;
            tablero.aumentosbool[i] = false; 
        }
    }
    
    public bool MirarVal(int player, int indexcemente)
    {
        if (player == indexcemente)
        {
            return true;
        }
        return false;
    }
}
