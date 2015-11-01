using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Components;

namespace CoreLogicalTest
{
    [TestClass]
    public class CompilerTest
    {
        [TestMethod]
        [TestCategory("Compiler")]
        public void Compile()
        {
            //|     x1          y2      x2     y1   |
            //|--+--[ ]--[OSR]--[/]--+--[/]----( )--|
            //|  |  y1               |              |
            //|  +--[ ]--------------+              |
            //|                                     |
            //|     x2          y1      x1     y2   |
            //|--+--[ ]--[OSR]--[/]--+--[/]----( )--|
            //|  |  y2               |              |
            //|  +--[ ]--------------+              |

            Diagram TestDiagram = new Diagram();
            TestDiagram.MasterRelay = true;

            #region Declare Components
            Rung Rung1 = new Rung();
            Rung Rung2 = new Rung();

            var Y1 = new Coil();
            Y1.Name = "1";

            var Y2 = new Coil();
            Y2.Name = "2";

            var X1 = new Contact();
            X1.Name = "1";

            var X1I = new Contact();
            X1I.Name = "1";
            X1I.IsInverted = true;

            var X2 = new Contact();
            X2.Name = "2";

            var X2I = new Contact();
            X2I.Name = "2";
            X2I.IsInverted = true;

            var Y1C = new Contact();
            Y1C.Name = "1";
            Y1C.Type = Contact.ContactType.OutputPin;

            var Y1CI = new Contact();
            Y1CI.Name = "1";
            Y1CI.Type = Contact.ContactType.OutputPin;
            Y1CI.IsInverted = true;

            var Y2C = new Contact();
            Y2C.Name = "2";
            Y2C.Type = Contact.ContactType.OutputPin;

            var Y2CI = new Contact();
            Y2CI.Name = "2";
            Y2CI.Type = Contact.ContactType.OutputPin;
            Y2CI.IsInverted = true;

            #endregion Declare Components

            #region Build Circuit
            Rung1.Add(Y1);
            Rung1.InsertBefore(X2I, Y1);
            Rung1.Add(X1);
            Rung1.InsertUnder(Y1C, X1);
            Rung1.InsertAfter(Y2CI, X1);
            Rung1.InsertBefore(new OSR(), Y2CI);

            TestDiagram.Add(Rung1);

            Rung2.Add(Y2);
            Rung2.InsertBefore(X1I, Y2);
            Rung2.Add(X2);
            Rung2.InsertUnder(Y2C, X2);
            Rung2.InsertAfter(Y1CI, X2);
            Rung2.InsertBefore(new OSR(), Y1CI);

            TestDiagram.InsertUnder(Rung2, Rung1);

            #endregion Build Circuit

            TestDiagram.RefreshPins();
            TestDiagram.Pins[0].Pin = "3";
            TestDiagram.Pins[1].Pin = "9";
            TestDiagram.Pins[2].Pin = "8";
            TestDiagram.Pins[3].Pin = "4";

            Console.WriteLine();
            Console.Write(LDFile.DiagramCompiler.CompileDiagram(TestDiagram));
        }
    }
}
