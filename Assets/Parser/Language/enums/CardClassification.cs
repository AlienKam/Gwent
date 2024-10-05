namespace Parser.Language
{
    /// <summary>
    /// Enumerado que clasifica las cartas en su tipo seg n su rango de ataque
    /// </summary>
    public enum CardClassification
    {
        /// <summary>
        /// Cartas de combate cuerpo a cuerpo
        /// </summary>
        Melee = 0b_001,
        /// <summary>
        /// Cartas de combate a larga distancia
        /// </summary>
        LongRange = 0b_010,
        /// <summary>
        /// Cartas de asedio
        /// </summary>
        Siege = 0b_100,
    }
}