using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Components;
using System.Diagnostics;

namespace CoreLogicalTest
{
    [TestClass]
    public class RungTest
    {
        [TestMethod]
        [TestCategory("Rung")]
        public void BasicTest()
        {
            //|     x1             x2     y1   |
            //|--+--[ ]--[OSR]--+--[/]----( )--|
            //|  |  y1          |              |
            //|  +--[ ]---------+              |
            //

            Rung TestRung = new Rung();

            var Y1 = new Coil();
            Y1.Name = "1";
            var X1 = new Contact();
            X1.Name = "1";
            var X2 = new Contact();
            X2.Name = "2";
            X2.IsInverted = true;

            var Y1C = new Contact();
            Y1C.Name = "1";
            Y1C.Type = Contact.ContactType.OutputPin;

            TestRung.Add(Y1);
            TestRung.InsertBefore(X2,Y1);
            TestRung.Add(X1);
            TestRung.InsertUnder(Y1C, X1);
            TestRung.InsertAfter(new OSR(),X1);

            Trace.WriteLine("Memory alloc");
            Trace.Indent();
            TestRung.DataTable = new Core.Data.LadderDataTable();
            Trace.Unindent();

            Trace.WriteLine("Cycle 1", "Unit Test");
            Trace.Indent();
            TestRung.Execute();
            Trace.Unindent();

            Trace.WriteLine("Cycle 2", "Unit Test");
            Trace.Indent();
            TestRung.DataTable.SetValue("X1", true);
            TestRung.Execute();
            Trace.Unindent();

            Trace.WriteLine("Cycle 3", "Unit Test");
            Trace.Indent();
            TestRung.DataTable.SetValue("X1", false);
            TestRung.Execute();
            Trace.Unindent();

            Trace.WriteLine("Cycle 4", "Unit Test");
            Trace.Indent();
            TestRung.Execute();
            Trace.Unindent();

            Trace.WriteLine("Cycle 5", "Unit Test");
            Trace.Indent();
            TestRung.DataTable.SetValue("X2", true);
            TestRung.Execute();
            Trace.Unindent();

            Trace.WriteLine("END", "Unit Test");
            TestRung.Clear();
            TestRung = null;
            GC.Collect();
        }

        [TestMethod]
        [TestCategory("Rung")]
        public void FindInterceptionTest()
        {
            //|     x1             x2     y1   |
            //|--+--[ ]--[OSR]--+--[/]----( )--|
            //|  |  y1    x3    |              |
            //|  +--[ ]---[ ]---+              |
            //

            Rung TestRung = new Rung();

            var Y1 = new Coil();
            Y1.Name = "1";
            var X1 = new Contact();
            X1.Name = "1";
            var X2 = new Contact();
            X2.Name = "2";
            X2.IsInverted = true;
            var X3 = new Contact();
            X3.Name = "2";
            X3.IsInverted = true;

            var Y1C = new Contact();
            Y1C.Name = "1";
            Y1C.Type = Contact.ContactType.OutputPin;

            TestRung.Add(Y1);
            TestRung.InsertBefore(X2, Y1);
            TestRung.Add(X1);
            TestRung.InsertUnder(Y1C, X1);
            TestRung.InsertAfter(new OSR(), X1);
            TestRung.InsertAfter(X3, Y1C);

            Tuple<Node, Node> temp;

            temp = TestRung.FindInterception(Y1C,X3);
            Assert.IsNull(temp);

            temp = TestRung.FindInterception(Y1C, X2);
            Assert.IsNull(temp);

            temp = TestRung.FindInterception(Y1C, Y1);
            Assert.IsNull(temp);

            temp = TestRung.FindInterception(Y1C, X1);
            Assert.AreEqual(temp.Item1, X1.LeftLide);
            Assert.AreEqual(temp.Item2, X3.RightLide);

            temp = TestRung.FindInterception(X3, X1);
            Assert.AreEqual(temp.Item1, X1.LeftLide);
            Assert.AreEqual(temp.Item2, X3.RightLide);
        }

        [TestMethod]
        [TestCategory("Rung")]
        public void GetAllBetweenTest()
        {
            //|     x1             x2     y1   |
            //|--+--[ ]--[OSR]--+--[/]----( )--|
            //|  |  y1    x3    |              |
            //|  +--[ ]---[ ]---+              |
            //

            Rung TestRung = new Rung();

            var Y1 = new Coil();
            Y1.Name = "1";
            var X1 = new Contact();
            X1.Name = "1";
            var X2 = new Contact();
            X2.Name = "2";
            X2.IsInverted = true;
            var X3 = new Contact();
            X3.Name = "2";
            X3.IsInverted = true;

            var Y1C = new Contact();
            Y1C.Name = "1";
            Y1C.Type = Contact.ContactType.OutputPin;

            TestRung.Add(Y1);
            TestRung.InsertBefore(X2, Y1);
            TestRung.Add(X1);
            TestRung.InsertUnder(Y1C, X1);
            TestRung.InsertAfter(new OSR(), X1);
            TestRung.InsertAfter(X3, Y1C);

            Assert.AreEqual(TestRung.GetAllBetween(X1.LeftLide, X1.RightLide).Count, 1);
            Assert.AreEqual(TestRung.GetAllBetween(X1.LeftLide, X3.RightLide).Count, 4);
            Assert.AreEqual(TestRung.GetAllBetween(X1.LeftLide, X2.RightLide).Count, 5);
            Assert.AreEqual(TestRung.GetAllBetween(X1.LeftLide, Y1.RightLide).Count, 6);

        }
    }
}
