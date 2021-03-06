﻿namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Compiler Current version
        /// </summary>
        internal const string COMPILER_VERSION = "0.3";

        /// <summary>
        /// Indetation level string
        /// </summary>
        internal const string INDENT = "  ";

        /// <summary>
        /// True constant string
        /// </summary>
        internal const string TRUE = "true";

        /// <summary>
        /// False constant string
        /// </summary>
        internal const string FALSE = "false";

        /// <summary>
        /// Marker for the node number in activation sub-expressions
        /// </summary>
        internal const string NODE_NUMBER_MARKER = "#";

        #region Names
        /// <summary>
        /// Ladder names prefix
        /// </summary>
        internal const string NAME_PREFIX = "ld_";

        #region Environment Variables
        /// <summary>
        /// Rung Activation Temporary Data name in compiled code
        /// </summary>
        internal const string RATD = NAME_PREFIX + "RATD";

        /// <summary>
        /// OSF states buffer name
        /// </summary>
        internal const string OSF_DATA = NAME_PREFIX + "StateOSF";

        /// <summary>
        /// OSR states buffer name
        /// </summary>
        internal const string OSR_DATA = NAME_PREFIX + "StateOSR";
        #endregion

        #region Functions Names
        /// <summary>
        /// Refresh Input function Name
        /// </summary>
        internal const string REFRESH_INPUT_FN = NAME_PREFIX + "InputRefresh";

        /// <summary>
        /// Refresh Output function Name
        /// </summary>
        internal const string REFRESH_OUTPUT_FN = NAME_PREFIX + "OutputRefresh";

        /// <summary>
        /// OSF function Name
        /// </summary>
        internal const string OSF_FN = NAME_PREFIX + "OSF";

        /// <summary>
        /// OSR function Name
        /// </summary>
        internal const string OSR_FN = NAME_PREFIX + "OSR";

        /// <summary>
        /// CTD function Name
        /// </summary>
        internal const string CTD_FN = NAME_PREFIX + "CTD";

        /// <summary>
        /// CTU function Name
        /// </summary>
        internal const string CTU_FN = NAME_PREFIX + "CTU";
        #endregion

        #endregion
    }
}
