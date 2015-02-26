using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Data;

namespace CoreLogicalTest
{
    [TestClass]
    public class LDIVariableTableTest
    {
        LDIVariableTable TestTable { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            TestTable = new LDIVariableTable();
            TestTable.Add("Var1", typeof(int));
            TestTable.Add("Var1", typeof(int), 10);
        }

        
    }
}
