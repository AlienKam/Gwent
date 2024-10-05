using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/* Al inicio de cada ronda las cartas son distribuidas
    2 - El jugador puede devolver hasta 2 cartas y robar 2 cartas 
    3 - Una ronda se finaliza cuando ambos jugadores se pasan o se quedan sin cartas
    4 - Se decide quien es el ganador de la ronda segun la cantidad de puntos que tenga ese jugador
    5 - 2 rondas ganadas deciden un ganador del juego 
    6 - En caso de empate se le da un punto de ronda a cada jugador*/

namespace Logica
{
    public class Rondas
    {
        // Inicio de Ronda
        public List<Player> players;

        // Finalizar rondas 
        bool empate;
        int index;
        int lastindex;
        public bool[] pasados;

        public Rondas(List<Player> players)
        {
            this.players = players;
            empate = false;
            index = 0;
            lastindex = index;
            pasados = new bool[players.Count];
        }

        public void InsertarCarta(BaseCard card, List<BaseCard> deck)
        {
            if (index < 0 || index >= deck.Count) return;
            deck.Add(card);
        }

        public BaseCard RoboCarta(List<BaseCard> deck, int index = 0)
        {
            BaseCard cardret = deck[index];
            deck.RemoveAt(index);
            return cardret;
        }

        public BaseCard IntercambioCarta(BaseCard card, List<BaseCard> deck, int index = 0)
        {
            InsertarCarta(card, deck);
            return RoboCarta(deck, index);
        }

        public void PuntosRonda()
        {
            double maxpuntos = double.MinValue;
            int indexWinner = -1;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].puntos > maxpuntos)
                {
                    maxpuntos = players[i].puntos;
                    indexWinner = i;
                }
            }

            lastindex = index;
            index = indexWinner;
            players[index].puntosronda++;

            for (int j = 0; j < players.Count; j++)
            {
                if (players[j].puntos == maxpuntos && j != index)
                {
                    players[j].puntosronda++;
                    empate = true;
                }
            }
        }
        public Player GanadorRonda()
        {
            if (empate)
            {
                return null;
            }
            return players[index];
        }

        public int GetIndexInicioRonda()
        {
            if (empate)
            {
                return lastindex;
            }
            return index;
        }

        public void GanadorJuego()
        {
            int maxrondas = int.MinValue;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].puntosronda > maxrondas)
                {
                    maxrondas = players[i].puntosronda;
                }
            }
        }

        //Esto borra a los pasados 
        public void BorrarPasados()
        {
            for (int i = 0; i < pasados.Length; i++)
            {
                pasados[i] = false;
                Debug.Log(pasados[i]);
            }
        }

        public List<BaseCard> CartasMano(int cantidadcart, List<BaseCard> hand, List<BaseCard> deck)
        {
            List<BaseCard> cartasinst = new List<BaseCard>();
            for (int i = 0; i < cantidadcart; i++)
            {
                if (deck.Count <= i) break;
                hand.Add(deck[i]);
                cartasinst.Add(deck[i]);
            }

            deck.RemoveRange(0, cartasinst.Count);

            return cartasinst;
        }
    }
}

