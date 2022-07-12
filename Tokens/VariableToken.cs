using System.Text;
using Interpreter.Types;
using Interpreter.Types.Function;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens; 

public class VariableToken : Token {
	public string Name = null!;
	public Token Args = null!;
	private IDictionary<string, Object> vars;

	public VariableToken(IDictionary<string, Object> vars) {
		this.vars = vars;
	}
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();

		sb.Append(indentStr).Append(Name).Append('\n');

		if (Args != null)
			sb.Append(Args.ToString(indent + 1));

		return sb.ToString();
	}

	public override Object Evaluate() {
		Object res;
		if (!vars.TryGetValue(Name, out res))
			throw new KeyNotFoundException("Line " + Line + ": Variable " + Name + " does not exist");
		
		if (res is Function f)
			return f.Execute(new [] {Args.Evaluate()});
		
		return res;
	}

	public override int Size() {
		return 1;
	}
}