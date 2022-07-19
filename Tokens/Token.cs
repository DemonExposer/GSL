using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens;

public abstract class Token {
	public int Line;
	public bool IsDone = false;
	public string Str = null!;

	public abstract string ToString(int indent);
	public abstract Object Evaluate(List<TrieDictionary<Object>> vars);
	public abstract int Size();
}