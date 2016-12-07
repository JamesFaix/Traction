using System;
using System.Collections;
using NUnit.Framework;

namespace Traction.Tests {

    [TestFixture]
    public class NonNullTest {

        #region Properties

        #region Normal Property
        [Test]
        public void NonNull_NormalProperty_GetDoesNotThrow() {
            var consumer = new NonNullConsumer();
            consumer._normalProperty = null;

            Assert.DoesNotThrow(() => {
                var x = consumer.NormalProperty;
            });
        }

        [Test]
        public void NonNull_NormalProperty_SetDoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.NormalProperty = null;
            });
        }
        #endregion

        #region ReadonlyPropertyWithContract
        [Test]
        public void NonNull_ReadonlyPropertyWithContract_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readonlyPropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ReadonlyPropertyWithContract;
            });
        }

        [Test]
        public void NonNull_ReadonlyPropertyWithContract_ThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readonlyPropertyWithContract = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ReadonlyPropertyWithContract;
            });
        }
        #endregion

        #region WriteonlyPropertyWithContract
        [Test]
        public void NonNull_WriteonlyPropertyWithContract_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.WriteonlyPropertyWithContract = "test";
            });
        }

        [Test]
        public void NonNull_WriteonlyPropertyWithContract_ThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                consumer.WriteonlyPropertyWithContract = null;
            });
        }
        #endregion

        #region ReadWritePropertyWithContract
        [Test]
        public void NonNull_ReadWritePropertyWithContract_GetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = "test";

            Assert.DoesNotThrow(() => {
                var x = consumer.ReadWritePropertyWithContract;
            });
        }

        [Test]
        public void NonNull_ReadWritePropertyWithContract_GetThrowsIfNull() {
            var consumer = new NonNullConsumer();
            consumer._readWritePropertyWithContract = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer.ReadWritePropertyWithContract;
            });
        }

        [Test]
        public void NonNull_ReadWritePropertyWithContract_SetDoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                consumer.ReadWritePropertyWithContract = "test";
            });
        }

        [Test]
        public void NonNull_ReadWritePropertyWithContract_SetThrowsIfNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                consumer.ReadWritePropertyWithContract = null;
            });
        }
        #endregion

        #region ReadonlyPropertyWithContractAndMultipleReturns
        [Test]
        public void NonNull_ReadonlyPropertyWithContractAndMultipleReturns_DoesNotThrowIfNotNull() {
            var consumer = new NonNullConsumer();
            //This property internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = "test1";
            consumer._returnValue2 = "test2";

            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                var x = consumer.ReadonlyPropertyWithContractAndMultipleReturns;
                var y = consumer.ReadonlyPropertyWithContractAndMultipleReturns;
                Assert.AreNotEqual(x, y);
            });
        }

        [Test]
        public void NonNull_ReadonlyPropertyWithContractAndMultipleReturns_ThrowsIfNull() {
            var consumer = new NonNullConsumer();
            //This property internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = null;
            consumer._returnValue2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<ReturnValueException>(() => {
                    var x = consumer.ReadonlyPropertyWithContractAndMultipleReturns;
                });
            }
        }

        #endregion
        #endregion

        #region Methods

        #region NormalMethod
        [Test]
        public void NonNull_NormalMethod_DoesNotThrow() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.NormalMethod());
        }
        #endregion

        #region MethodWithPrecondition
        [Test]
        public void NonNull_MethodWithPrecondition_DoesNotThrowIfArgNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MethodWithPrecondition("test"));
        }

        [Test]
        public void NonNull_MethodWithPrecondition_ThrowsIfArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() =>
                consumer.MethodWithPrecondition(null));
        }
        #endregion

        #region MethodWithPostcondition
        [Test]
        public void NonNull_MethodWithPostcondition_DoesNotThrowIfResultNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MethodWithPostcondition("test"));
        }

        [Test]
        public void NonNull_MethodWithPostcondition_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ReturnValueException>(() =>
                consumer.MethodWithPostcondition(null));
        }
        #endregion

        #region MethodWithPreAndPostcondition
        [Test]
        public void NonNull_MethodWithPreAndPostcondition_DoesNotThrowIfArgAndResultNotNull() {
            var consumer = new NonNullConsumer();
            Func<string> textGenerator = () => "test";

            Assert.DoesNotThrow(() =>
                consumer.MethodWithPreAndPostcondition(textGenerator));
        }

        [Test]
        public void NonNull_MethodWithPreAndPostcondition_ThrowsIfArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() =>
                consumer.MethodWithPreAndPostcondition(null));
        }

        [Test]
        public void NonNull_MethodWithPreAndPostcondition_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();
            Func<string> textGenerator = () => null;

            Assert.Throws<ReturnValueException>(() =>
                consumer.MethodWithPreAndPostcondition(textGenerator));
        }
        #endregion

        #region MethodWithMultiplePreconditions[Test]
        [Test]
        public void NonNull_MethodWithMultiplePreconditions_DoesNotThrowIfNoArgsNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() =>
                consumer.MethodWithMultiplePreconditions("test", "testing"));
        }

        [Test]
        public void NonNull_MethodWithMultiplePreconditions_ThrowsIfAnyArgNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() =>
                consumer.MethodWithMultiplePreconditions("test", null));

            Assert.Throws<ArgumentNullException>(() =>
                consumer.MethodWithMultiplePreconditions(null, "test"));
        }
        #endregion

        #region MethodWithPostconditionAndMultipleReturns
        [Test]
        public void NonNull_MethodWithPostconditionAndMultipleReturns_DoesNotThrowIfResultsNonNull() {
            var consumer = new NonNullConsumer();
            //This method internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = "test1";
            consumer._returnValue2 = "test2";

            Assert.DoesNotThrow(() => {
                //Getting value twice will hit check return statements.
                consumer.MethodWithPostconditionAndMultipleReturns();
                consumer.MethodWithPostconditionAndMultipleReturns();
            });
        }

        [Test]
        public void NonNull_MethodWithPostconditionAndMultipleReturns_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();
            //This method internally toggles between returning two fields from two return statements.
            consumer._returnValue1 = null;
            consumer._returnValue2 = null;

            //Getting value twice will hit check return statements.
            for (var i = 0; i <= 1; i++) {
                Assert.Throws<ReturnValueException>(() => {
                    consumer.MethodWithPostconditionAndMultipleReturns();
                });
            }
        }
        #endregion
        #endregion

        #region Operators

        #region Normal operator (+)
        [Test]
        public void NonNull_NormalOperator_DoesNotThrow() {
            var consumer1 = new NonNullConsumer();
            var consumer2 = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 + consumer2;
            });
        }
        #endregion

        #region Precondition operator (-)
        [Test]
        public void NonNull_OperatorWithPrecondition_DoesNotThrowIfArgNotNull() {
            var consumer1 = new NonNullConsumer();
            var consumer2 = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 - consumer2;
            });
        }

        [Test]
        public void NonNull_OperatorWithPrecondition_ThrowsIfArgNull() {
            NonNullConsumer consumer1 = null;
            var consumer2 = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                var x = consumer1 - consumer2;
            });
        }
        #endregion

        #region Postcondition operator (*)
        // * returns null if either parameter is null

        [Test]
        public void NonNull_OperatorWithPostcondition_DoesNotThrowIfResultNotNull() {
            var consumer1 = new NonNullConsumer();
            var consumer2 = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 * consumer2;
            });
        }

        [Test]
        public void NonNull_OperatorWithPostcondition_ThrowsIfResultNull() {
            var consumer1 = new NonNullConsumer();
            NonNullConsumer consumer2 = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer1 * consumer2;
            });
        }
        #endregion

        #region Pre and Postcondition operator (/)
        // / returns null if both arguments are equal

        [Test]
        public void NonNull_OperatorWithPreAndPostcondition_DoesNotThrowIfArgAndResultNotNull() {
            var consumer1 = new NonNullConsumer();
            var consumer2 = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                var x = consumer1 / consumer2;
            });
        }

        [Test]
        public void NonNull_OperatorWithPreAndPostcondition_ThrowsIfArgNull() {
            NonNullConsumer consumer1 = null;
            var consumer2 = new NonNullConsumer();

            Assert.Throws<ArgumentNullException>(() => {
                var x = consumer1 / consumer2;
            });
        }

        [Test]
        public void NonNull_OperatorWithPreAndPostcondition_ThrowsIfResultNull() {
            var consumer1 = new NonNullConsumer();

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer1 / consumer1;
            });
        }
        #endregion

        #region Postcondition operator with multiple returns (%)
        // % returns different values depending on whether the arguments are equal

        [Test]
        public void NonNull_OperatorWithPostconditionAndMultipleReturns_DoesNotThrowIfResultsNonNull() {
            var consumer1 = new NonNullConsumer();
            var consumer2 = new NonNullConsumer();
            consumer1._returnValue1 = "test";
            consumer1._returnValue2 = "testing";

            Assert.DoesNotThrow(() => {
                var x = consumer1 % consumer2;
            });
            Assert.DoesNotThrow(() => {
                var x = consumer1 % consumer1;
            });
        }

        [Test]
        public void NonNull_OperatorWithPostconditionAndMultipleReturns_ThrowsIfResultNull() {
            var consumer1 = new NonNullConsumer();
            var consumer2 = new NonNullConsumer();
            consumer1._returnValue1 = null;
            consumer1._returnValue2 = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = consumer1 % consumer2;
            });
            Assert.Throws<ReturnValueException>(() => {
                var x = consumer1 % consumer1;
            });
        }
        #endregion
        #endregion

        #region Conversions

        #region NormalConversion (int)
        [Test]
        public void NonNull_NormalConversion_DoesNotThrow() {
            NonNullConsumer consumer = null;

            Assert.DoesNotThrow(() => {
                var x = (int)consumer;
            });
        }
        #endregion

        #region Precondition conversion (long)
        [Test]
        public void NonNull_ConversionWithPrecondition_DoesNotThrowIfArgNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                var x = (long)consumer;
            });
        }

        [Test]
        public void NonNull_ConversionWithPrecondition_ThrowsIfArgNull() {
            NonNullConsumer consumer = null;

            Assert.Throws<ArgumentNullException>(() => {
                var x = (long)consumer;
            });
        }
        #endregion

        #region Postcondition conversion (string)
        // returns null if arg is null

        [Test]
        public void NonNull_ConversionWithPostcondition_DoesNotThrowIfResultNotNull() {
            var consumer = new NonNullConsumer();

            Assert.DoesNotThrow(() => {
                var x = (string)consumer;
            });
        }

        [Test]
        public void NonNull_ConversionWithPostcondition_ThrowsIfResultNull() {
            NonNullConsumer consumer = null;

            Assert.Throws<ReturnValueException>(() => {
                var x = (string)consumer;
            });
        }
        #endregion

        #region Pre and Postcondition conversion (ArrayList)
        //returns null if argument's _returnValue1 field is null

        [Test]
        public void NonNull_ConversionWithPreAndPostcondition_DoesNotThrowIfArgAndResultNotNull() {
            var consumer = new NonNullConsumer();
            consumer._returnValue1 = "test";

            Assert.DoesNotThrow(() => {
                var x = (ArrayList)consumer;
            });
        }

        [Test]
        public void NonNull_ConversionWithPreAndPostcondition_ThrowsIfArgNull() {
            NonNullConsumer consumer = null;

            Assert.Throws<ArgumentNullException>(() => {
                var x = (ArrayList)consumer;
            });
        }

        [Test]
        public void NonNull_ConversionWithPreAndPostcondition_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();

            Assert.Throws<ReturnValueException>(() => {
                var x = (ArrayList)consumer;
            });
        }
        #endregion

        #region Post with multiple returns (int[])
        //chooses argument's _returnValue1 or _returnValue2, the first that is not null
        //returns int array that is length of selected field, or null if field is empty string

        [Test]
        public void NonNull_ConversionWithPostconditionAndMultipleReturns_DoesNotThrowIfResultsNonNull() {
            var consumer = new NonNullConsumer();
            consumer._returnValue1 = "test";
            
            Assert.DoesNotThrow(() => {
                var x = (int[])consumer;
            });

            consumer._returnValue1 = null;
            consumer._returnValue2 = "test";

            Assert.DoesNotThrow(() => {
                var x = (int[])consumer;
            });
        }

        [Test]
        public void NonNull_ConversionWithPostconditionAndMultipleReturns_ThrowsIfResultNull() {
            var consumer = new NonNullConsumer();
            consumer._returnValue1 = "";

            Assert.Throws<ReturnValueException>(() => {
                var x = (int[])consumer;
            });

            consumer._returnValue1 = null;
            consumer._returnValue2 = "";

            Assert.Throws<ReturnValueException>(() => {
                var x = (int[])consumer;
            });
        }
        #endregion
        #endregion
    }
}
