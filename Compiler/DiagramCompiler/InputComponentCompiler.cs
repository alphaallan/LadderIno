using Core.Components;

namespace Compiler
{
    public static partial class DiagramCompiler
    {
        /// <summary>
        /// Compile a single input component
        /// </summary>
        /// <param name="component">Component to be compiled</param>
        /// <returns></returns>
        internal static string CompileInputComponent(ComponentBase component)
        {
            if (component is Contact)
            {
                Contact contact = (component as Contact);
                return (contact.IsInverted) ? "!" + contact.FullName : contact.FullName;
            }
            else if (component is CompareComponent)
            {
                CompareComponent cp = component as CompareComponent;

                if (component is EQU)
                {
                    return cp.VarA + " == " + cp.VarB;
                }
                else if (component is GEQ)
                {
                    return cp.VarA + " >= " + cp.VarB;
                }
                else if (component is GRT)
                {
                    return cp.VarA + " > " + cp.VarB;
                }
                else if (component is LEG)
                {
                    return cp.VarA + " <= " + cp.VarB;
                }
                else if (component is LES)
                {
                    return cp.VarA + " < " + cp.VarB;
                }
                else if (component is NEQ)
                {
                    return cp.VarA + " != " + cp.VarB;
                }
            }

            return TRUE;
        }
    }
}
