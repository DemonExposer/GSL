using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Comparable.Numbers;
using Interpreter.Types.Function;
using String = Interpreter.Types.Comparable.String;
using TrieDictionary;

namespace Interpreter.Types; 

public class LowObject {
	public Class ClassType = null!;
	public TrieDictionary<Object> Properties = new TrieDictionary<Object>();
	private static List<Type> parents = new List<Type>();

	public LowObject() {
		if (this is not Function.Function)
			Properties["toString"] = new Function.Function(new FunctionArgument[0], new ToStringFunction(this, null!));
	}

	public new virtual string ToString() => "Object";

	public new virtual string GetType() => "Object";

	private class ToStringFunction : FunctionBody {
		private LowObject context;

		public ToStringFunction(LowObject context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new String(context.ToString());
	}
}