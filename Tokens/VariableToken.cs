using System.Text;

namespace Interpreter.Tokens; 

public class VariableToken : Token {
	public string Name = null!;
	private IDictionary<string, object> vars;

	public VariableToken(IDictionary<string, object> vars) {
		this.vars = vars;
	}
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();

		sb.Append(indentStr).Append(Name).Append('\n');

		return sb.ToString();
	}

	public override int Evaluate() {
		object res;
		if (!vars.TryGetValue(Name, out res))
			throw new KeyNotFoundException("Line " + Line + ": Variable " + Name + " does not exist");

		return (int) res;
	}

	public override int Size() {
		return 1;
	}
}