using TrieDictionary;
using Double = Interpreter.Types.Comparable.Numbers.Double;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Numbers; 

public class DoubleToken : Token {
	public Double Num = null!;
	
	public override string ToString(int indent) => throw new NotImplementedException();

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => throw new NotImplementedException();

	public override int Size() => throw new NotImplementedException();
}