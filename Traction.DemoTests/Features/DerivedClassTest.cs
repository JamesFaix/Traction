using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics.Contracts;
using NUnit.Framework;

namespace Traction.Demo.Tests {

    [TestFixture]
    class DerivedClassTest {

        private DerivedClassDemo GetConsumer() => new DerivedClassDemo();

        [Test]
        public void DerivedClass_PreconditionMethod_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.PreconditionMethod("test");
            });
        }

        [Test]
        public void DerivedClass_PreconditionMethod_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.PreconditionMethod(null);
            });
        }

        [Test]
        public void DerivedClass_PostconditionMethod_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.PostconditionMethod("test");
            });
        }

        [Test]
        public void DerivedClass_PostconditionMethod_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() => {
                consumer.PostconditionMethod(null);
            });
        }

        [Test]
        public void DerivedClass_PreconditionProperty_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.Property = "test";
            });
        }

        [Test]
        public void DerivedClass_PreconditionProperty_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.Property = null;
            });
        }

        [Test]
        public void DerivedClass_PostconditionProperty_DoesNotThrowIfContractMet() {
            var consumer = GetConsumer();
            consumer._property = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.Property;
            });
        }

        [Test]
        public void DerivedClass_PostconditionProperty_ThrowsIfContractBroken() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.Property;
            });
        }

    }
}
