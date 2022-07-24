using System.Text;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using TrieDictionary;

namespace Interpreter.Types; 

public class Array : Object {
	public List<Object> Arr;

	public Array(IEnumerable<Object> arr) {
		Arr = arr.ToList();
		Properties["length"] = new Function.Function(new FunctionArgument[0], new LengthGetter(this, null!));
	}
	
	public override string ToString() {
		if (Arr.Count == 0)
			return "[]";
		
		StringBuilder sb = new StringBuilder("[");
		Arr.ForEach(o => sb.Append(o.ToString()).Append(", "));
		sb.Append("\u0008\u0008]");

		return sb.ToString();
	}

	public override string GetType() => "Array";
	
	private class LengthGetter : FunctionBody {
		private Array context;

		public LengthGetter(Array context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new Integer(context.Arr.Count);
	}
}