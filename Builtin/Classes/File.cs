using System.Text;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types;
using Interpreter.Types.Function;
using TrieDictionary;
using static System.Text.Encoding;
using Object = Interpreter.Types.Object;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Builtin.Classes; 

public class File : Class {
	private ReadBody rb;
	private WriteBody wb;
	
	public File() {
		Name = "File";
		rb = new ReadBody(null!);
		ClassProperties["readAll"] = new Function(new FunctionArgument[0], rb);
		wb = new WriteBody(null!);
		ClassProperties["write"] = new Function(new [] {new FunctionArgument {ArgType = typeof(String), Name = "data"}}, wb);
	}

	public override Object Instantiate(params Object[] args) {
		if (args.Length != 1)
			throw new ArgumentException("File constructor takes 1 argument, " + args.Length + " were given");

		string fileName = ((String) args[0]).Str;
		wb.FileName = rb.FileName = fileName;
		return new Instance {ClassType = this, Properties = ClassProperties};
	}

	private class ReadBody : FunctionBody {
		public string FileName = null!;
		
		public ReadBody(MultilineStatementOperator expressions) : base(expressions) { }

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) {
			FileStream fs = System.IO.File.Open(FileName, FileMode.OpenOrCreate);
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, (int) fs.Length);
			fs.Close();
			return new String(Default.GetString(buffer));
		}
	}

	private class WriteBody : FunctionBody {
		public string FileName = null!;
		
		public WriteBody(MultilineStatementOperator expressions) : base(expressions) { }

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) {
			FileStream fs = System.IO.File.Open(FileName, FileMode.OpenOrCreate);
			fs.Write(Default.GetBytes(((String) args[0]).Str));
			fs.Close();
			return null!;
		}
	}
}