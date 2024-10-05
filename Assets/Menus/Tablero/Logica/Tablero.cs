using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logica
{
   public enum TipoPosicion
   {
      CuerpoaCuerpo = 0b_001,
      LargaDistancia = 0b_010,
      Asedio = 0b_100,
      Aumento,
      Clima,
      Deck,
   }
   public class Tablero
   {
      // Porque las señuelos no se pueden hacer monstercards
      public MonsterCard[,] tablero;
      public bool[,] tablerobool;
      public BaseCard[] aumentos;
      public bool[] aumentosbool;
      public BaseCard[] climas;
      public bool[] climasbool;
      public List<BaseCard> cementerio;
 
      public Tablero()
      {
         tablero = new MonsterCard[6, 5];
         tablerobool = new bool[6, 5];
         climas = new BaseCard[6];
         aumentos = new BaseCard[6];
         aumentosbool = new bool[6];
         climasbool = new bool[6];
         cementerio = new List<BaseCard>();
      }
   }

   public class FuncionesTablero
   {
      Tablero tab;
      public FuncionesTablero(Tablero tablero)
      {
         tab = tablero;
      }
      Tablero tablero = new Tablero();
      public void InicializarTablero(Tablero tablerojuego)
      {
         tablero = tablerojuego;
      }
      public void PonerCartas(BaseCard card, int fila, int columna, Player player)
      {
         if (columna > 5)
         {
            if (columna == 6)
            {
               tablero.aumentos[fila] = card;
               tablero.aumentosbool[fila] = true;
            }

            else
            {
               tablero.climas[fila] = card;
               tablero.climasbool[fila] = true;
            }
         }
         else
         {
            MonsterCard carta = card as MonsterCard;
            tablero.tablero[fila, columna] = carta;
            tablero.tablerobool[fila, columna] = true;
            player.puntos += carta.Power;
         }
      }

      public bool IsValido(uint clas, uint clascard, int playeractual, int seccion)
      {
         bool val = (clas & clascard) == clas;
         if ((seccion == playeractual) && val)
         {
            return true;
         }

         return false;
      }

      public bool IsValidoEspecial(TipoPosicion tab, string card, int playeractual, int seccion)
      {
         if((seccion == playeractual)&&((tab == TipoPosicion.Aumento && card == "Aumento") || (tab == TipoPosicion.Clima && card == "Clima")))
         {
            return true;
         }
         return false;
      }
   }
}