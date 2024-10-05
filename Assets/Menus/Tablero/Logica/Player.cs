using System.Collections;
using System.Collections.Generic;


namespace Logica
{
    public class Player
    {
        public int playerId;
        public string nombreplayer;
        public string faccion;
        public double puntos;
        public int puntosronda;
        public List<BaseCard> hand;
        public Player(string name, string faccion, int playerId)
        {
            this.playerId = playerId;
            nombreplayer = name;
            this.faccion = faccion;
            puntos = 0;
            puntosronda = 0;
            hand = new List<BaseCard>();
        }
    }
}
