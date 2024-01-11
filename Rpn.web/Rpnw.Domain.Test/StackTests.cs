using Rpnw.Domain.Impl.Rpn.Operands;
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
    public class RpnStackTests
    {
        private IRpnStack stack;

        [SetUp]
        public void SetUp()
        {
            stack = new RpnStack();
        }

        [Test]
        public void Push_ShouldAddElementToStack()
        {
            // Arrange
            var operand = new Operand(5);

            // Act
            stack.Push(operand);

            // Assert
            Assert.IsFalse(stack.IsEmpty);
            Assert.AreEqual(operand, stack.Peek());
        }

        [Test]
        public void Pop_ShouldRemoveAndReturnElementFromStack()
        {
            // Arrange
            var operand = new Operand(5);
            stack.Push(operand);

            // Act
            var result = stack.Pop();

            // Assert
            Assert.IsTrue(stack.IsEmpty);
            Assert.AreEqual(operand, result);
        }

        [Test]
        public void Peek_ShouldReturnTopElementWithoutRemovingIt()
        {
            // Arrange
            var operand = new Operand(5);
            stack.Push(operand);
            stack.Push(new Operand(10));

            // Act
            var result = stack.Peek() as Operand;

            // Assert
            Assert.IsFalse(stack.IsEmpty);
            Assert.AreEqual(10, result.Value);
        }

        [Test]
        public void Clear_ShouldRemoveAllElements()
        {
            // Arrange
            stack.Push(new Operand(5));
            stack.Push(new Operand(10));

            // Act
            stack.Clear();

            // Assert
            Assert.IsTrue(stack.IsEmpty);
        }

        [Test]
        public void IsEmpty_ShouldReturnTrueWhenStackIsEmpty()
        {
            // Assert
            Assert.IsTrue(stack.IsEmpty);
        }

        [Test]
        public void IsEmpty_ShouldReturnFalseWhenStackIsNotEmpty()
        {
            // Arrange
            stack.Push(new Operand(5));

            // Assert
            Assert.IsFalse(stack.IsEmpty);
        }

        [Test]
        public void GetEnumerator_ShouldAllowIterationOverStack()
        {
            // Arrange
            var expected = new List<IRpnElement>
        {
            new Operand(5),
            new Operand(10),
            new Operand(15)
        };

            foreach (var operand in expected)
            {
                stack.Push(operand);
            }

            var actual = new List<IRpnElement>();

            // Act
            foreach (var element in stack)
            {
                actual.Add(element);
            }

            // Assert
            expected.Reverse(); // L'itération doit être dans l'ordre LIFO (Last-In, First-Out), c'est une pile
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
