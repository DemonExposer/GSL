namespace Interpreter.Tokens.Operators.Arithmetic; 

public class MinusBinaryOperator : ArithmeticOperator {
	public MinusBinaryOperator() {
		Symbol = "-";
	}
	
	public override int Evaluate() {
		return Left.Evaluate() - Right.Evaluate();
	}
}