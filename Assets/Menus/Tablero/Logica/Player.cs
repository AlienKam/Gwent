using System.Collections;
using System.Collections.Generic;


namespace Logica
{
    public class Player
    {
        public string nombreplayer;
        public string faccion;
        public double puntos;
        public int puntosronda;
        public Player(string name, string faccion)
        {
            nombreplayer = name;
            this.faccion = faccion;
            puntos = 0;
            puntosronda = 0;
        }
    }
}
