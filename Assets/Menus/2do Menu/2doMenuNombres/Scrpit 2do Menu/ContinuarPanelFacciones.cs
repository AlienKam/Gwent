using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Logica;
using Scene = UnityEngine.SceneManagement.Scene;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ContinuarPanelFacciones : MonoBehaviour
{
   public TMP_InputField intputplayer1;
   public TMP_InputField intputplayer2;
   public TMP_Dropdown player1drop;
   public TMP_Dropdown player2drop;
   public GameObject deck1;
   public GameObject deck2;
   public GameObject player1;
   public GameObject player2;
   public Scene newScene;

   public void Update()
   {
      if (player1drop == null || player2drop == null) return;
      var facciones = Directory.CreateDirectory("Assets/Resources/Prefabs Cartas/").GetDirectories().Select(x => x.Name).Where(x => x != "Cartas Base");

      if (player1drop.options.Count == facciones.Count()) return;
      player1drop.ClearOptions();
      player1drop.AddOptions(facciones.ToList());
      player2drop.ClearOptions();
      player2drop.AddOptions(facciones.ToList());
   }

   // Este metodo prepara las cosas iniciales para el juego osea crea los directorios de las cartas instasncia los jugadores y coge las facciones
   public void Continuar()
   {
      string nameplayer1 = intputplayer1.textComponent.text;
      string nameplayer2 = intputplayer2.textComponent.text;

      string faccionName1 = player1drop.captionText.text;
      string faccionName2 = player1drop.captionText.text;

      player1.GetComponent<PlayersVisual>().nameplayer = nameplayer1;
      player1.GetComponent<PlayersVisual>().faccionplayer = faccionName1;
      player2.GetComponent<PlayersVisual>().nameplayer = nameplayer2;
      player2.GetComponent<PlayersVisual>().faccionplayer = faccionName2;

      // DirectoryInfo directory1 = Directory.CreateDirectory($"Assets/Iconos/Prefabs Cartas/{faccionName1}");
      // DirectoryInfo directory2 = Directory.CreateDirectory($"Assets/Iconos/Prefabs Cartas/{faccionName2}");

      // List<GameObject> cartsFaccion1 = LoadFaccionCarts(directory1).ToList();
      // List<GameObject> cartsFaccion2 = LoadFaccionCarts(directory2).ToList();

      List<GameObject> cartsFaccion1 = Resources.LoadAll<GameObject>($"Prefabs Cartas/{faccionName1}").ToList();
      List<GameObject> cartsFaccion2 = Resources.LoadAll<GameObject>($"Prefabs Cartas/{faccionName2}").ToList();

      deck1.GetComponent<Decks>().deck = cartsFaccion1;
      deck2.GetComponent<Decks>().deck = cartsFaccion2;

      // foreach (var item in cartsFaccion1) DontDestroyOnLoad(item);
      // foreach (var item in cartsFaccion2) DontDestroyOnLoad(item);

      DontDestroyOnLoad(player1);
      DontDestroyOnLoad(player2);
      DontDestroyOnLoad(deck1);
      DontDestroyOnLoad(deck2);

      // Cargar la nueva escena de manera aditiva
      SceneManager.LoadScene("Tablero");
      // Es el indice de la escena en Build Settings
   }

   // ES EL TC
   //Este carga las cartas del directorio 
   public IEnumerable<GameObject> LoadFaccionCarts(DirectoryInfo directoryActual)
   {
      foreach (FileInfo file in directoryActual.GetFiles())
      {
         if (!file.Extension.Equals(".prefab")) continue;

         string filePath = file.FullName;
         GameObject cart = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
         yield return cart;
      }
      foreach (DirectoryInfo directory in directoryActual.GetDirectories())
      {
         foreach (GameObject cart in LoadFaccionCarts(directory))
         {
            yield return cart;
         }
      }
   }
}
