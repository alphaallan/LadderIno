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
        public void BasicComponents()
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
    }
}
