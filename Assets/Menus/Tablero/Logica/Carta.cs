using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parser.Language;
using System;
using Unity.VisualScripting;

namespace Logica
{
    public class ContextCard : IContextCard
    {
        public BaseCard baseCard;

        public ContextCard(BaseCard baseCard, int owner)
        {
            this.baseCard = baseCard;
            Owner = owner;
        }

        public int Owner { get; }

        public string Name => baseCard.Name;

        public string Type => baseCard.Type;

        public string Faction => baseCard.Faction;

        public double Power
        {
            get
            {
                return baseCard.Power;
            }
            set
            {
                baseCard.Power = value;
            }
        }

        public CardClassification[] Range => baseCard.Range;
    }

    public abstract class BaseCard : ICardDef, ICard
    {
        public string Name { get; }
        public string Type { get; }
        public string Faction { get; }
        public double Power { get; set; }
        public CardClassification[] Range { get; }
        public IEnumerable<IOnActivation> OnActivations { get; }

        public BaseCard(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations)
        {
            Name = name;
            Type = GetType().Name;
            Faction = faccion;
            Power = power;
            Range = range;
            OnActivations = onActivations;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public abstract class MonsterCard : BaseCard
    {
        public uint clasificacion { get; }
        protected MonsterCard(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        {
            for (int i = 0; i < range.Length; i++)
            {
                clasificacion += (uint)range[i];
            }
        }
    }

    public class Aumento : BaseCard
    {
        public Aumento(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        { }
    }

    public class Senuelo : BaseCard
    {
        public Senuelo(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        { }
    }

    public class Clima : BaseCard
    {
        public Clima(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        { }
    }

    public class Normales : MonsterCard
    {
        public Normales(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        { }
    }

    public class Heroe : MonsterCard
    {
        public Heroe(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        { }
    }

    public class Lider : BaseCard
    {
        public Lider(string name, double power, string faccion, CardClassification[] range, IEnumerable<IOnActivation> onActivations) : base(name, power, faccion, range, onActivations)
        {
            range = new CardClassification[0];
        }
    }
}
