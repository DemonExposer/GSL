using System.Text;
using Interpreter.Tokens;
using Interpreter.Types.Function;
using Object = Interpreter.Types.Object;

namespace Interpreter.BuiltinFunctions; 

public class Print : FunctionBody {
	public Print(Token[] expressions) : base(expressions) { }

	public override Object Execute(Object[] args, IDictionary<string, Object> vars) {
		StringBuilder sb = new StringBuilder();
		foreach (Object o in args)
			sb.Append(o.ToString()).Append(" ");
		
		Console.WriteLine(sb.ToString().Trim());
		return null!;
	}
}