using System.Collections.Generic;

namespace Parser.Language
{
    /// <summary>
    /// Interfaz que define una carta de juego
    /// </summary>
    public interface ICardDef
    {
        /// <summary>
        /// Nombre de la carta
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Tipo de la carta (criatura, habilidad, etc.)
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Facción a la que pertenece la carta
        /// </summary>
        string Faction { get; }

        /// <summary>
        /// Poder de la carta
        /// </summary>
        double Power { get; set; }

        /// <summary>
        /// Rango de la carta en el campo de batalla
        /// </summary>
        CardClassification[] Range { get; }
    }

    /// <summary>
    /// Interfaz que define una carta de juego con efectos de activación
    /// </summary>
    public interface ICard : ICardDef
    {
        /// <summary>
        /// Efectos de activación de la carta
        /// </summary>
        IEnumerable<IOnActivation> OnActivations { get; }
    }
}