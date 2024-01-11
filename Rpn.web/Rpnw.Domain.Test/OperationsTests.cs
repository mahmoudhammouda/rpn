using Rpnw.Domain.Impl.Rpn.Operands;
using Rpnw.Domain.Impl.Rpn.Operators;
using Rpnw.Domain.Impl.Rpn.Stack;
using Rpnw.Domain.Model.Rpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Domain.Test
{
    [TestFixture]
    public class OperationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddOperator_ShouldCalculateSumCorrectly()
        {
            // Arrange
            var stack = new RpnStack();
            var addOperator = new AddOperator();
            stack.Push(new Operand(5));
            stack.Push(new Operand(3));

            // Act
            addOperator.Operate(stack);

            // Assert
            var result = stack.Pop() as IRpnOperand;
            Assert.AreEqual(8, result.Value, "5 + 3 doit etre egale a 8.");
        }

        [Test]
        public void MinuOperator_ShouldCalculateDifferenceCorrectly()
        {
            // Arrange
            var stack = new RpnStack();
            var subOperator = new MinusOperator();
            stack.Push(new Operand(5));
            stack.Push(new Operand(3));

            // Act
            subOperator.Operate(stack);

            // Assert
            var result = stack.Pop() as IRpnOperand;
            Assert.AreEqual(2, result.Value, "5 - 3 doit etre egale a 2.");
        }

        [Test]
        public void MulOperator_ShouldCalculateProductCorrectly()
        {
            // Arrange
            var stack = new RpnStack();
            var mulOperator = new MulOperator();
            stack.Push(new Operand(5));
            stack.Push(new Operand(3));

            // Act
            mulOperator.Operate(stack);

            // Assert
            var result = stack.Pop() as IRpnOperand;
            Assert.AreEqual(15, result.Value, "5 * 3 doit etre egale a 15.");
        }

        [Test]
        public void DivOperator_ShouldCalculateQuotientCorrectly()
        {
            // Arrange
            var stack = new RpnStack();
            var divOperator = new DivOperator();
            stack.Push(new Operand(15));
            stack.Push(new Operand(3));

            // Act
            divOperator.Operate(stack);

            // Assert
            var result = stack.Pop() as IRpnOperand;
            Assert.AreEqual(5, result.Value, "15 / 3 should equal 5.");
        }

        [Test]
        public void DivOperator_ShouldThrowExceptionOnDivideByZero()
        {
            // Arrange
            var stack = new RpnStack();
            var divOperator = new DivOperator();
            stack.Push(new Operand(15));
            stack.Push(new Operand(0));

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => divOperator.Operate(stack), "Division par 0 doit thrower DivideByZeroException.");
        }

        [Test]
        public void SqrtOperator_ShouldCalculateSquareRootCorrectly()
        {
            // Arrange
            var stack = new RpnStack();
            var sqrtOperator = new SqrtOperator();
            stack.Push(new Operand(16));

            // Act
            sqrtOperator.Operate(stack);

            // Assert
            var result = stack.Pop() as IRpnOperand;
            Assert.AreEqual(4, result.Value, "la recine de 16 doit etre 4.");
        }

        [Test]
        public void SqrtOperator_ShouldThrowExceptionOnNegativeInput()
        {
            // Arrange
            var stack = new RpnStack();
            var sqrtOperator = new SqrtOperator();
            stack.Push(new Operand(-16));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => sqrtOperator.Operate(stack), "la racine carré d'un chiffre negatif doit thrower un InvalidOperationException.");
        }
    }
}
