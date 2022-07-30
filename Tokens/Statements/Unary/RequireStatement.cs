using Interpreter.util;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class RequireStatement : UnaryStatement {
	public RequireStatement() {
		Symbol = "require";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => null!;

	// This leaves the require statement in the parse tree, but that is not a problem, as its evaluation does not affect the program's state
	public void Insert(DoublyLinkedList<string> list) {
		DoublyLinkedList<string> save = list.Right;
		DoublyLinkedList<string> importedList = DoublyLinkedList<string>.FromArray(File.ReadAllLines(Child.Str + ".gsl"));
		list.Right = importedList;
		importedList.Left = list;

		while (importedList.Right != null!)
			importedList = importedList.Right;

		importedList.Right = save;
		save.Left = importedList;
	}
}