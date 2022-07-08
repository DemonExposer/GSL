using System.Data;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators;
using Interpreter.Tokens.Operators.Arithmetic;

namespace Interpreter;

using System.Text.RegularExpressions;

public class Program {
	private static IDictionary<string, Type> bindings = new Dictionary<string, Type>();
	private static IDictionary<string, object> vars = new Dictionary<string, object>();
	private static IDictionary<string, int> priorities = new Dictionary<string, int>();
	
	private static Token Parse(CheckedString[] line, int i, int depth) {
		if (depth > 10)
			throw new StackOverflowException();
		Console.WriteLine("i: " + i);
		foreach (CheckedString cs in line)
			Console.Write("{0}, ", cs.Str);
		Console.WriteLine();
		Console.WriteLine(line[i].Str);
		
		Token t;
		Type tokenType;
		// Check if string is a keyword/operator
		if (bindings.TryGetValue(line[i].Str, out tokenType)) {
			t = (Token) Activator.CreateInstance(tokenType); // Instantiate the corresponding class
			
			// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
			if (t is ArithmeticOperator) {
				line[i].IsDone = true;

				if (line[i - 1].Str == ")") {
					int numBrackets = 1;
					int j;
					for (j = i-2; numBrackets > 0; j--) {
						if (line[j].Str == "(")
							numBrackets--;
						else if (line[j].Str == ")")
							numBrackets++;
					}
					
					if (j <= 0 || line[j].IsDone)
						((ArithmeticOperator) t).Left = Parse(line, i - 1, depth + 1);
					else
						((ArithmeticOperator) t).Left = Parse(line, j, depth + 1);
				} else if (i - 2 < 0 || line[i-2].IsDone || line[i-1].Str == ")") {
					((ArithmeticOperator) t).Left = Parse(line, i - 1, depth + 1);
				} else {
					((ArithmeticOperator) t).Left = Parse(line, i - 2, depth + 1);
				}
				
				if (line[i + 1].Str == "(") {
					int numBrackets = 1;
					int j;
					for (j = i+2; numBrackets > 0; j++) {
						if (line[j].Str == "(")
							numBrackets++;
						else if (line[j].Str == ")")
							numBrackets--;
					}
					
					if (j >= line.Length || line[j].IsDone)
						((ArithmeticOperator) t).Right = Parse(line, i + 1, depth + 1);
					else
						((ArithmeticOperator) t).Right = Parse(line, j, depth + 1);
				} else if (i + 2 >= line.Length || line[i+2].IsDone || line[i+1].Str == "(") {
					((ArithmeticOperator) t).Right = Parse(line, i + 1, depth + 1);
				} else {
					((ArithmeticOperator) t).Right = Parse(line, i + 2, depth + 1);
				}
			} else if (t is DeclarationOperator) {
				((DeclarationOperator) t).SetVars(vars);
				((DeclarationOperator) t).Left = Parse(line, i + 1, depth+1);
				((DeclarationOperator) t).Right = Parse(line, i + 3, depth+1); // Skip =
			} else if (t is AssignmentOperator) {
				((AssignmentOperator) t).SetVars(vars);
				((AssignmentOperator) t).Left = Parse(line, i - 1, depth+1);
				((AssignmentOperator) t).Right = Parse(line, i + 1, depth+1);
			} else if (t is ParenthesesOperator) {
				int highestPriorityNum = -1;
				int index = -1;
				CheckedString[] subLine = {};
				if (line[i].Str == "(") {
					int numBrackets = 1;
					for (int j = i+1; numBrackets > 0; j++) {
						if (line[j].Str == ")")
							numBrackets--;
						else if (line[j].Str == "(")
							numBrackets++;

						if (numBrackets == 0)
							break;
						
						if (line[j].Str == "(") {
							while (numBrackets > 1) {
								subLine = subLine.Append(line[j]).ToArray();
								j++;
								if (line[j].Str == "(")
									numBrackets++;
								else if (line[j].Str == ")")
									numBrackets--;
							}
						}
						
						foreach (CheckedString cs in line)
							Console.Write("line: {0}, ", cs.Str);
						Console.WriteLine();
						subLine = subLine.Append(line[j]).ToArray();
						
						int priority;
						if (priorities.TryGetValue(line[j].Str, out priority)) {
							if (priority >= highestPriorityNum) {
								highestPriorityNum = priority;
								index = j;
							}
						}
					}
					Console.WriteLine("Final: " + line[index].Str);
				} else {
					int numBrackets = 1;
					for (int j = i-1; numBrackets > 0; j--) {
						if (line[j].Str == ")")
							numBrackets++;
						else if (line[j].Str == "(")
							numBrackets--;

						if (numBrackets == 0)
							break;
						
						if (line[j].Str == ")") {
							while (numBrackets > 1) {
								subLine = subLine.Prepend(line[j]).ToArray();
								j++;
								if (line[j].Str == "(")
									numBrackets--;
								else if (line[j].Str == ")")
									numBrackets++;
							}
						}
						
						subLine = subLine.Prepend(line[j]).ToArray();
						
						int priority;
						if (priorities.TryGetValue(line[j].Str, out priority)) {
							if (priority > highestPriorityNum) {
								highestPriorityNum = priority;
								index = 2*i - j; // Invert j with respect to i, so that later in the Parse call, it will result in the right number
							}
						}
					}
					Console.WriteLine("final: " + line[index].Str);
				}
				
				Console.WriteLine("Length: " + subLine.Length);
				foreach (CheckedString cs in subLine)
					Console.Write("sub: {0}, ", cs.Str);
				Console.WriteLine();
				Console.WriteLine("{0} {1} {2}", index, i, index - i);
				// Parse the data in the brackets, where index is the index of the head of the tree in line, so index-i
				// (i being the starting point of subLine in line) will be the index in subLine plus one
				((ParenthesesOperator) t).Child = Parse(subLine, index-i-1, depth + 1);
			}
		} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
			VariableToken vt = new VariableToken(vars);
			vt.Name = line[i].Str;
			t = vt;

			if (i < line.Length-1 && line[i + 1].Str == "(") {
				line[i + 1].IsDone = true;
				// TODO: parse everything in the function call, and mark the second bracket as done
			}
		} else if (Regex.Matches(line[i].Str, "(\\s|^)-?\\d+(\\s|$)").Count == 1) {
			NumberToken nt = new NumberToken();
			nt.Num = Int32.Parse(line[i].Str);
			t = nt;
		} else {
			throw new InvalidExpressionException("Line " + line[i].Line + ": " + line[i].Str + " is not a valid expression");
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;

		// TODO: reduce complexity from n^2 to n, by using priorities
	/*	if (depth == 0) {
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
*/
		return t;
	}

