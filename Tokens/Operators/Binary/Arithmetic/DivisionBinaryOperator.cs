using Interpreter.Types.Comparable;
using Interpreter.Types.Comparable.Numbers;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class DivisionBinaryOperator : ArithmeticOperator {
	public DivisionBinaryOperator() {
		Symbol = "/";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Number.GetProperInstance(((Number) Left.Evaluate(vars)).Num / ((Number) Right.Evaluate(vars)).Num);
}