using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Function;
using TrieDictionary;

namespace Interpreter.Types.Comparable; 

public class String : Comparable {
	public string Str = null!;

	public String(string s) {
		Str = s;
		Properties["length"] = new Function.Function(new FunctionArgument[0], new LengthGetter(this, null!));
		Properties["toInteger"] = new Function.Function(new FunctionArgument[0], new ToInteger(this, null!));
	}

	public override string ToString() => Str;

	public override string GetType() => "String";
	
	public override Boolean Equals(Comparable c) {
		if (c is not String s)
			throw new IncomparableException("trying to compare String with non-String");

		return new Boolean(Str == s.Str);
	}

	private class LengthGetter : FunctionBody {
		private String context;

		public LengthGetter(String context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new Integer(context.Str.Length);
	}
	
	private class ToInteger : FunctionBody {
		private String context;
		
		public ToInteger(String context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new Integer(Int32.Parse(context.Str));
	}
}