using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using TrieDictionary;

namespace Interpreter.Types; 

public class String : Object {
	public string Str = null!;

	public String(string s) {
		Str = s;
		Properties["length"] = new Function.Function(new FunctionArgument[0], new LengthGetter(this, null!));
	}

	public override string ToString() => Str;

	public override string GetType() => "String";

	private class LengthGetter : FunctionBody {
		private String context;

		public LengthGetter(String context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new Integer(context.Str.Length);
	}
}