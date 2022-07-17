using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Separators; 

public abstract class Separator : Token {
	public override string ToString(int indent) => null!;
	
	public override Object Evaluate() => null!;

	public override int Size() => 1;
}