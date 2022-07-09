namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class MultiplicationBinaryOperator : ArithmeticOperator {
	public MultiplicationBinaryOperator() {
		Symbol = "*";
	}
	
	public override int Evaluate() {
		return Left.Evaluate() * Right.Evaluate();
	}
}