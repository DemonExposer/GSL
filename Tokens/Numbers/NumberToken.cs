using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Numbers; 

public class NumberToken : Token {

	public override string ToString(int indent) => throw new NotImplementedException();

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => throw new NotImplementedException();

	public override int Size() => throw new NotImplementedException();
}