using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Data;

namespace CoreLogicalTest
{
    [TestClass]
    public class LDIVariableTableTest
    {
        [TestMethod]
        public void BasicTests()
        {
            LDIVariableTable TestTable = new LDIVariableTable();

            TestTable.Add("Var1", typeof(int));
            TestTable.Add("Var1", typeof(int), 10);
            TestTable.Add("Var2", typeof(bool));
            TestTable.Add("Var3", typeof(byte));
            TestTable.Add("Var4", typeof(int));
            TestTable.Add("Var5", typeof(int));

            Assert.AreEqual(5, TestTable.Count, "Stored Variable number incorrect");

            TestTable.Rename("Var1", "Var6");
            TestTable.Rename("Var6", "Var5");
            TestTable.Remove("Var1");

            Assert.AreEqual(4, TestTable.Count, "Stored Variable number incorrect");

            TestTable.SetValue(0, true);
        }

        [TestMethod]
        public void WrongNameTests()
        {
            LDIVariableTable TestTable = new LDIVariableTable();
            TestTable.Add("Var1", typeof(int));

            try
            {
                TestTable.GetIndexOf("Var2");
            }
            catch (ArgumentException ex )
            {
                StringAssert.Contains(ex.Message, "");
            }
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexOfRangeTest()
        {
            LDIVariableTable TestTable = new LDIVariableTable();

            TestTable.GetValue(2);
        }
    }
}
