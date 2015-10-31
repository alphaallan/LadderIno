using Core.Components;
using Core.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System;

namespace LDFile
{
    public static partial class LDFile
    {
        private static void ReadPinout(Diagram diagram, XmlNodeList pins)
        {
            diagram.RefreshPins();

            Trace.WriteLine("Pins Load Started");
            Trace.Indent();
            foreach (XmlNode xPin in pins)
            {
                PinType temp;
                if (!Enum.TryParse<PinType>(xPin.LocalName, out temp)) throw new FormatException("Corrupted File. Unrecognized pin type");

                LDPin pin = diagram.Pins.Where(x => x.Variable == xPin.Attributes["Variable"].Value).First();
                pin.Pin = xPin.Attributes["Pin"].Value;
            }
            Trace.Unindent();
            Trace.WriteLine("Pins Load Ended");
        }
    }
}
