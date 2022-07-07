namespace Interpreter.Tokens.Operators.Arithmetic; 

public class PowerBinaryOperator : ArithmeticOperator {
	public PowerBinaryOperator() {
		Symbol = "^";
	}
	
	public override int Evaluate() {
		return (int) Math.Pow(Left.Evaluate(), Right.Evaluate());
	}
}