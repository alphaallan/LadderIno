using System.Collections.Generic;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// This class is used as a buffer to the compiled code parts and data
        /// </summary>
        internal class CompilerBuffer
        {
            /// <summary>
            /// Define statments
            /// </summary>
            public List<string> Defines { get; set; }

            /// <summary>
            /// include statments
            /// </summary>
            public List<string> Includes { get; set; }

            /// <summary>
            /// Global variables declartations
            /// </summary>
            public List<string> Globals { get; set; }

            /// <summary>
            /// Arduino's setup function content
            /// </summary>
            public List<string> SetupContent { get; set; }

            /// <summary>
            /// Ladder refresh inputs function content
            /// </summary>
            public List<string> InputRefreshContent { get; set; }

            /// <summary>
            /// Ladder refresh output function content
            /// </summary>
            public List<string> OutputRefreshContent { get; set; }

            /// <summary>
            /// Rungs Compiled code
            /// </summary>
            public List<List<string>> Rungs { get; set; }

            /// <summary>
            /// User defined C++ functions
            /// </summary>
            public List<List<string>> Functions { get; set; }

            /// <summary>
            /// Number of OSF components in diagram
            /// </summary>
            public int OSFCount { get; set; }

            /// <summary>
            /// Number of OSR components in diagram
            /// </summary>
            public int OSRCount { get; set; }

            /// <summary>
            /// Number of CTD components in diagram
            /// </summary>
            public int CTDCount { get; set; }

            /// <summary>
            /// Number of CTU components in diagram
            /// </summary>
            public int CTUCount { get; set; }

            /// <summary>
            /// Number of boolean temporary variables needed in code
            /// </summary>
            public int BoolTempCount { get; set; }

            public CompilerBuffer()
            {
                Defines = new List<string>();
                Includes = new List<string>();
                Globals = new List<string>();
                SetupContent = new List<string>();
                InputRefreshContent = new List<string>();
                OutputRefreshContent = new List<string>();
                Rungs = new List<List<string>>();
                Functions = new List<List<string>>();
                OSFCount = 0;
                OSRCount = 0;
                CTDCount = 0;
                CTUCount = 0;
                BoolTempCount = 0;
            }
        }
    }
}
