using TrieDictionary;
using Double = Interpreter.Types.Comparable.Numbers.Double;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Numbers; 

public class DoubleToken : NumberToken {
	public Double D { get => (Double) Num; set => Num = value; }

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => D;
}