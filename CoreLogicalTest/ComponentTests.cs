using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Data;
using Core.Components;
using System.Diagnostics;

namespace CoreLogicalTest
{
    [TestClass]
    public class ComponentTests
    {
        [TestMethod]
        [TestCategory("Component")]
        public void Basic()
        {
            Node PowerRail = new Node();
            PowerRail.LogicLevel = false;

            Coil coil = new Coil(PowerRail);
            Contact contact = new Contact();
            contact.LeftLide = PowerRail;
            OSF osf = new OSF();
            osf.LeftLide = PowerRail;
            OSR osr = new OSR();
            osr.LeftLide = PowerRail;
            ShortCircuit sc = new ShortCircuit(PowerRail);

            #region Coil
            coil.Execute();
            Assert.IsFalse(coil.InternalState);
            PowerRail.LogicLevel = true;
            Assert.IsFalse(coil.InternalState);
            coil.Execute();
            Assert.IsTrue(coil.InternalState);

            PowerRail.LogicLevel = false;
            Assert.IsTrue(coil.InternalState);
            coil.Execute();
            Assert.IsFalse(coil.InternalState);

            coil.Mode = Coil.CoilMode.Negated;
            Assert.IsFalse(coil.InternalState);
            coil.Execute();
            Assert.IsTrue(coil.InternalState);

            PowerRail.LogicLevel = true;
            coil.Execute();
            Assert.IsFalse(coil.InternalState);
            
            coil.Mode = Coil.CoilMode.Set;
            PowerRail.LogicLevel = false;
            coil.Execute();
            coil.Execute();
            Assert.IsFalse(coil.InternalState);

            PowerRail.LogicLevel = true;
            coil.Execute();
            Assert.IsTrue(coil.InternalState);
            PowerRail.LogicLevel = false;
            coil.Execute();
            Assert.IsTrue(coil.InternalState);

            coil.Mode = Coil.CoilMode.Reset;
            coil.Execute();
            Assert.IsTrue(coil.InternalState);
            PowerRail.LogicLevel = true;
            coil.Execute();
            Assert.IsFalse(coil.InternalState);
            PowerRail.LogicLevel = false;
            coil.Execute();
            Assert.IsFalse(coil.InternalState);
            PowerRail.LogicLevel = true;
            coil.Execute();
            Assert.IsFalse(coil.InternalState);
            #endregion Coil

            #region Contact
            PowerRail.LogicLevel = true;
            contact.Execute();
            Assert.IsFalse(contact.InternalState);
            
            contact.IsClosed = true;
            contact.Execute();
            Assert.IsTrue(contact.InternalState);

            PowerRail.LogicLevel = false;
            contact.Execute();
            Assert.IsFalse(contact.InternalState);

            contact.IsInverted = true;
            contact.Execute();
            Assert.IsFalse(contact.InternalState);

            PowerRail.LogicLevel = true;
            contact.Execute();
            Assert.IsFalse(contact.InternalState);

            contact.IsClosed = false;
            contact.Execute();
            Assert.IsTrue(contact.InternalState);
            #endregion Contact

            #region Short Circuit
            PowerRail.LogicLevel = false;
            sc.Execute();
            Assert.IsFalse(sc.InternalState);
            PowerRail.LogicLevel = true;
            sc.Execute();
            Assert.IsTrue(sc.InternalState);
            #endregion Short Circuit

            #region OSF
            PowerRail.LogicLevel = false;
            osf.Execute();
            Assert.IsFalse(osf.InternalState);

            PowerRail.LogicLevel = true;
            osf.Execute();
            Assert.IsFalse(osf.InternalState);
            
            PowerRail.LogicLevel = false;
            osf.Execute();
            Assert.IsTrue(osf.InternalState);
            osf.Execute();
            Assert.IsFalse(osf.InternalState);
            #endregion OSF

            #region OSR
            PowerRail.LogicLevel = false;
            osr.Execute();
            Assert.IsFalse(osr.InternalState);

            PowerRail.LogicLevel = true;
            osr.Execute();
            Assert.IsTrue(osr.InternalState);

            osr.Execute();
            Assert.IsFalse(osr.InternalState);

            PowerRail.LogicLevel = false;
            osr.Execute();
            Assert.IsFalse(osr.InternalState);
            #endregion OSR
        }

        [TestMethod]
        [TestCategory("Component")]
        public void Analog()
        {
            var TestTable = new LadderDataTable();
            Node PowerRail = new Node();
            PowerRail.LogicLevel = false;

            PWM pwm = new PWM(PowerRail);
            pwm.DataTable = TestTable;
            ADC adc = new ADC("1",PowerRail);
            adc.DataTable = TestTable;

            #region PWM
            pwm.DudyCycle = "255";
            pwm.Execute();
            Assert.IsFalse(pwm.InternalState);

            PowerRail.LogicLevel = true;
            pwm.Execute();
            Assert.IsTrue(pwm.InternalState);

            PowerRail.LogicLevel = false;
            pwm.DudyCycle = "DudyCycle";
            pwm.DudyCycleValue = (byte)128;
            pwm.Execute();
            Assert.IsFalse(pwm.InternalState);

            PowerRail.LogicLevel = true;
            pwm.Execute();
            Assert.IsTrue(pwm.InternalState);
            #endregion PWM

            #region ADC
            PowerRail.LogicLevel = false;
            adc.Execute();
            Assert.IsFalse(adc.InternalState);
            adc.InputValue = (short)1023;
            adc.Execute();
            Assert.IsFalse(adc.InternalState);

            PowerRail.LogicLevel = true;
            adc.Execute();
            Assert.IsTrue(adc.InternalState);
            #endregion ADC
        }

