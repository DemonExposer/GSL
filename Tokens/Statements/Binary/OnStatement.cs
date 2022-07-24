using System.Text;
using Interpreter.Tokens.Statements.Unary;
using TrieDictionary;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Binary; 

public class OnStatement : BinaryStatement {
	public ElseStatement ElseChild = null!;
	
	public OnStatement() {
		Symbol = "on";
	}

	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append('\n');
		if (Left != null!)
			sb.Append(Left.ToString(indent+1));
		if (Right != null!)
			sb.Append(Right.ToString(indent+1));
		if (ElseChild != null!)
			sb.Append(ElseChild.ToString(indent + 1));

		return sb.ToString();
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		List<TrieDictionary<Object>> properVars = new List<TrieDictionary<Object>>(vars);
		properVars.Add(new TrieDictionary<Object>());
		
		return ((Boolean) Left.Evaluate(properVars)).Bool ? Right.Evaluate(properVars) : ElseChild != null! ? ElseChild.Evaluate(vars) : null!;
	}
}