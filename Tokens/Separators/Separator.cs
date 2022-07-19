using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Separators; 

public abstract class Separator : Token {
	public override string ToString(int indent) => null!;
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => null!;

	public override int Size() => 1;
}