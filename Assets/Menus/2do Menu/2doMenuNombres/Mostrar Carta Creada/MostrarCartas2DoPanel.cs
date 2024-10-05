using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MostrarCartas2DoPanel : MonoBehaviour
{
    public CreadorDeFacciones creador;
    public List<GameObject> cardscread;
    public GameObject posicion1card;
    public List<GameObject> posiciones2card;
    public List<GameObject> posiciones3card;
    public List<GameObject> posicions4card;
    public List<GameObject> posicionscard;


    public void MostrarCartas()
    {
        //posicionscard = GameObject.FindGameObjectsWithTag(" Posiciones 1").ToList();
        cardscread = creador.cartas;
        if (cardscread.Count == 1)
        {
            cardscread[0].transform.position = posicion1card.transform.position;
            cardscread[0].transform.SetParent(posicion1card.transform);
            RectTransform parent = cardscread[0].transform.parent.GetComponent<RectTransform>();
            cardscread[0].GetComponent<RectTransform>().sizeDelta = new Vector2(parent.sizeDelta.x, parent.sizeDelta.y);
        }
        if (cardscread.Count == 2) Cartas(posiciones2card);
        if (cardscread.Count == 3) Cartas(posiciones2card);
        if (cardscread.Count == 4) Cartas(posicions4card);
        if (cardscread.Count >= 5) Cartas(posicionscard);
    }

    public void Cartas(List<GameObject> pos)
    {
        for (int i = 0; i < cardscread.Count; i++)
        {
            cardscread[i].transform.position = pos[i].transform.position;
            cardscread[i].transform.SetParent(pos[i].transform);
            RectTransform parent = cardscread[i].transform.parent.GetComponent<RectTransform>();
            cardscread[i].GetComponent<RectTransform>().sizeDelta = new Vector2(parent.sizeDelta.x, parent.sizeDelta.y);
        }
    }
}
