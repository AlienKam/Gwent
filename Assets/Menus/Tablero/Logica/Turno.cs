using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEngine;

namespace Logica
{
  public class Turno
  {
    public List<Player> players;
    public int current;
    public int jugadorespas;
    public bool pasarTurno;
    public bool habLider;
    public bool[] pasados;
    public Turno(List<Player> player)
    {
      players = player;
      current = 0;
      jugadorespas = 0;
      pasarTurno = false;
      habLider = false;
      pasados = new bool[player.Count]; // Antes del implica es el parametro y luego del implica el codigo
    }

    //Problemas 
    //El metodo de BegunnTurn cuando comienza una nueva ronda no siempre comienza por el primero ni el ganador
    //El problema esta en el current cuando uno termina un turno el current no se reinicia sino mas bien que cuando el otro jugador se hace false su pase pasa para este 
    //Una forma de resolverlo es que cuando se reinicie una ronda el current ta,bien se reinicie esto es provisional
    public Player BegingTurn(bool[] pasados, int current = -1)
    {
      if (current > -1) this.current = current;
      while(pasados[this.current])
      {
        EndTurn();
      }

      return players[this.current];
    }

    public void EndTurn()
    {
      if (pasarTurno)
      {
        pasados[current] = true;
        jugadorespas++;
      }
      current = (current + 1) % players.Count;
    }

    public void ReinicioTurno()
    {
      current = 0;
      jugadorespas = 0;
      pasarTurno = false;
      habLider = true;
      pasados = new bool[players.Count];
    }

    public int GetCurrent()
    {
      return current;
    }
  }
}
