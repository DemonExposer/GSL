using Interpreter.Types.Comparable;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class PowerBinaryOperator : ArithmeticOperator {
	public PowerBinaryOperator() {
		Symbol = "^";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Integer((int) Math.Pow(((Integer) Left.Evaluate(vars)).Int, ((Integer) Right.Evaluate(vars)).Int));
}