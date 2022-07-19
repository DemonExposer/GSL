using Interpreter.Types.Comparable;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class PlusBinaryOperator : ArithmeticOperator {
	public PlusBinaryOperator() {
		Symbol = "+";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Integer(((Integer) Left.Evaluate(vars)).Int + ((Integer) Right.Evaluate(vars)).Int);
}