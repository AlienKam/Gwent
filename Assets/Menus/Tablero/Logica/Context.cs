using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Parser.Language;
using static Parser.Language.CallMethods;

namespace Logica
{
   public class Context : IContext
   {
      public int TriggerPlayer { get; }

      private Tablero tablero;
      private Func<int, IEnumerable<IContextCard>> hand;
      private Func<int, IEnumerable<IContextCard>> field;
      private Func<int, IEnumerable<IContextCard>> graveyard;
      private Func<int, IEnumerable<IContextCard>> deck;
      private Action<int> shuffleHand;
      private Action<int> shuffleDeck;
      private Action<IContextCard, int, int> insertDeck;
      private Action<ICard, int> removeDeck;

      public List<IContextCard> Board { get; }
      public List<IContextCard> Deck => DeckOfPlayer(TriggerPlayer);
      public List<IContextCard> Field => FieldOfPlayer(TriggerPlayer);
      public List<IContextCard> Graveyard => GraveyardOfPlayer(TriggerPlayer);
      public List<IContextCard> Hand => HandOfPlayer(TriggerPlayer);

      public Context(int triggerPlayer, Tablero tablero,
         IEnumerable<IContextCard> board,
         Func<int, IEnumerable<IContextCard>> hand,
         Func<int, IEnumerable<IContextCard>> field,
         Func<int, IEnumerable<IContextCard>> graveyard,
         Func<int, IEnumerable<IContextCard>> deck,
         Action<int> shuffleHand, Action<int> shuffleDeck,
         Action<IContextCard, int, int> insertDeck, Action<ICard, int> removeDeck)
      {
         TriggerPlayer = triggerPlayer;
         this.tablero = tablero;
         Board = board.ToList();

         this.hand = hand;
         this.field = field;
         this.graveyard = graveyard;
         this.deck = deck;
         this.shuffleHand = shuffleHand;
         this.shuffleDeck = shuffleDeck;
         this.insertDeck = insertDeck;
         this.removeDeck = removeDeck;
      }

      public List<IContextCard> HandOfPlayer(int player) => hand(player).ToList();
      public List<IContextCard> FieldOfPlayer(int player) => field(player).ToList();
      public List<IContextCard> GraveyardOfPlayer(int player) => graveyard(player).ToList();
      public List<IContextCard> DeckOfPlayer(int player) => deck(player).ToList();

      public void Shuffle(List<IContextCard> list, TypeList typeList, int player)
      {
         switch (typeList)
         {
            case TypeList.Hand:
               shuffleHand(player);
               break;
            case TypeList.Deck:
               shuffleDeck(player);
               break;
            default:
               throw new Exception();
         }
      }

      public void Push(List<IContextCard> list, TypeList typeList, IContextCard card, int player)
      {
         if (typeList != TypeList.Deck)
            throw new Exception();
         list.Insert(0, card);
         insertDeck(card, player, 0);
      }

      public void Remove(List<IContextCard> list, TypeList typeList, IContextCard card, int player)
      {
         if (typeList != TypeList.Deck)
            throw new Exception();
         list.Remove(card);
         removeDeck((card as ContextCard).baseCard, player);
      }

      public void SendButton(List<IContextCard> list, TypeList typeList, IContextCard card, int player)
      {
         if (typeList != TypeList.Deck)
            throw new Exception();
         list.Insert(list.Count - 1, card);
         insertDeck(card, player, list.Count - 1);
      }
   }
}