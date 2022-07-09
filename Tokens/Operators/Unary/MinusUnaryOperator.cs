namespace Interpreter.Tokens.Operators; 

public class MinusUnaryOperator : UnaryOperator {
	public MinusUnaryOperator() {
		Symbol = "-";
	}
	
	public override int Evaluate() {
		return -Child.Evaluate();
	}
}