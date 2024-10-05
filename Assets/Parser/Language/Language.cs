
using System;
using System.Collections.Generic;
using Dicciona;
using UnityEditor.Build;

namespace Parser.Language
{
    public class Node { }

    /// <summary>
    /// Clase donde se guardan las cartas y los efectos.
    /// </summary>
    public class Definitions : Node, IDefinitions
    {
        private Dictionary<string, ICard> cards;
        public Dictionary<string, IEffectDef> effectsdef;

        public IEnumerable<ICard> Cards => cards.Values;
        public IEnumerable<IEffectDef> EffectDefs => effectsdef.Values;
        public Definitions()
        {
            cards = new Dictionary<string, ICard>();
            effectsdef = new Dictionary<string, IEffectDef>();
        }

        public void AddEffectDef(string name, EffectDef def)
        {
            effectsdef.Add(name, def);
        }

        public void AddCardDef(string name,Card def)
        {
            cards.Add(name, def);
        }
    }
    /// <summary>
    /// Clase que define un efecto, esta clase tiene las siguientes propiedades:
    /// <list type="bullet">
    /// <item>
    /// <description>name: es el nombre del efecto</description>
    /// </item>
    /// <item>
    /// <description>params: son los par metros que recibe el efecto</description>
    /// </item>
    /// <item>
    /// <description>action: es la accion que se va a ejecutar cuando se llame al efecto</description>
    /// </item>
    /// </list>
    /// </summary>
    public class EffectDef : Node, IEffectDef
    {
        public EffectDef(string name, IParams[] @params, Action<IEnumerable<IContextCard>, IContext, IInputParams[]> action)
        {
            Name = name;
            Params = @params;
            Action = action;
        }

        /// <summary>
        /// El nombre del efecto
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Los par metros que recibe el efecto
        /// </summary>
        public IParams[] Params { get; private set; }

        /// <summary>
        /// La accion que se va a ejecutar cuando se llame al efecto
        /// </summary>
        public Action<IEnumerable<IContextCard>, IContext, IInputParams[]> Action { get; private set; }
    }
    public class Params : Node, IParams
    {
        public Params(string name, VarType paramsType)
        {
            Name = name;
            ParamsType = paramsType;
        }

        public string Name { get; private set; }

        public VarType ParamsType { get; private set; }
    }

    /// <summary>
    /// Clase que representa los parametros de entrada que se puede definir en una carta,
    /// esta clase tiene las siguientes propiedades:
    /// <list type="bullet">
    /// <item>
    /// <description>name: es el nombre del parametro</description>
    /// </item>
    /// <item>
    /// <description>value: es el valor del parametro</description>
    /// </item>
    /// <item>
    /// <description>paramsType: es el tipo de parametro</description>
    /// </item>
    /// </list>
    /// </summary>
    public class InputParams : Node, IInputParams
    {
        public InputParams(string name, object value, VarType paramsType)
        {
            Name = name;
            Value = value;
            ParamsType = paramsType;
        }

        public InputParams(string name, ValueExp valueExp)
        {
            Name = name;
            Value = valueExp.Value.Execute(new BlockContext());
            ParamsType = valueExp.Type;
        }

        public string Name { get; set; }

        public object Value { get; set; }

        public VarType ParamsType { get; set; }
    }
    public class Card : Node, ICard, ICardDef
    {
        public Card(string name, string type, string faction, int power, CardClassification[] range, IEnumerable<IOnActivation> onActivations)
        {
            Name = name;
            Type = type;
            Faction = faction;
            Power = power;
            Range = range;
            OnActivations = onActivations;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Faction { get; set; }

        public double Power { get; set; }

        public CardClassification[] Range { get; private set; }

        public IEnumerable<IOnActivation> OnActivations { get; private set; }
    }

    public class ActionEffect : Node, IActionEffect
    {
        public ActionEffect(ICallEffect effect, ISelector selector)
        {
            Effect = effect;
            Selector = selector;
        }

        public ICallEffect Effect { get; private set; }

        public ISelector Selector { get; private set; }
    }

    public class CallEffect : Node, ICallEffect
    {
        public CallEffect(string name, IInputParams[] @params, bool simple = false)
        {
            Name = name;
            Params = @params;
            this.simple = simple;
        }

        public bool simple { get; }

        public string Name { get; private set; }

        public IInputParams[] Params { get; private set; }
    }

    public class Selector : Node, ISelector
    {
        public Selector(Source source, bool single, Func<IContextCard, bool> predicate)
        {
            Source = source;
            Single = single;
            Predicate = predicate;
        }

        public Source Source { get; private set; }

        public bool Single { get; private set; }

        public Func<IContextCard, bool> Predicate { get; private set; }
    }

    public class OnActivation : Node, IOnActivation, IActionEffect
    {
        public OnActivation(IActionEffect postAction, ICallEffect effect, ISelector selector)
        {
            PostAction = postAction;
            Effect = effect;
            Selector = selector;
        }

        public IActionEffect PostAction { get; private set; }

        public ICallEffect Effect { get; private set; }

        public ISelector Selector { get; private set; }
    }
}