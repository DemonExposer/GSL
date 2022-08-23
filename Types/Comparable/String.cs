using System.Globalization;
using System.Text;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Comparable.Numbers;
using Interpreter.Types.Function;
using TrieDictionary;

namespace Interpreter.Types.Comparable; 

public class String : Comparable {
	private string str = null!;
	public string Str { 
		get => str;
		set {
			IsEscaped = false;
			str = value;
		}
	}
	public bool IsEscaped = false;

	public String(string s) {
		Str = s;
		Properties["length"] = new Function.Function(new FunctionArgument[0], new LengthGetter(this, null!));
		Properties["toInteger"] = new Function.Function(new FunctionArgument[0], new ToInteger(this, null!));
		Properties["split"] = new Function.Function(new [] {new FunctionArgument {ArgType = typeof(String), Name = "separator"}}, new Split(this, null!));
		Properties["charAt"] = new Function.Function(new [] {new FunctionArgument {ArgType = typeof(Integer), Name = "index"}}, new CharAt(this, null!));
	}

	public override string ToString() => Str;

	public override string GetType() => "String";
	
	public override Boolean Equals(Comparable c) {
		if (c is not String s)
			throw new IncomparableException("trying to compare String with non-String");

		return new Boolean(Str == s.Str);
	}

	public void Escape() {
		if (IsEscaped)
			return;
		
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < str.Length; i++) {
			if (str[i] == '\\') { // Replace escapes
				switch (str[i+1]) {
					case 'n':
						sb.Append('\n');
						i++;
						break;
					case 'r':
						sb.Append('\r');
						i++;
						break;
					case 't':
						sb.Append('\t');
						i++;
						break;
					case '\\':
						sb.Append('\\');
						i++;
						break;
					case 'u':
						sb.Append((char) Int32.Parse(str.Substring(i + 2, 4), NumberStyles.HexNumber));
						i += 5;
						break;
				}
			} else {
				sb.Append(str[i]);
			}
		}

		str = sb.ToString();
		IsEscaped = true;
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

	private class Split : FunctionBody {
		private String context;
		
		public Split(String context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new Array(context.Str.Split(((String) args[0]).Str).ToList().Select(str => new String(str)));
	}

	private class CharAt : FunctionBody {
		private String context;
		
		public CharAt(String context, MultilineStatementOperator expressions) : base(expressions) {
			this.context = context;
		}

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new String(context.Str[((Integer) args[0]).Int].ToString());
	}
}