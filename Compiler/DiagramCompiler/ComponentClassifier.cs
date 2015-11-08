using Core.Components;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Component class in compiler
        /// </summary>
        internal enum ComponentCompilerClass
        {
            Input,
            InputFunction,
            Output
        }

        /// <summary>
        /// Get component's compiler class
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        internal static ComponentCompilerClass GetCompilerClass(this ComponentBase component)
        {
            if (component.Class == ComponentBase.ComponentClass.Input)
            {
                if ((component is OSR) || (component is OSF))
                {
                    return ComponentCompilerClass.InputFunction;
                }
                else return ComponentCompilerClass.Input;
            }
            else if (component.Class == ComponentBase.ComponentClass.Mixed)
            {
                return ComponentCompilerClass.InputFunction;
            }
            else return ComponentCompilerClass.Output;
        }
    }
}
