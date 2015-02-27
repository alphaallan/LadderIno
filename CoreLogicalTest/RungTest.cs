using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Components;

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
            TestRung.Execute();
            TestRung.Execute();
        }
    }
}
