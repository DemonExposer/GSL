namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class MinusBinaryOperator : ArithmeticOperator {
	public MinusBinaryOperator() {
		Symbol = "-";
	}
	
	public override int Evaluate() {
		return Left.Evaluate() - Right.Evaluate();
	}
}