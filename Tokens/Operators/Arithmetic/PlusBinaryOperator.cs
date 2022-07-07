namespace Interpreter.Tokens.Operators.Arithmetic; 

public class PlusBinaryOperator : ArithmeticOperator {
	public PlusBinaryOperator() {
		Symbol = "+";
	}
	
	public override int Evaluate() {
		return Left.Evaluate() + Right.Evaluate();
	}
}