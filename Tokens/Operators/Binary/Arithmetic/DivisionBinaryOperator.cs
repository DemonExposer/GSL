using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class DivisionBinaryOperator : ArithmeticOperator {
	public DivisionBinaryOperator() {
		Symbol = "/";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Integer(((Integer) Left.Evaluate(vars)).Int / ((Integer) Right.Evaluate(vars)).Int);
}