using System.Text;
using Interpreter.Tokens;
using Interpreter.Types.Function;

namespace Interpreter.Types; 

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