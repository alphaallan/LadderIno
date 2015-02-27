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
        public void RungTest1()
        {
            Rung TestRung = new Rung();
            TestRung.DataTable = new Core.Data.LadderDataTable();

            Coil temp = new Coil();
            temp.Name = "1";
            TestRung.Add(temp);
            TestRung.Add(new Coil());
            TestRung.Add(new OSR());
            TestRung.Add(new Contact());
            temp.Name = "2";

            TestRung.DataTable.SetValue("XNEW", true);
            TestRung.Execute();

            TestRung.Remove(temp);
            temp = null;
            TestRung.Execute();
            TestRung.Execute();

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
