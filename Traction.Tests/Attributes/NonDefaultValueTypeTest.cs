﻿using System;
using NUnit.Framework;
using Traction.Demo;

namespace Traction.Tests {

    [TestFixture]
    public class NonDefaultValueTypeTest {

        private NonDefaultConsumer GetConsumer() => new NonDefaultConsumer();

        #region Properties

        #region NormalProperty
        [Test]
        public void NonDefault_NormalValueTypeProperty_GetDoesNotThrow() {
            var consumer = GetConsumer();
            consumer._normalValueTypeProperty = 0;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_NormalValueTypeProperty_SetDoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalReadWriteValueTypeProperty = 0;
            });
        }
        #endregion

        #region ContractReadWriteProperty
        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_GetDoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();
            consumer._contractReadWriteValueTypeProperty = 1;

            Assert.DoesNotThrow(() => {
                var x = consumer.ContractReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_GetThrowsIfDefault() {
            var consumer = GetConsumer();
            consumer._contractReadWriteValueTypeProperty = 0;

            Assert.Throws<PostconditionException>(() => {
                var x = consumer.ContractReadWriteValueTypeProperty;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_SetDoesNotThrowIfNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ContractReadWriteValueTypeProperty = 1;
            });
        }

        [Test]
        public void NonDefault_ContractReadWriteValueTypeProperty_SetThrowsIfDefault() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() => {
                consumer.ContractReadWriteValueTypeProperty = 0;
            });
        }
        #endregion

        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonDefault_NormalMethod_DoesNotThrow() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region PreconditionMethod
        [Test]
        public void NonDefault_PreconditionValueTypeMethod_DoesNotThrowIfArgNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreconditionValueTypeMethod(1));
        }

        [Test]
        public void NonDefault_PreconditionValueTypeMethod_ThrowsIfArgDefault() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreconditionValueTypeMethod(0));
        }
        #endregion

        #region PostconditionMethod
        [Test]
        public void NonDefault_PostconditionValueTypeMethod_DoesNotThrowIfResultNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PostconditionValueTypeMethod(1));
        }

        [Test]
        public void NonDefault_PostconditionValueTypeMethod_ThrowsIfResultDefault() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PostconditionValueTypeMethod(0));
        }
        #endregion

        #region PreAndPostconditionMethod
        [Test]
        public void NonDefault_PreAndPostconditioValueTypenMethod_DoesNotThrowIfArgAndResultNotDefault() {
            var consumer = GetConsumer();

            Assert.DoesNotThrow(() =>
                consumer.PreAndPostconditionValueTypeMethod(100));
        }

        [Test]
        public void NonDefault_PreAndPostconditionValueTypeMethod_ThrowsIfArgDefault() {
            var consumer = GetConsumer();

            Assert.Throws<PreconditionException>(() =>
                consumer.PreAndPostconditionValueTypeMethod(0));
        }

        [Test]
        public void NonDefault_PreAndPostconditionValueTypeMethod_ThrowsIfResultDefault() {
            var consumer = GetConsumer();

            Assert.Throws<PostconditionException>(() =>
                consumer.PreAndPostconditionValueTypeMethod(1));
        }
        #endregion

        #endregion
    }
}
