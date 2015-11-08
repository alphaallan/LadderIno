using Core.Components;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Compile single input function
        /// </summary>
        /// <param name="component">Component to be compiled</param>
        /// <param name="input">Function logical input</param>
        /// <param name="codeBuffer">Compiler's code buffer</param>
        /// <returns></returns>
        internal static string CompileInputFunctionComponent(ComponentBase component, string input, CompilerBuffer codeBuffer)
        {
            string buffer = string.Empty;

            if (component is OSF)
            {
                buffer = OSF_FN + "(" + codeBuffer.OSRCount + ", " + input + ")";
                codeBuffer.OSFCount++;
            }
            else if (component is OSR)
            {
                buffer = OSR_FN + "(" + codeBuffer.OSRCount + ", " + input + ")";
                codeBuffer.OSRCount++;
            }
            else if (component is CounterComponent)
            {
                CounterComponent counter = component as CounterComponent;

                if (component is CTD)
                {
                    buffer = CTD_FN + "(" + counter.Limit + ", &" + counter.FullName + ", " + codeBuffer.OSRCount + ", " + input + ")";
                    codeBuffer.OSRCount++;
                }
                else if (component is CTU)
                {
                    buffer = CTU_FN + "(" + counter.Limit + ", &" + counter.FullName + ", " + codeBuffer.OSRCount + ", " + input + ")";                    
                    codeBuffer.OSRCount++;
                }
            }

            return buffer;
        }
    }
}
