namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class DivisionBinaryOperator : ArithmeticOperator {
	public DivisionBinaryOperator() {
		Symbol = "/";
	}
	
	public override int Evaluate() {
		return Left.Evaluate() / Right.Evaluate();
	}
}