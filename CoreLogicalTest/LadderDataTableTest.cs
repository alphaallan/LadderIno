using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Data;

namespace CoreLogicalTest
{
    [TestClass]
    public class LadderDataTableTest
    {
        [TestMethod]
        [TestCategory("DataTable")]
        public void BasicTests()
        {
            LadderDataTable TestTable = new LadderDataTable();

            TestTable.Add("Var1", typeof(int));
            TestTable.Add("Var1", typeof(int), 10);
            TestTable.Add("Var2", typeof(bool));
            TestTable.Add("Var3", typeof(byte));
            TestTable.Add("Var4", typeof(short));
            TestTable.Add("Var5", typeof(int));

            Assert.AreEqual(5, TestTable.Count, "Stored Variable number incorrect");

            TestTable.Rename("Var1", "Var6");
            TestTable.Rename("Var6", "Var5");
            TestTable.Remove("Var1");

            TestTable.SetValue(0, true);
            TestTable.SetValue("Var3",(byte)255);
            TestTable.SetValue(TestTable.GetIndexOf("Var4"),(short)777);

            Assert.AreEqual(4, TestTable.Count, "Stored Variable number incorrect");

        }

        [TestMethod]
        [TestCategory("DataTable")]
        public void WrongArgsTests()
        {
            LadderDataTable TestTable = new LadderDataTable();
            TestTable.Add("Var1", typeof(int));

            try
            {
                TestTable.Add("", typeof(int));
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "name");
            }

            try
            {
                TestTable.Add("Var2", null);
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "type");
            }

            try
            {
                TestTable.Add("", typeof(int),10);
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "name");
            }

            try
            {
                TestTable.Add("Var2", null,10);
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "type");
            }

            try
            {
                TestTable.Remove("");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "name");
            }

            try
            {
                TestTable.Remove("Var3");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.ParamName, "name");
            }

            try
            {
                TestTable.Rename("", "Var4");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "oldName");
            }

            try
            {
                TestTable.Rename("Var3", "");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Equals(ex.ParamName, "newName");
            }

            try
            {
                TestTable.Rename("Var3", "Var4");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.Message, "Variable not found");
                StringAssert.Equals(ex.ParamName, "oldName");
            }

            try
            {
                TestTable.GetIndexOf("Var3");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.Message, "Variable not found");
                StringAssert.Equals(ex.ParamName, "name");
            }

            try
            {
                TestTable.GetValue("Var3");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.Message, "Variable not found");
                StringAssert.Equals(ex.ParamName, "name");
            }
        }

        [TestMethod]
        [TestCategory("DataTable")]
        public void TypeSafetyTests()
        {
            LadderDataTable TestTable = new LadderDataTable();
            TestTable.Add("Var1", typeof(int));
            TestTable.Add("Var2", typeof(bool));

            try
            {
                TestTable.Add("Var1", typeof(bool));
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.ParamName, "name");
                Assert.IsInstanceOfType(ex.InnerException, typeof(FormatException));
            }

            try
            {
                TestTable.Add("Var1", typeof(bool), false);
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.ParamName , "name");
                Assert.IsInstanceOfType(ex.InnerException, typeof(FormatException));
            }

            try
            {
                TestTable.Add("Var3", typeof(int), null);
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains(ex.Message, "non-nullable");
                StringAssert.Equals(ex.ParamName, "value");
            }

            try
            {
                TestTable.Rename("Var2", "Var1");
                throw new AssertFailedException("Exception expected");
            }
            catch (ArgumentException ex)
            {
                StringAssert.Equals(ex.ParamName, "newName");
                Assert.IsInstanceOfType(ex.InnerException, typeof(FormatException));
            }

            try
            {
                TestTable.SetValue("Var1", "Var1");
                throw new AssertFailedException("Exception expected");
            }
            catch (FormatException ex)
            {
                StringAssert.Equals(ex.Message, "Value Type Mismatch");
            }

            try
            {
                TestTable.SetValue(0, "Var1");
                throw new AssertFailedException("Exception expected");
            }
            catch (FormatException ex)
            {
                StringAssert.Equals(ex.Message, "Value Type Mismatch");
            }
        }

        [TestMethod]
        [TestCategory("DataTable")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IndexOfRangeTest()
        {
            LadderDataTable TestTable = new LadderDataTable();

            TestTable.GetValue(1);
        }
    }
}
