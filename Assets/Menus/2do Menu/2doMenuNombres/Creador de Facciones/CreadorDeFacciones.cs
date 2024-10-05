using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parser.Language;
using Unity.Mathematics;
using Logica;
using UnityEngine.UI;
using System;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.WSA;
using UnityEditor;
using System.IO;
using System.Linq;

public class CreadorDeFacciones : MonoBehaviour
{
    public GameObject cardHeroe;
    public GameObject cardPlata;
    public GameObject cardClima;
    public GameObject cardAumento;
    public GameObject cardSeñuelo;
    public GameObject cardAsedio;
    public GameObject cardneutral;

    public List<GameObject> cartas;

    public void CreadorFacciones(ICard carta)
    {
        GameObject instanciecard;

        switch (carta.Type)
        {
            case "Heroe":
                instanciecard = GameObject.Instantiate(cardHeroe);
                break;
            case "Plata":
                instanciecard = GameObject.Instantiate(cardPlata);
                break;
            case "Clima":
                instanciecard = GameObject.Instantiate(cardClima);
                break;
            case "Aumento":
                instanciecard = GameObject.Instantiate(cardAumento);
                break;
            case "Asedio":
                instanciecard = GameObject.Instantiate(cardAsedio);
                break;
            case "Señuelo":
                instanciecard = GameObject.Instantiate(cardSeñuelo);
                break;
            default:
                instanciecard = GameObject.Instantiate(cardneutral);
                break;
        }

        NameCard(instanciecard, carta.Name);
        ImageCard(instanciecard, carta.Type);
        TextName(instanciecard, carta.Name);
        TextAbility(instanciecard, carta.OnActivations, carta.Range);
        TextPower(instanciecard, carta.Power);

        Cartas viewCard = instanciecard.AddComponent<Cartas>();
        viewCard.nombre = carta.Name;
        viewCard.faccion = carta.Faction;
        viewCard.habilidad = carta.OnActivations.ToList();
        viewCard.power = carta.Power;
        viewCard.range = carta.Range;
        viewCard.tipoCarta = carta.Type;
        cartas.Add(instanciecard);

        instanciecard.GetComponent<RectTransform>().sizeDelta = new Vector2(77.1028f, 92.5267f);

        string pathDir = $"Assets\\Resources\\Prefabs Cartas\\{carta.Faction}\\{carta.Type}";
        if (!Directory.Exists(pathDir))
        {   
            Directory.CreateDirectory(pathDir);
        }
        PrefabUtility.SaveAsPrefabAsset(instanciecard, $"{pathDir}\\{carta.Name}.prefab");
    }

    private void NameCard(GameObject instanciecard, string name)
    {
        instanciecard.name = name;
    }

    public void ImageCard(GameObject card, string type)
    {
        Image imagencard = card.transform.GetChild(1).GetComponent<Image>();
        imagencard.type = Image.Type.Filled;
        imagencard.fillMethod = Image.FillMethod.Radial180;
        imagencard.fillOrigin = (int)Image.Origin180.Top;
        imagencard.fillAmount = 0.95f;

    }

    public void TextName(GameObject card, string key)
    {
        card.transform.GetChild(4).GetComponent<TMP_Text>().text = key;
    }

    public void TextAbility(GameObject card, IEnumerable<IOnActivation> onActivations, CardClassification[] range)
    {
        TMP_Text text = card.transform.GetChild(5).GetComponentInChildren<TMP_Text>();
        Debug.Log(text);

        foreach (var item in range)
        {
            string name = Enum.GetName(typeof(CardClassification), item);
            text.text += $"- {name}\n";
        }
        foreach (var item in onActivations)
        {
            text.text += $"- {item.Effect.Name}\n";
        }
    }

    public void TextPower(GameObject card, double power)
    {
        card.transform.GetChild(6).GetComponent<TMP_Text>().text = power.ToString();
    }
}