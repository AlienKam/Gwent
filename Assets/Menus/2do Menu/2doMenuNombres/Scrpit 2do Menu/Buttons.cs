using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Parser;
using Parser.Tokenstools;
using Parser.Language;
using System;
using Dicciona;
using Logica;

public class Buttons : MonoBehaviour
{
   public GameObject paneldesact;
   public GameObject paneldesact2;
   public GameObject panelact;
   public TMP_InputField inputField;
   public CreadorDeFacciones creador;
   TextAsset textAsset;
   public string fileName = "Creador.txt";

   public void CambioDeScena()
   {
      SceneManager.LoadScene(0);
   }

   public void CreateCards()
   {
      fileName = "Creador";
      panelact.SetActive(true);
      paneldesact.SetActive(false);
      paneldesact2.SetActive(false);

      textAsset = Resources.Load<TextAsset>(fileName);
      if (textAsset != null)
      {
         inputField.text = textAsset.text;
      }
   }

   /// <summary>
   /// Escribe en un archivo de texto el contenido actual de <see cref="inputField"/>, con el nombre
   /// de archivo especificado en <see cref="fileName"/>, y con la extension .txt.
   /// </summary>
   public void Acep()
   {
      string text = inputField.text;
      string separadores = ",;:. {[()]}/*-+=<>&|@\r\n\"";
      ParserCards parserCards = new ParserCards();

      string[] words = Lexer.MySplit(text, separadores);
      IToken[] tokens = Lexer.GetTokens(words, Storage.Tokens);

      Definitions node = parserCards.Parse(tokens);

      foreach (var item in node.effectsdef)
      {
         if (Dictionaryeffects.effects.ContainsKey(item.Key))
         {
            Dictionaryeffects.effects[item.Key] = item.Value;
         }
         else
         {
            Dictionaryeffects.effects.Add(item.Key, item.Value);
         }
      }

      foreach (var item in node.Cards)
      {
         Debug.Log(creador);
         Debug.Log(item);
         creador.CreadorFacciones(item);
      }
      Debug.Log("");
   }

   public void Atras()
   {
      panelact.SetActive(false);
      paneldesact.SetActive(true);
      paneldesact2.SetActive(true);
   }
}