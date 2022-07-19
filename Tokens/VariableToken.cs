using System.Text;
using Interpreter.Types.Function;
using Interpreter.Types.Util;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens;

public class VariableToken : Token {
	public string Name = null!;
	public Token Args = null!;
	private List<TrieDictionary<Object>> vars;

	public VariableToken(List<TrieDictionary<Object>> vars) {
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
		Object res = null!;

		for (int i = vars.Count - 1; i >= 0; i--) try {
			res = vars[i][Name];
			break;
		} catch (KeyNotFoundException) { }
		
		if (vars.Count == 2)
			Console.WriteLine(vars[0].GetKeySet().Length);
		if (res == null!)
			throw new KeyNotFoundException("Line " + Line + ": Variable " + Name + " does not exist");

		if (res is Function f) {
			Object o = Args.Evaluate();
			return f.Execute(o is ArgumentArray aa ? aa.Arr : new [] {o});
		}

		return res;
	}

	public override int Size() => 1;
}