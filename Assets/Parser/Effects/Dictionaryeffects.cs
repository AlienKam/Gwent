using System;
using System.Collections;
using System.Collections.Generic;
using Parser.Language;
using Unity.Mathematics;
using UnityEngine;
using Logica;

namespace Dicciona
{
    public class Dictionaryeffects
    {
        public static Dictionary<string, IEffectDef> effects = new Dictionary<string,IEffectDef>();
        public static Dictionary<string, IContextCard> cards = new Dictionary<string, IContextCard>();
    }
}
