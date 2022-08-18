using Interpreter.Tokens.Statements.Binary;
using Interpreter.Tokens.Statements.Unary;
using Interpreter.Types.Util;
using TrieDictionary;
using Object = Interpreter.Types.Object;
using Dictionary = Interpreter.Types.Dictionary;

namespace Interpreter.Tokens.Operators.N_Ary; 

public class MultilineStatementOperator : NAryOperator {
	public bool IsDictionary = false;
	public bool IsPartOfFunction = false;
	
	public MultilineStatementOperator() {
		Symbol = "{}";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		if (IsDictionary) {
			Dictionary dictionary = new Dictionary();
			foreach (Token t in Children) {
				DictionaryEntry entry = (DictionaryEntry) t.Evaluate(vars);
				dictionary.Properties[entry.Key.Str] = entry.Value;
			}

			return dictionary;
		}

		foreach (Token t in Children) {
			if (t is ReturnStatement) {
				if (!IsPartOfFunction)
					throw new FormatException("Line " + t.Line + ": return statement not allowed outside of function");
				
				return t.Evaluate(vars);
			}

			if (t is BinaryStatement bs) {
				((MultilineStatementOperator) bs.Right).IsPartOfFunction = IsPartOfFunction;
				Object obj = t.Evaluate(vars);
				if (obj != null!)
					return obj;
			} else {
				t.Evaluate(vars);
			}
		}

		return null!;
	}
}