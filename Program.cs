using System.Data;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators;
using Interpreter.Tokens.Operators.Arithmetic;

namespace Interpreter;

using System.Text.RegularExpressions;

public class Program {
	private static IDictionary<string, Type> bindings = new Dictionary<string, Type>();
	private static IDictionary<string, object> vars = new Dictionary<string, object>();

	private static Token Parse(CheckedString[] line, int i, int depth) {
		Token t;
		Type tokenType;
		if (bindings.TryGetValue(line[i].Str, out tokenType)) {
			t = (Token) Activator.CreateInstance(tokenType);
			
			if (t is ArithmeticOperator) {
				((ArithmeticOperator) t).Left = Parse(line, i + 1, depth+1);
				for (int j = i + 1; j < line.Length; j++) {
					if (!line[j].IsDone) {
						((ArithmeticOperator) t).Right = Parse(line, j, depth+1);
						break;
					}
				}
			} else if (t is DeclarationOperator) {
				((DeclarationOperator) t).SetVars(vars);
				((DeclarationOperator) t).Left = Parse(line, i + 1, depth+1);
				((DeclarationOperator) t).Right = Parse(line, i + 3, depth+1); // Skip =
			} else if (t is AssignmentOperator) {
				((AssignmentOperator) t).SetVars(vars);
				((AssignmentOperator) t).Left = Parse(line, i - 1, depth+1);
				((AssignmentOperator) t).Right = Parse(line, i + 1, depth+1);
			}
		} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
			VariableToken vt = new VariableToken(vars);
			vt.Name = line[i].Str;
			t = vt;
		} else if (Regex.Matches(line[i].Str, "(\\s|^)-?\\d+(\\s|$)").Count == 1) {
			NumberToken nt = new NumberToken();
			nt.Num = Int32.Parse(line[i].Str);
			t = nt;
		} else {
			throw new InvalidExpressionException("Line " + line[i].Line + ": " + line[i].Str + " is not a valid expression");
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;

		if (depth == 0) {
			for (int j = 0; j < line.Length; j++) {
				if (!line[j].IsDone) {
					int tSize = t.Size();
					Token otherToken = Parse(line, j, depth + 1);
					if (Math.Max(tSize, otherToken.Size()) != tSize) {
						t = otherToken;
					}
				}
			}
		}

		return t;
	}

	public static void Main(string[] args) {
		bindings.Add("+", typeof(PlusBinaryOperator));
		bindings.Add("-", typeof(MinusBinaryOperator));
		bindings.Add("*", typeof(MultiplicationBinaryOperator));
		bindings.Add("/", typeof(DivisionBinaryOperator));
		bindings.Add("^", typeof(PowerBinaryOperator));
		bindings.Add("decl", typeof(DeclarationOperator));
		bindings.Add("=", typeof(AssignmentOperator));

		string[] lines = File.ReadAllLines(args[0]);
		for (int i = 0; i < lines.Length; i++) {
			CheckedString[] lexedLine = Regex.Matches(lines[i], "([a-zA-Z1-9]+|-?\\d+|[\\^*/+-=])").ToList().Select(match => new CheckedString {Str = match.Value.Trim(), Line = i+1}).ToArray();
			Token tree = Parse(lexedLine, 0, 0);
		//	Console.WriteLine(tree.ToString(0));
			Console.WriteLine(tree.Evaluate());
		}
	}
}
