using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types;
using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using TrieDictionary;
using static System.Text.Encoding;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Builtin.Classes; 

public class File : Class {
	private FileBody read, write, append, exists;
	private delegate Object FileAction(string fileName, Object[] args);
	
	public File() {
		Name = "File";
		
		read = new FileBody(null!);
		read.Command = (fileName, _) => {
			FileStream fs = System.IO.File.Open(fileName, FileMode.Open);
			byte[] buffer = new byte[fs.Length];
			fs.Read(buffer, 0, (int) fs.Length);
			fs.Close();
			return new String(Default.GetString(buffer));
		};
		ClassProperties["readAll"] = new Function(new FunctionArgument[0], read);
		
		write = new FileBody(null!);
		write.Command = (fileName, args) => {
			FileStream fs = System.IO.File.Open(fileName, FileMode.OpenOrCreate);
			fs.Write(Default.GetBytes(((String) args[0]).Str));
			fs.Close();
			return null!;
		};
		ClassProperties["write"] = new Function(new [] {new FunctionArgument {ArgType = typeof(String), Name = "data"}}, write);
		
		append = new FileBody(null!);
		append.Command = (fileName, args) => {
			FileStream fs = System.IO.File.Open(fileName, FileMode.Append);
			fs.Write(Default.GetBytes(((String) args[0]).Str));
			fs.Close();
			return null!;
		};
		ClassProperties["append"] = new Function(new [] {new FunctionArgument {ArgType = typeof(String), Name = "data"}}, append);

		exists = new FileBody(null!);
		exists.Command = (fileName, _) => new Boolean(System.IO.File.Exists(fileName));
		ClassProperties["exists"] = new Function(new FunctionArgument[0], exists);
	}

	public override Object Instantiate(params Object[] args) {
		if (args.Length != 1)
			throw new ArgumentException("File constructor takes 1 argument, " + args.Length + " were given");

		string fileName = ((String) args[0]).Str;
		write.FileName = read.FileName = append.FileName = exists.FileName = fileName;
		return new Instance {ClassType = this, Properties = ClassProperties};
	}

	private class FileBody : FunctionBody {
		public string FileName = null!;
		public FileAction Command = null!;
		
		public FileBody(MultilineStatementOperator expressions) : base(expressions) { }

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => Command(FileName, args);
	}
}