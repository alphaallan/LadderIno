namespace Core.Data
{
    public enum LDVarClass
    {
        Input = 'I',
        Output = 'O',
        Analog = 'A',
        PWM = 'P',
        Data = 'D',
        Simulator = 'S'
    }

    public static partial class LDCoreExtensions
    {
        public static bool IsPin(this LDVarClass varClass)
        {
            return (varClass == Data.LDVarClass.Analog) || (varClass == Data.LDVarClass.Input) || (varClass == Data.LDVarClass.Output) || (varClass == Data.LDVarClass.PWM);
        }

        public static PinType ToPin(this LDVarClass varClass)
        {
            return (PinType)((char)varClass);
        }
    } 
}
