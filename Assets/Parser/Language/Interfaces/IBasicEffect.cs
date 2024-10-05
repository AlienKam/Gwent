
using System;
using System.Collections.Generic;

namespace Parser.Language
{
    /// <summary>
    /// Interfaz que define una efecto de carta
    /// </summary>
    public interface IEffectDef
    {
        /// <summary>
        /// El nombre del efecto
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Los parámetros del efecto
        /// </summary>
        IParams[] Params { get; }

        /// <summary>
        /// La acción del efecto
        /// </summary>
        Action<IEnumerable<IContextCard>, IContext, InputParams[]> Action { get; }
    }

    /// <summary>
    /// Interfaz que define una efecto que se puede llamar
    /// </summary>
    public interface IActionEffect
    {
        /// <summary>
        /// El efecto que se puede llamar
        /// </summary>
        ICallEffect Effect { get; }

        /// <summary>
        /// El selector de la carta que debe ser afectada por el efecto
        /// </summary>
        ISelector Selector { get; }
    }

    /// <summary>
    /// Interfaz que define una efecto que se activa al momento de ser llamado
    /// </summary>
    public interface IOnActivation : IActionEffect
    {
        /// <summary>
        /// La acción que se debe realizar luego de activar el efecto
        /// </summary>
        IActionEffect PostAction { get; }
    }

    /// <summary>
    /// Interfaz que define una efecto que se puede llamar
    /// </summary>
    public interface ICallEffect
    {
        /// <summary>
        /// El nombre del efecto
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Los parámetros del efecto
        /// </summary>
        IInputParams[] Params { get; }
    }
}