
namespace Parser.Language
{
    /// <summary>
    /// Enum que define las fuentes de una carta, que son desde donde se puede
    /// obtener una carta.
    /// </summary>
    public enum Source
    {
        Hand,
        OtherHands,
        Deck,
        OtherDecks,
        Field,
        OtherFields,
        Parent,
    }
}