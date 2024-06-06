namespace IziHardGames
{
    /// <summary>
    /// <see cref=""/>
    /// </summary>
    public enum EConditionRelation
    {
        // Resolving Stage
        /// <summary>
        /// If Right side is ON than left side is OFF (override)
        /// </summary>
        Yield,
        /// <summary>
        /// If Left side Is ON than right side is OFF (override)
        /// </summary>
        Dominate,

        /// <summary>
        /// Left side is ON only if right side is ON (lookup after filtering. Resolving Stage)
        /// </summary>
        With,
        /// <summary>
        /// Left side is OFF is right side is ON (lookup after filtering. Resolving stage)
        /// </summary>
        Without,

        // Execution stage
        /// <summary>
        /// On Executing of Left Execute Right (Executing stage)
        /// </summary>
        Tandem,
        /// <summary>
        /// On Executing Left set Right to OFF (left must be higher priority than right)
        /// </summary>
        Dump,
    }

    public enum EConditionType
    {
        /// <summary>
        /// Left size is <see cref="UserControlConditionUnit"/> right size is is <see cref="UserControlConditionUnit"/> 3rd slot is <see cref="EConditionRelation"/>
        /// </summary>
        ActionToAction,

        /// <summary>
        /// Left size is <see cref="UserControlConditionUnit"/> right size is Boolean
        /// </summary>
        IsMode,
        IsAction,

    }
}
