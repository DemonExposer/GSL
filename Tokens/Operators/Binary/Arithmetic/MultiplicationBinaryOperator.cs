using Interpreter.Types.Comparable;
using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class MultiplicationBinaryOperator : ArithmeticOperator {
	public MultiplicationBinaryOperator() {
		Symbol = "*";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Number(((Number) Left.Evaluate(vars)).Num * ((Number) Right.Evaluate(vars)).Num);
}