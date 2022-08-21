using System.Text;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types;
using Interpreter.Types.Util;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class ClassStatement : UnaryStatement {
	public string Name = null!;
	public InheritsStatement Parents = null!;

	public ClassStatement() {
		Symbol = "class";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		List<TrieDictionary<Object>> classProperties = new List<TrieDictionary<Object>>(new [] {new TrieDictionary<Object>()});
		((MultilineStatementOperator) Child).Evaluate(classProperties);

		Class res = new Class {Name = Name, ClassProperties = classProperties[0]};

		if (Parents != null) {
			Object o = Parents.Evaluate(vars);
			if (o is ArgumentArray aa)
				aa.Arr.Cast<Class>().ToList().ForEach(res.AddParent);
			else
				res.AddParent((Class) o);
		}

		return vars[^1][Name] = res;
	}

	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append(' ').Append(Name).Append('\n');
		if (Parents != null!)
			sb.Append(Parents.ToString(indent+1));
		if (Child != null!)
			sb.Append(Child.ToString(indent+1));

		return sb.ToString();
	}
}