	private static CheckedString[] CheckComment(CheckedString[] line) {
		for (int i = 0; i < line.Length; i++)
			if (line[i].Str == "#")
				return line.Take(i).ToArray();

		return line;
	}

	public static void Main(string[] args) {
		bindings.Add("+", typeof(PlusBinaryOperator));
		bindings.Add("-", typeof(MinusBinaryOperator));
		bindings.Add("*", typeof(MultiplicationBinaryOperator));
		bindings.Add("/", typeof(DivisionBinaryOperator));
		bindings.Add("^", typeof(PowerBinaryOperator));
		bindings.Add("decl", typeof(DeclarationOperator));
		bindings.Add("=", typeof(AssignmentOperator));
		bindings.Add("(", typeof(ParenthesesOperator));
		bindings.Add(")", typeof(ParenthesesOperator));

		// Low number for priority means a higher priority
		priorities.Add("(", 0);
		priorities.Add("^", 1);
		priorities.Add("*", 2);
		priorities.Add("/", 2);
		priorities.Add("+", 3);
		priorities.Add("-", 3);
		priorities.Add("=", 4);
		priorities.Add("decl", 5);

		string[] lines = File.ReadAllLines(args[0]);
		for (int i = 0; i < lines.Length; i++) {
			CheckedString[] lexedLine = Regex.Matches(lines[i], "([a-zA-Z1-9]+|-?\\d+|[\\^*/+-=()#])").ToList().Select(match => new CheckedString {Str = match.Value.Trim(), Line = i+1}).ToArray();
			foreach (CheckedString cs in lexedLine)
				Console.Write("{0}, ", cs.Str);
			Console.WriteLine();
			lexedLine = CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;

			int highestPriorityNum = -1;
			int index = -1;
			for (int j = 0; j < lexedLine.Length; j++) {
				int priority;
				if (lexedLine[j].Str == "(") {
					int numBrackets = 1;
					while (numBrackets > 0) {
						j++;
						if (lexedLine[j].Str == "(")
							numBrackets++;
						else if (lexedLine[j].Str == ")")
							numBrackets--;
					}
				}
				if (priorities.TryGetValue(lexedLine[j].Str, out priority)) {
					if (priority >= highestPriorityNum) {
						highestPriorityNum = priority;
						index = j;
					}
				}
			}

			if (index == -1)
				throw new FormatException("Line " + (i + 1) + " contains no expression");
			
			Token tree = Parse(lexedLine, index, 0);
		//	Console.WriteLine(tree.ToString(0));
			Console.WriteLine(tree.Evaluate());
		}
	}
}
