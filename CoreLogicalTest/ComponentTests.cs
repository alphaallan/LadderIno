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
            #region Startup
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
            #endregion Startup

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
            #region Startup
            var TestTable = new LadderDataTable();
            Node PowerRail = new Node();
            PowerRail.LogicLevel = false;

            PWM pwm = new PWM(PowerRail);
            pwm.DataTable = TestTable;
            ADC adc = new ADC("1",PowerRail);
            adc.DataTable = TestTable;
            #endregion Startup

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
            #region Startup
            var TestTable = new LadderDataTable();
            Node PowerRail = new Node();

            EQU equ = new EQU();
            equ.LeftLide = PowerRail;
            equ.VarA = "VarA";
            equ.VarB = "VarB";
            equ.DataTable = TestTable;

            GEQ geq = new GEQ();
            geq.LeftLide = PowerRail;
            geq.VarA = "VarA";
            geq.VarB = "VarB";
            geq.DataTable = TestTable;

            GRT grt = new GRT();
            grt.LeftLide = PowerRail;
            grt.VarA = "VarA";
            grt.VarB = "VarB";
            grt.DataTable = TestTable;

            LEG leg = new LEG();
            leg.LeftLide = PowerRail;
            leg.VarA = "VarA";
            leg.VarB = "VarB";
            leg.DataTable = TestTable;

            LES les = new LES();
            les.LeftLide = PowerRail;
            les.VarA = "VarA";
            les.VarB = "VarB";
            les.DataTable = TestTable;

            NEQ neq = new NEQ();
            neq.LeftLide = PowerRail;
            neq.VarA = "VarA";
            neq.VarB = "VarB";
            neq.DataTable = TestTable;
            #endregion Startup

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

        [TestMethod]
        [TestCategory("Component")]
        public void Counter()
        {
            #region Startup
            var TestTable = new LadderDataTable();
            Node PowerRail = new Node();

            CTC ctc = new CTC();
            ctc.Name = "1";
            ctc.Limit = "Limit";
            ctc.LeftLide = PowerRail;
            ctc.DataTable = TestTable;

            CTD ctd = new CTD();
            ctd.Name = "1";
            ctd.Limit = "Limit";
            ctd.LeftLide = PowerRail;
            ctd.DataTable = TestTable;

            CTU ctu = new CTU();
            ctu.Name = "1";
            ctu.Limit = "Limit";
            ctu.LeftLide = PowerRail;
            ctu.DataTable = TestTable;

            RES res = new RES();
            res.Name = "1";
            res.LeftLide = PowerRail;
            res.DataTable = TestTable;
            #endregion Startup

            #region CTC
            Trace.WriteLine("CTC", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("StartUP", "CTC");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            ctc.CurrentValue = 1;
            ctc.LimitValue = 2;
            res.Execute();
            Trace.Unindent();

            Trace.WriteLine("Input False", "CTC");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            ctc.Execute();
            ctc.Execute();
            ctc.Execute();
            Assert.IsFalse(ctc.InternalState);
            Trace.Unindent();

            Trace.WriteLine("Input True", "CTC");
            Trace.Indent();

            PowerRail.LogicLevel = false;
            ctc.Execute();
            PowerRail.LogicLevel = true;
            ctc.Execute();
            Assert.IsFalse(ctc.InternalState, "Fail First Cycle");

            PowerRail.LogicLevel = false;
            ctc.Execute();
            PowerRail.LogicLevel = true;
            ctc.Execute();
            Assert.IsTrue(ctc.InternalState, "Fail Second Cycle");

            PowerRail.LogicLevel = false;
            ctc.Execute();
            PowerRail.LogicLevel = true;
            ctc.Execute();
            Assert.IsFalse(ctc.InternalState, "Fail Third Cycle");

            PowerRail.LogicLevel = false;
            ctc.Execute();
            PowerRail.LogicLevel = true;
            ctc.Execute();
            Assert.IsFalse(ctc.InternalState, "Fail Fourth Cycle");

            Trace.Unindent();
            Trace.Unindent();
            #endregion CTC

            #region CTD
            Trace.WriteLine("CTD", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("StartUP", "CTD");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            ctc.CurrentValue = 3;
            ctc.LimitValue = 1;
            Trace.Unindent();

            Trace.WriteLine("Input False", "CTD");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            ctd.Execute();
            ctd.Execute();
            ctd.Execute();
            Assert.IsFalse(ctc.InternalState);
            Trace.Unindent();

            Trace.WriteLine("Input True", "CTD");
            Trace.Indent();

            PowerRail.LogicLevel = false;
            ctd.Execute();
            PowerRail.LogicLevel = true;
            ctd.Execute();
            Assert.IsTrue(ctd.InternalState, "Fail First Cycle");

            PowerRail.LogicLevel = false;
            ctd.Execute();
            PowerRail.LogicLevel = true;
            ctd.Execute();
            Assert.IsTrue(ctd.InternalState, "Fail Second Cycle");

            PowerRail.LogicLevel = false;
            ctd.Execute();
            PowerRail.LogicLevel = true;
            ctd.Execute();
            Assert.IsFalse(ctd.InternalState, "Fail Third Cycle");

            PowerRail.LogicLevel = false;
            ctd.Execute();
            PowerRail.LogicLevel = true;
            ctd.Execute();
            Assert.IsFalse(ctd.InternalState, "Fail Fourth Cycle");

            Trace.Unindent();
            Trace.Unindent();
            #endregion CTD

            #region CTU
            Trace.WriteLine("CTU", "Unit Test");
            Trace.Indent();

            Trace.WriteLine("StartUP", "CTU");
            Trace.Indent();
            PowerRail.LogicLevel = true;
            ctu.CurrentValue = 1;
            ctu.LimitValue = 3;
            res.Execute();
            Trace.Unindent();

            Trace.WriteLine("Input False", "CTU");
            Trace.Indent();
            PowerRail.LogicLevel = false;
            ctu.Execute();
            ctu.Execute();
            ctu.Execute();
            Assert.IsFalse(ctu.InternalState);
            Trace.Unindent();

            Trace.WriteLine("Input True", "CTU");
            Trace.Indent();

            PowerRail.LogicLevel = false;
            ctu.Execute();
            PowerRail.LogicLevel = true;
            ctu.Execute();
            Assert.IsFalse(ctu.InternalState, "Fail First Cycle");

            PowerRail.LogicLevel = false;
            ctu.Execute();
            PowerRail.LogicLevel = true;
            ctu.Execute();
            Assert.IsFalse(ctu.InternalState, "Fail Second Cycle");

            PowerRail.LogicLevel = false;
            ctu.Execute();
            PowerRail.LogicLevel = true;
            ctu.Execute();
            Assert.IsTrue(ctu.InternalState, "Fail Third Cycle");

            PowerRail.LogicLevel = false;
            ctu.Execute();
            Assert.IsFalse(ctu.InternalState, "Fail Fourth Cycle");
            PowerRail.LogicLevel = true;
            ctu.Execute();
            Assert.IsTrue(ctu.InternalState, "Fail Fourth Cycle");

            Trace.Unindent();
            Trace.Unindent();
            #endregion CTU
        }
    }
}
