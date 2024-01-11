using Rpnw.Domain.Impl.Rpn.Calculator;
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
    public class CalculatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateStack_ShouldReturnUniqueStackId()
        {

            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);

            // Act
            var stackId1 = calculator.CreateStack();
            var stackId2 = calculator.CreateStack();

            // Assert
            Assert.AreNotEqual(stackId1, stackId2);
        }

        [Test]
        public void GetAllElements_ShouldReturnAllStackElements()
        {


            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);

            var stackId = calculator.CreateStack();
            calculator.PushOperand(stackId, 1);
            calculator.PushOperand(stackId, 2);
            calculator.PushOperand(stackId, 3);

            // Act
            var elements = calculator.GetAllElements(stackId).ToList();

            // Assert
            Assert.AreEqual(3, elements.Count);
            Assert.AreEqual(3, ((Operand)elements[0]).Value);
            Assert.AreEqual(2, ((Operand)elements[1]).Value);
            Assert.AreEqual(1, ((Operand)elements[2]).Value);
        }

        [Test]
        public void Peek_ShouldReturnTopElementWithoutRemovingIt()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);

            // var calculator = new RpnCalculator(true);
            var stackId = calculator.CreateStack();
            calculator.PushOperand(stackId, 1);
            calculator.PushOperand(stackId, 2);

            // Act
            var topElement = calculator.Peek(stackId) as Operand;

            // Assert
            Assert.AreEqual(2, topElement.Value);
            Assert.AreEqual(2, ((Operand)calculator.Peek(stackId)).Value);

        }
        [Test]
        public void Undo_ShouldRevertLastOperation()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);

            var stackId = calculator.CreateStack();
            calculator.PushOperand(stackId, 10);
            calculator.PushOperand(stackId, 5);

            calculator.PushOperator(stackId, RpnOperatorEnum.Add); 


            // Act
            calculator.Undo(stackId);

            // Assert
            Assert.AreEqual(5, ((Operand)calculator.Peek(stackId)).Value);
            Assert.AreEqual(2, calculator.GetAllElements(stackId).Count());
        }


        [Test]
        public void GetAllStackIds_ShouldReturnAllCreatedStackIds()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory,false);

            var createdStackIds = new List<Guid>
            {
                calculator.CreateStack(),
                calculator.CreateStack(),
                calculator.CreateStack()
            };

            // Act
            var allStackIds = calculator.GetAllStackIds().ToList();

            // Assert
            CollectionAssert.AreEquivalent(createdStackIds, allStackIds);
        }

        [Test]
        public void RemoveStack_ShouldSuccessfullyRemoveStackById()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory,false);

            var stackId = calculator.CreateStack();

            // Act
            calculator.RemoveStack(stackId);

            // Assert
            var allStackIds = calculator.GetAllStackIds();
            Assert.IsFalse(allStackIds.Contains(stackId));
        }

        [Test]
        public void RemoveStack_ShouldThrowIfStackIdNotFound()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory,false);
            var nonExistentStackId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<StackNotFoundException>(() => calculator.RemoveStack(nonExistentStackId));
        }

        [Test]
        public void ImmediateEvaluation_ShouldCalculateCorrectResult()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);//  immédiate

            var stackId = calculator.CreateStack();

            // Act
            calculator.PushOperand(stackId, 2);
            calculator.PushOperand(stackId, 10);
            calculator.PushOperand(stackId, 5);
          
            calculator.PushOperator(stackId, RpnOperatorEnum.Add);
            calculator.PushOperator(stackId, RpnOperatorEnum.Mult); 

            // Assert
            var result = calculator.Peek(stackId) as IRpnOperand;
            Assert.IsNotNull(result);
            Assert.AreEqual(30, result.Value);
        }

        [Test]
        public void UndoOperationAfterImmediateEvaluation_ShouldRevertLastOperation()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);// Mode immédiate

            var stackId = calculator.CreateStack();

            calculator.PushOperand(stackId, 10);
            calculator.PushOperand(stackId, 5);
            calculator.PushOperator(stackId, RpnOperatorEnum.Add); 

            // Act
            calculator.Undo(stackId);

            // Assert
            var elements = calculator.GetAllElements(stackId);


            var result1 = elements.ElementAt(0) as IRpnOperand;
            var result2 = elements.ElementAt(1) as IRpnOperand;

            Assert.IsNotNull(result1);
            Assert.IsNotNull(result2);
            Assert.AreEqual(5, result1.Value, "Le premier opérande (5) doit être sur le dessus de la pile après Undo.");
            Assert.AreEqual(10, result2.Value, "Le second opérande (10) doit être sur la pile après Undo.");
        }

        [Test]
        public void ComplexOperationsWithUndo_ShouldHandleMultipleOperationsCorrectly()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory, true);// Mode immédiate

            var stackId = calculator.CreateStack();

            // Act & Assert
            calculator.PushOperand(stackId, 3);
            calculator.PushOperand(stackId, 4);
            calculator.PushOperator(stackId, RpnOperatorEnum.Add);

            Assert.AreEqual(7, (calculator.Peek(stackId) as IRpnOperand).Value, "Le résultat de 3 + 4 doit être 7.");

            calculator.PushOperand(stackId, 2);
            calculator.PushOperator(stackId, RpnOperatorEnum.Mult);
            Assert.AreEqual(14, (calculator.Peek(stackId) as IRpnOperand).Value, "Le résultat de 7 * 2 doit être 14.");

            // Annuler la multiplication
            calculator.Undo(stackId);
            var intermediateResult = calculator.GetAllElements(stackId).Select(e => (e as IRpnOperand).Value).ToArray();
            Assert.AreEqual(new double[] { 2, 7 }, intermediateResult, "Après Undo, la pile doit contenir 2 et 7.");


            calculator.PushOperand(stackId, 5);
            calculator.PushOperator(stackId, RpnOperatorEnum.Add);
            Assert.AreEqual(7, (calculator.Peek(stackId) as IRpnOperand).Value, "Le résultat de 2 + 5 doit être 7.");

            var finalResult = calculator.GetAllElements(stackId).Select(e => (e as IRpnOperand).Value).ToArray();
            Assert.AreEqual(new double[] { 7, 7 }, finalResult, "La pile finale doit contenir 7 et 7.");
        }

        [Test]
        public void DeferredEvaluationWithAddAndMul_ShouldCalculateCorrectly()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory,false); // Mode différée

            var stackId = calculator.CreateStack();

            // Act
            calculator.PushOperand(stackId, 3); 
            calculator.PushOperand(stackId, 4); 
            calculator.PushOperator(stackId, RpnOperatorEnum.Add); 
            calculator.PushOperand(stackId, 2); 
            calculator.PushOperator(stackId, RpnOperatorEnum.Mult); 

            // Évaluez la pile entière
            calculator.Evaluate(stackId);

            // Assert
            var result = calculator.Peek(stackId) as IRpnOperand;
            Assert.IsNotNull(result);
            Assert.AreEqual(14, result.Value, "Le résultat de (3 + 4) * 2 doit être 14.");
        }

        [Test]
        public void RpnCalculator_ShouldEvaluateComplexExpressionCorrectly()
        {
            // Arrange
            var stackFactory = new RpnStackFactory();
            var calculator = new RpnCalculator(stackFactory,false);// Mode évaluation immédiate

            //var calculator = new RpnCalculator(false); // Mode d'évaluation immédiate
            var stackId = calculator.CreateStack();

            // Act
            calculator.PushOperand(stackId, 10); // 10
            calculator.PushOperand(stackId, 6);  // 10 6
            calculator.PushOperand(stackId, 9);  // 10 6 9
            calculator.PushOperand(stackId, 3);  // 10 6 9 3
            //calculator.PushOperator(stackId, new AddOperator()); // 10 6 (9+3) = 10 6 (12)
            calculator.PushOperator(stackId, RpnOperatorEnum.Add); // 10 6 (9+3) = 10 6 (12)
            calculator.PushOperand(stackId, -11);  // 10 6 (9+3) -11 = 10 6 (12) -11
            //calculator.PushOperator(stackId, new MulOperator()); // 10 6  ((9+3) * -11) = 10 6 (-132)
            calculator.PushOperator(stackId, RpnOperatorEnum.Mult);
            //calculator.PushOperator(stackId, new DivOperator()); // 10 (6 / ((9+3) * -11)) = 10 0.04545
            calculator.PushOperator(stackId, RpnOperatorEnum.Div); // 10 (6 / ((9+3) * -11)) = 10 0.04545
            //calculator.PushOperator(stackId, new MulOperator()); // 10 * (6 / ((9+3) * -11)) = 0.4545
            calculator.PushOperator(stackId, RpnOperatorEnum.Mult); // 10 * (6 / ((9+3) * -11)) = 0.4545
            calculator.PushOperand(stackId, 17); // 10 * (6 / ((9+3) * -11)) 17 = 0.4545 17
            //calculator.PushOperator(stackId, new AddOperator()); // (10 * (6 / ((9 + 3) * -11)) + 17) = 0.4545 + 17 =17.4545
            calculator.PushOperator(stackId, RpnOperatorEnum.Add); // (10 * (6 / ((9 + 3) * -11)) + 17) = 0.4545 + 17 =17.4545
            calculator.PushOperand(stackId, 5); //(10 * (6 / ((9 + 3) * -11)) + 17) 5 = 17.4545 5
            //calculator.PushOperator(stackId, new AddOperator()); // ((10 * (6 / ((9 + 3) * -11)) + 17) +5) = 22.4545
            calculator.PushOperator(stackId, RpnOperatorEnum.Add); // ((10 * (6 / ((9 + 3) * -11)) + 17) +5) = 22.4545

            calculator.Evaluate(stackId);

            // Assert
            var result = calculator.Peek(stackId) as IRpnOperand;
            double expectedResult = (10.0 * (6.0 / ((9.0 + 3.0) * -11.0)) + 17.0) + 5.0; // attention au double
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult, result.Value, "The expression should be evaluated to the correct result.");
        }
    }
}
