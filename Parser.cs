using System.Data;
using System.Text.RegularExpressions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators;
using Interpreter.Tokens.Operators.Arithmetic;

namespace Interpreter; 

public class Parser {
	private static Token ArithmeticParse(CheckedString[] line, int i, int depth, bool isRightBound) {
		if (isRightBound ? line[i + 1].Str == "(" : line[i - 1].Str == ")") {
			// j will be the index of the operator on the other side of the brackets
			int numBrackets = 1;
			int j;
			for (j = isRightBound ? i+2 : i-2; numBrackets > 0; j += isRightBound ? 1 : -1) {
				if (line[j].Str == "(")
					numBrackets += isRightBound ? 1 : -1;
				else if (line[j].Str == ")")
					numBrackets += isRightBound ? -1 : 1;
			}
					
			/*
			 * Of course check for bounds and check whether this is the time when the brackets should be parsed
			 * If there is an operator on the other side of the brackets,
			 * they should only be parsed when that operator is above the current operator in the parse tree.
			 * Or in other words, the other operator should be done
			 */
			if ((isRightBound ? j >= line.Length : j <= 0) || line[j].IsDone)
				return Parse(line, i + (isRightBound ? 1 : -1), depth + 1);
			
			return Parse(line, j, depth + 1);
		}
		
		// If there is no operator on the other side, parse and
		// if the other operator is above the current operator in the parse tree, parse too
		if (isRightBound ? i + 2 >= line.Length || line[i+2].IsDone : i - 2 < 0 || line[i-2].IsDone)
			return Parse(line, i + (isRightBound ? 1 : -1), depth + 1);
		
		// Else, parse operator on the other side
		return Parse(line, i + (isRightBound ? 2 : -2), depth + 1);
	}

	private static Token ParenthesesParse(CheckedString[] line, int i, int depth, bool isRightBound) {
		int startIndex = -1;
		int highestPriorityNum = -1;
		int index = -1;
		CheckedString[] subLine = {};
		int numBrackets = 1;
		for (int j = i + (isRightBound ? 1 : -1); numBrackets > 0; j += isRightBound ? 1 : -1) {
			if (line[j].Str == ")")
				numBrackets += isRightBound ? -1 : 1;
			else if (line[j].Str == "(")
				numBrackets += isRightBound ? 1 : -1;

			if (numBrackets == 0)
				break;

			if (isRightBound ? line[j].Str == "(" : line[j].Str == ")") {
				while (numBrackets > 1) {
					if (isRightBound && subLine.Length == 0)
						startIndex = j;
					subLine = isRightBound ? subLine.Append(line[j]).ToArray() : subLine.Prepend(line[j]).ToArray();
					j += isRightBound ? 1 : -1;
					if (line[j].Str == "(")
						numBrackets += isRightBound ? 1 : -1;
					else if (line[j].Str == ")")
						numBrackets += isRightBound ? -1 : 1;
				}
			}

			if (isRightBound) {
				if (subLine.Length == 0)
					startIndex = j;
				subLine = subLine.Append(line[j]).ToArray();
			} else {
				startIndex = j;
				subLine = subLine.Prepend(line[j]).ToArray();
			}

			int priority;
			if (Program.priorities.TryGetValue(line[j].Str, out priority)) {
				if (isRightBound ? priority >= highestPriorityNum : priority > highestPriorityNum) {
					highestPriorityNum = priority;
					index = j;
				}
			}
		}
		
		if (index == -1) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Warning: Double brackets on line " + line[i].Line);
			Console.ResetColor();
			index = startIndex; // Make sure index-startIndex is 0
		}

		if (startIndex == -1) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error in bracket parsing");
			Console.ResetColor();
		}
		
		// Parse the data in the brackets, where index is the index of the head of the tree in line, so index-i
		// (i being the starting point of subLine in line) will be the index in subLine plus one
		return Parse(subLine, index - startIndex, depth + 1);
	}
	
	public static Token Parse(CheckedString[] line, int i, int depth) {
		Token t;
		Type tokenType;
		// Check if string is a keyword/operator
		if (Program.bindings.TryGetValue(line[i].Str, out tokenType)) {
			t = (Token) Activator.CreateInstance(tokenType); // Instantiate the corresponding class
			
			// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
			if (t is ArithmeticOperator) {
				line[i].IsDone = true;

				((ArithmeticOperator) t).Left = ArithmeticParse(line, i, depth, false);
				((ArithmeticOperator) t).Right = ArithmeticParse(line, i, depth, true);
			} else if (t is DeclarationOperator) {
				((DeclarationOperator) t).SetVars(Program.vars);
				((DeclarationOperator) t).Left = Parse(line, i + 1, depth+1);
				((DeclarationOperator) t).Right = Parse(line, i + 3, depth+1); // Skip =
			} else if (t is AssignmentOperator) {
				((AssignmentOperator) t).SetVars(Program.vars);
				((AssignmentOperator) t).Left = Parse(line, i - 1, depth+1);
				((AssignmentOperator) t).Right = Parse(line, i + 1, depth+1);
			} else if (t is ParenthesesOperator) {
				((ParenthesesOperator) t).Child = ParenthesesParse(line, i, depth + 1, line[i].Str == "(");
			}
		} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
			VariableToken vt = new VariableToken(Program.vars);
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
		
		return t;
	}
}