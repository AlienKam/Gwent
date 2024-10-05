
using System;
using System.Collections.Generic;

namespace Parser.Language
{


    internal interface IDefinitions
    {
        public IEnumerable<ICard> Cards { get; }
        public IEnumerable<IEffectDef> EffectDefs { get; }
    }

    /// <summary>
    /// Interfaz para representar un par�metro de una funci�n
    /// </summary>
    public interface IParams
    {
        /// <summary>
        /// El nombre del par�metro
        /// </summary>
        string Name { get; }

        /// <summary>
        /// El tipo de dato del par�metro
        /// </summary>
        VarType ParamsType { get; }
    }

    public interface IInputParams : IParams
    {
        object Value { get; }
    }

    /// <summary>
    /// Interfaz para representar un selector de cartas
    /// </summary>
    public interface ISelector
    {
        /// <summary>
        /// La fuente de la selecci�n
        /// </summary>
        Source Source { get; }

        /// <summary>
        /// Indica si se selecciona una carta �nica (true) o varias (false)
        /// </summary>
        bool Single { get; }

        /// <summary>
        /// La predicado que se aplica para seleccionar las cartas
        /// </summary>
        Func<IContextCard, bool> Predicate { get; }
    }
}