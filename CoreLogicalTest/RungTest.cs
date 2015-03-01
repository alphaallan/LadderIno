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


            TestRung.DataTable = new Core.Data.LadderDataTable();

            Trace.WriteLine("Cycle 1", "Unit Test");
            TestRung.Execute();

            Trace.WriteLine("Cycle 2", "Unit Test");
            TestRung.DataTable.SetValue("X1", true);
            TestRung.Execute();

            Trace.WriteLine("Cycle 3", "Unit Test");
            TestRung.DataTable.SetValue("X1", false);
            TestRung.Execute();

            Trace.WriteLine("Cycle 4", "Unit Test");
            TestRung.Execute();

            Trace.WriteLine("Cycle 5", "Unit Test");
            TestRung.DataTable.SetValue("X2", true);
            TestRung.Execute();

            Trace.WriteLine("END", "Unit Test");
            TestRung.Clear();
            TestRung = null;
            GC.Collect();
        }

        [TestMethod]
        public void ComponentSelfAllocDealoc()
        {
            var DataTable = new Core.Data.LadderDataTable();

            ComponentAlloc(DataTable);
            GC.Collect();
            Trace.WriteLine(DataTable.Count);
        }

        private void ComponentAlloc(Core.Data.LadderDataTable table)
        {
            var t = new Coil();
            t.DataTable = table;
            t.Name = "TEST";
            t.Type = Coil.CoilType.InternalRelay;
            //t.DataTable = null;
        }
    }
}