        [TestMethod]
        [TestCategory("Component")]
        public void Compare()
        {
            var TestTable = new LadderDataTable();
            Node PowerRail = new Node();

            #region Declarações
            EQU equ = new EQU();
            equ.LeftLide = PowerRail;
            equ.NameA = "VarA";
            equ.NameB = "VarB";
            equ.DataTable = TestTable;

            GEQ geq = new GEQ();
            geq.LeftLide = PowerRail;
            geq.NameA = "VarA";
            geq.NameB = "VarB";
            geq.DataTable = TestTable;

            GRT grt = new GRT();
            grt.LeftLide = PowerRail;
            grt.NameA = "VarA";
            grt.NameB = "VarB";
            grt.DataTable = TestTable;

            LEG leg = new LEG();
            leg.LeftLide = PowerRail;
            leg.NameA = "VarA";
            leg.NameB = "VarB";
            leg.DataTable = TestTable;

            LES les = new LES();
            les.LeftLide = PowerRail;
            les.NameA = "VarA";
            les.NameB = "VarB";
            les.DataTable = TestTable;

            NEQ neq = new NEQ();
            neq.LeftLide = PowerRail;
            neq.NameA = "VarA";
            neq.NameB = "VarB";
            neq.DataTable = TestTable;
            #endregion Declarações

            #region EQU
            Trace.WriteLine("EQU", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("Input False", "EQU");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(equ.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(equ.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsFalse(equ.Execute());
            Trace.Unindent();

            Trace.WriteLine("Input True", "EQU");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsTrue(equ.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(equ.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsFalse(equ.Execute());
            Trace.Unindent();
            Trace.Unindent();
            #endregion EQU

            #region GEQ
            Trace.WriteLine("GEQ", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("Input False", "GEQ");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(geq.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(geq.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsFalse(geq.Execute());
            Trace.Unindent();

            Trace.WriteLine("Input True", "GEQ");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsTrue(geq.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(geq.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsTrue(geq.Execute());
            Trace.Unindent();
            Trace.Unindent();
            #endregion GEQ

            #region GRT
            Trace.WriteLine("GRT", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("Input False", "GRT");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            grt.Execute();
            Assert.IsFalse(grt.InternalState);

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            grt.Execute();
            Assert.IsFalse(grt.InternalState);

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            grt.Execute();
            Assert.IsFalse(grt.InternalState);
            Trace.Unindent();

            Trace.WriteLine("Input True", "GRT");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            grt.Execute();
            Assert.IsFalse(grt.InternalState);

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            grt.Execute();
            Assert.IsFalse(grt.InternalState);

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            grt.Execute();
            Assert.IsTrue(grt.InternalState);
            Trace.Unindent();
            Trace.Unindent();
            #endregion GRT

            #region LEG
            Trace.WriteLine("LEG", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("Input False", "LEG");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            leg.Execute();
            Assert.IsFalse(leg.InternalState);

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            leg.Execute();
            Assert.IsFalse(leg.InternalState);

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            leg.Execute();
            Assert.IsFalse(leg.InternalState);
            Trace.Unindent();

            Trace.WriteLine("Input True", "LEG");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            leg.Execute();
            Assert.IsTrue(leg.InternalState);

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            leg.Execute();
            Assert.IsTrue(leg.InternalState);

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            leg.Execute();
            Assert.IsFalse(leg.InternalState);
            Trace.Unindent();
            Trace.Unindent();
            #endregion LEG

            #region LES
            Trace.WriteLine("LES", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("Input False", "LES");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(les.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(les.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsFalse(les.Execute());
            Trace.Unindent();

            Trace.WriteLine("Input True", "LES");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(les.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsTrue(les.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsFalse(les.Execute());
            Trace.Unindent();
            Trace.Unindent();
            #endregion LES

            #region NEQ
            Trace.WriteLine("NEQ", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("Input False", "NEQ");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(neq.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(neq.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsFalse(neq.Execute());
            Trace.Unindent();

            Trace.WriteLine("Input True", "NEQ");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            
            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)1);
            Assert.IsFalse(neq.Execute());

            TestTable.SetValue(0, (short)0);
            TestTable.SetValue(1, (short)1);
            Assert.IsTrue(neq.Execute());

            TestTable.SetValue(0, (short)1);
            TestTable.SetValue(1, (short)0);
            Assert.IsTrue(neq.Execute());
            Trace.Unindent();
            Trace.Unindent();
            #endregion NEQ
        }
    }
}
