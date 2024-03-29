using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class PlusBinaryOperator : ArithmeticOperator {
	public PlusBinaryOperator() {
		Symbol = "+";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Number.GetProperInstance(((Number) Left.Evaluate(vars)).Num + ((Number) Right.Evaluate(vars)).Num);
}