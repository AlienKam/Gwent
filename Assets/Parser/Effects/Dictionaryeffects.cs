using System;
using System.Collections;
using System.Collections.Generic;
using Parser.Language;
using UnityEngine;
using Logica;

namespace Dicciona
{
    public static class Dictionaryeffects
    {
        public static Dictionary<string, IEffectDef> effects = new Dictionary<string,IEffectDef>();
        public static Dictionary<string, IContextCard> cards = new Dictionary<string, IContextCard>();
        public static Dictionary<string, List<IOnActivation>> cardEffects = new Dictionary<string, List<IOnActivation>>(); 
    }
}
