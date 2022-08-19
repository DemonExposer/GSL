using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Numbers; 

public class IntegerToken : NumberToken {
	public Integer Int { get => (Integer) Num; set => Num = value; }

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Int;
}