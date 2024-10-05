
using System.Collections.Generic;

namespace Parser.Language
{

    /// <summary>
    /// Interfaz que define una carta en el contexto del juego,
    /// incluyendo información adicional como el dueño de la carta
    /// </summary>
    public interface IContextCard : ICardDef
    {
        /// <summary>
        /// El jugador que posee la carta
        /// </summary>
        int Owner { get; }
    }
    /// <summary>
    /// Interfaz que representa el contexto global del juego,
    /// incluyendo el tablero, el cementerio, la baraja y la mano
    /// de cada jugador
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// El jugador que ha lanzado el efecto
        /// </summary>
        int TriggerPlayer { get; }

        /// <summary>
        /// El tablero del juego
        /// </summary>
        List<IContextCard> Board { get; }

        /// <summary>
        /// El campo del juego
        /// </summary>
        List<IContextCard> Field { get; }

        /// <summary>
        /// El cementerio del juego
        /// </summary>
        List<IContextCard> Graveyard { get; }

        /// <summary>
        /// La baraja del juego
        /// </summary>
        List<IContextCard> Deck { get; }

        /// <summary>
        /// La mano del juego
        /// </summary>
        List<IContextCard> Hand { get; }

        /// <summary>
        /// El campo del jugador especificado
        /// </summary>
        /// <param name="player">El jugador del que se quiere obtener el campo</param>
        /// <returns>El campo del jugador especificado</returns>
        List<IContextCard> FieldOfPlayer(int player);

        /// <summary>
        /// El cementerio del jugador especificado
        /// </summary>
        /// <param name="player">El jugador del que se quiere obtener el cementerio</param>
        /// <returns>El cementerio del jugador especificado</returns>
        List<IContextCard> GraveyardOfPlayer(int player);

        /// <summary>
        /// La baraja del jugador especificado
        /// </summary>
        /// <param name="player">El jugador del que se quiere obtener la baraja</param>
        /// <returns>La baraja del jugador especificado</returns>
        List<IContextCard> DeckOfPlayer(int player);

        /// <summary>
        /// La mano del jugador especificado
        /// </summary>
        /// <param name="player">El jugador del que se quiere obtener la mano</param>
        /// <returns>La mano del jugador especificado</returns>
        List<IContextCard> HandOfPlayer(int player);

        void Shuffle(List<IContextCard> list);
    }

    public interface IBlockContext
    {
        IDictionary<string, (VarType type, object value)> LocalVar { get; }
        IDictionary<string, (VarType type, object value)> GlobalVar { get; }

        IContext context { get; }
        IEnumerable<IContextCard> targets { get; }
    }

    #region IContextList

    // public interface IContextList<T>
    // {
    //     public T this[int index] { get; set; }
    //     public IEnumerable<T> Find(Func<T, bool> predicate);
    //     public T Pop();

    //     public void Push(T card);
    //     public void SendButton(T card);
    //     public void Remove(T card);
    //     public void Shuffle();
    // }

    #endregion
}