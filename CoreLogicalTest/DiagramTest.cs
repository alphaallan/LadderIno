using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Components;
using System.Diagnostics;

namespace CoreLogicalTest
{
    [TestClass]
    public class DiagramTest
    {
        [TestMethod]
        [TestCategory("Diagram")]
        public void BasicTest()
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

            Trace.WriteLine("");

            #region Cycle 1
            Trace.WriteLine("Cycle 1", "Unit Test");
            Trace.Indent();
            TestDiagram.Execute();
            Assert.IsFalse(X1.InternalState, "X1 Fail");
            Assert.IsFalse(X2.InternalState, "X2 Fail");
            Assert.IsFalse(X1I.InternalState, "X1I Fail");
            Assert.IsFalse(X2I.InternalState, "X2I Fail");
            Assert.IsFalse(Y1.InternalState, "Y1 Fail");
            Assert.IsFalse(Y2.InternalState, "Y2 Fail");
            Assert.IsFalse(Y1C.InternalState, "Y1C Fail");
            Assert.IsFalse(Y2C.InternalState, "Y2C Fail");
            Assert.IsFalse(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsFalse(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 1

            #region Cycle 2
            Trace.WriteLine("Cycle 2", "Unit Test");
            Trace.Indent();
            X1.IsClosed = true;
            TestDiagram.Execute();
            Assert.IsTrue(X1.InternalState, "X1 Fail");
            Assert.IsFalse(X2.InternalState, "X2 Fail");
            Assert.IsFalse(X1I.InternalState, "X1I Fail");
            Assert.IsTrue(X2I.InternalState, "X2I Fail");
            Assert.IsTrue(Y1.InternalState, "Y1 Fail");
            Assert.IsFalse(Y2.InternalState, "Y2 Fail");
            Assert.IsFalse(Y1C.InternalState, "Y1C Fail");
            Assert.IsFalse(Y2C.InternalState, "Y2C Fail");
            Assert.IsFalse(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsTrue(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 2

            #region Cycle 3
            Trace.WriteLine("Cycle 3", "Unit Test");
            Trace.Indent();
            X1.IsClosed = false;
            TestDiagram.Execute();
            Assert.IsFalse(X1.InternalState, "X1 Fail");
            Assert.IsFalse(X2.InternalState, "X2 Fail");
            Assert.IsFalse(X1I.InternalState, "X1I Fail");
            Assert.IsTrue(X2I.InternalState, "X2I Fail");
            Assert.IsTrue(Y1.InternalState, "Y1 Fail");
            Assert.IsFalse(Y2.InternalState, "Y2 Fail");
            Assert.IsTrue(Y1C.InternalState, "Y1C Fail");
            Assert.IsFalse(Y2C.InternalState, "Y2C Fail");
            Assert.IsFalse(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsFalse(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 3

            #region Cycle 4
            Trace.WriteLine("Cycle 4", "Unit Test");
            Trace.Indent();
            X2.IsClosed = true;
            TestDiagram.Execute();
            Assert.IsFalse(X1.InternalState, "X1 Fail");
            Assert.IsTrue(X2.InternalState, "X2 Fail");
            Assert.IsTrue(X1I.InternalState, "X1I Fail");
            Assert.IsFalse(X2I.InternalState, "X2I Fail");
            Assert.IsFalse(Y1.InternalState, "Y1 Fail");
            Assert.IsTrue(Y2.InternalState, "Y2 Fail");
            Assert.IsTrue(Y1C.InternalState, "Y1C Fail");
            Assert.IsFalse(Y2C.InternalState, "Y2C Fail");
            Assert.IsTrue(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsFalse(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 4

            #region Cycle 5
            Trace.WriteLine("Cycle 5", "Unit Test");
            Trace.Indent();
            X2.IsClosed = false;
            TestDiagram.Execute();
            Assert.IsFalse(X1.InternalState, "X1 Fail");
            Assert.IsFalse(X2.InternalState, "X2 Fail");
            Assert.IsTrue(X1I.InternalState, "X1I Fail");
            Assert.IsFalse(X2I.InternalState, "X2I Fail");
            Assert.IsFalse(Y1.InternalState, "Y1 Fail");
            Assert.IsTrue(Y2.InternalState, "Y2 Fail");
            Assert.IsFalse(Y1C.InternalState, "Y1C Fail");
            Assert.IsTrue(Y2C.InternalState, "Y2C Fail");
            Assert.IsFalse(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsFalse(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 5

            #region Cycle 6
            Trace.WriteLine("Cycle 6", "Unit Test");
            Trace.Indent();
            X1.IsClosed = true;
            TestDiagram.Execute();
            Assert.IsTrue(X1.InternalState, "X1 Fail");
            Assert.IsFalse(X2.InternalState, "X2 Fail");
            Assert.IsFalse(X1I.InternalState, "X1I Fail");
            Assert.IsFalse(X2I.InternalState, "X2I Fail");
            Assert.IsFalse(Y1.InternalState, "Y1 Fail");
            Assert.IsFalse(Y2.InternalState, "Y2 Fail");
            Assert.IsFalse(Y1C.InternalState, "Y1C Fail");
            Assert.IsTrue(Y2C.InternalState, "Y2C Fail");
            Assert.IsFalse(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsFalse(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 6

            #region Cycle 7
            Trace.WriteLine("Cycle 7", "Unit Test");
            Trace.Indent();
            X1.IsClosed = false;
            TestDiagram.Execute();
            Assert.IsFalse(X1.InternalState, "X1 Fail");
            Assert.IsFalse(X2.InternalState, "X2 Fail");
            Assert.IsFalse(X1I.InternalState, "X1I Fail");
            Assert.IsFalse(X2I.InternalState, "X2I Fail");
            Assert.IsFalse(Y1.InternalState, "Y1 Fail");
            Assert.IsFalse(Y2.InternalState, "Y2 Fail");
            Assert.IsFalse(Y1C.InternalState, "Y1C Fail");
            Assert.IsFalse(Y2C.InternalState, "Y2C Fail");
            Assert.IsFalse(Y1CI.InternalState, "Y1CI Fail");
            Assert.IsFalse(Y2CI.InternalState, "Y2CI Fail");
            Trace.Unindent();
            #endregion Cycle 7
        }
    }
}
