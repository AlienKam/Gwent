using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logica;
using UnityEngine.EventSystems;
using Parser.Language;
using Unity.VisualScripting;
using UnityEngine.Analytics;


public class Cartas : MonoBehaviour
{
    public string faccion;
    public string nombre;
    public double power;
    public string tipoCarta;
    public IEnumerable<IOnActivation> habilidad;
    public CardClassification[] range;
    public BaseCard baseCard;

    public void Update()
    {
        if(baseCard == null) return;
        else
        {
            power = baseCard.Power; 
            faccion = baseCard.Faction;
            nombre = baseCard.Name;
            tipoCarta = baseCard.Type;
            range = baseCard.Range;
            habilidad = baseCard.OnActivations;
        }
    }

    public BaseCard CrearCarta()
    {
        if (baseCard != null) return baseCard;

        switch (tipoCarta)
        {
            case "Heroe":
                baseCard = new Heroe(nombre, power, faccion, range, habilidad);
                break;
            case "Normales":
                baseCard = new Normales(nombre, power, faccion, range, habilidad);
                break;
            case "Clima":
                baseCard = new Clima(nombre, power, faccion, range, habilidad);
                break;
            case "Aumento":
                baseCard = new Aumento(nombre, power, faccion, range, habilidad);
                break;
            case "Señuelo":
                baseCard = new Senuelo(nombre, power, faccion, range, habilidad);
                break;
            case "Lider":
                baseCard = new Lider(nombre, power, faccion, range, habilidad);
                break;
            default:
                throw new System.Exception("Tu carta no es de ningun tipo vuelve a intentarlo");
        }
        return baseCard;
    }

    // LLamar a los metodos que cambian los valores visualmente
}

