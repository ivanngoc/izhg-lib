using System;

namespace IziHardGames
{

    [Flags]
    public enum EUserControlConditionState : int
    {
        Error = -1,

        None = 0,
        /// <summary>
        /// Current state is ON
        /// </summary>
        Active = 1 << 0,
        /// <summary>
        /// Current state is OFF
        /// </summary>
        Inactive = 1 << 1,
        /// <summary>
        /// Previous and current states are equal
        /// </summary>
        Unchanged = 1 << 2,
        /// <summary>
        /// Previous and current states are different
        /// </summary>
        Changed = 1 << 3,
        /// <summary>
        /// State Of TIGGERED is ON
        /// </summary>
        Triggered = 1 << 4,
        /// <summary>
        /// State of FILTERED is ON
        /// </summary>
        PassFilter = 1 << 5,
        /// <summary>
        /// Pass concurency stage
        /// </summary>
        WinConcurrency = 1 << 6,
        /// <summary>
        /// Will be performed at current check (this state might change with every step during frame. depends on call/check time)
        /// </summary>
        Scheduled = 1 << 7,
        /// <summary>
        /// performed in this frame
        /// </summary>
        Fired = 1 << 8,

        /// <summary>
        /// This flag is raised when On state is occured in current frame and in previous frame state was Off
        /// </summary>
        Raised = Active | Changed,
        Droped = Inactive | Changed,

        /// <summary>
        /// State of On is >=2
        /// </summary>
        KeepedOn = Active | Unchanged,
        /// <summary>
        /// State of Off is >=2
        /// </summary>
        KeepedOff = Inactive | Unchanged,
    }
}
