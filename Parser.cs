using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Binary.Boolean;
using Interpreter.Tokens.Operators.Unary;

namespace Interpreter; 

public class Parser {
	public static int GetTopElementIndex(Token[] line, int startIndex, bool isRightBound) {
		int highestPriorityNum = -1;
		int index = -1;
		for (int i = startIndex; i < line.Length && i >= 0; i += isRightBound ? 1 : -1) {
			int priority = -1;
			int numBrackets = 0;
			if (line[i].Str == "(")
				numBrackets++;
			else if (line[i].Str == ")")
				numBrackets--;
			
			while (numBrackets != 0) {
				i += isRightBound ? 1 : -1;
				if (line[i].Str == "(")
					numBrackets++;
				else if (line[i].Str == ")")
					numBrackets--;
			}
			
			try {
				priority = Program.priorities[line[i].Str];
			} catch (KeyNotFoundException) { }
			
			if (!line[i].IsDone && line[i] is BinaryOperator && priority != -1) {
				if (priority >= highestPriorityNum) {
					highestPriorityNum = priority;
					index = i;
				}
			}
		}

		if (index == -1)
			return startIndex;

		return index;
	}
	
	/**
	 * This now practically just parses everything, so maybe some refactoring is needed
	 */
	private static Token SymmetricBinaryOperatorParse(Token[] line, int i, int depth, bool isRightBound) {
		int j = GetTopElementIndex(line, i + (isRightBound ? 1 : -1), isRightBound);
		if (j == i && !isRightBound) {
			j = i - 1;
			int numBrackets = 0;
			while (numBrackets != 0 || j >= 0 && line[j] is not BinaryOperator) {
				if (line[j].Str == "(")
					numBrackets++;
				else if (line[j].Str == ")")
					numBrackets--;

				j--;
			}
		}

		// BinaryOperator does not exist or is already done
		if (j < 0 || line[j].IsDone)
			if (!isRightBound)
				return Parse(line, j + 1, depth + 1);
			else
				return Parse(line, i + 1, depth + 1);

		// BinaryOperator does exist and is not done, parse it
		return Parse(line, j, depth + 1);
	}

	/**
	 * Removes parentheses and parses inside expression by identifying the top operator and calling Parse
	 */
	private static Token ParenthesesParse(Token[] line, int i, int depth, bool isRightBound) {
		int startIndex = -1;
		int highestPriorityNum = -1;
		int index = -1;
		Token[] subLine = {};
		int numBrackets = 1;
		for (int j = i + (isRightBound ? 1 : -1); numBrackets > 0; j += isRightBound ? 1 : -1) {
			// Go until paired bracket is found
			if (line[j].Str == ")")
				numBrackets += isRightBound ? -1 : 1;
			else if (line[j].Str == "(")
				numBrackets += isRightBound ? 1 : -1;

			if (numBrackets == 0)
				break;

			// If there is a nested set of brackets, add that entire set immediately, because otherwise something goes wrong
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

			// Get the index of the operator with the lowest priority (highest number) to make sure that gets parsed first
			// This should really go through GetTopElementIndex, but that would need an adaptation for leftbound cases
			int priority = -1;
			try {
				priority = Program.priorities[line[j].Str];
			} catch (KeyNotFoundException) { }
			
			if (line[j] is BinaryOperator && priority != -1) {
				if (isRightBound ? priority >= highestPriorityNum : priority > highestPriorityNum) {
					highestPriorityNum = priority;
					index = j;
				}
			}
		}
		
		if (index == -1)
			index = startIndex; // This makes sure index-startIndex is 0, because the first and only element should be parsed

		if (startIndex == -1) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Error in bracket parsing");
			Console.ResetColor();
		}
		
		// Parse the data in the brackets, where index is the index of the head of the tree in line, so index-i
		// (i being the starting point of subLine in line) will be the index in subLine plus one
		return Parse(subLine, index - startIndex, depth + 1);
	}
	
	public static Token Parse(Token[] line, int i, int depth) {
		Token t = line[i];

		// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
		if (t is ArithmeticOperator or BooleanOperator) {
			line[i].IsDone = true;

			// Parse only the appropriate section (i.e. Left should only parse to the left and Right only to the right, that's what the array slicing does)
			((BinaryOperator) t).Left = SymmetricBinaryOperatorParse(line.Take(i+1).ToArray(), i, depth + 1, false);
			((BinaryOperator) t).Right = SymmetricBinaryOperatorParse(new ArraySegment<Token>(line, i, line.Length - i).ToArray(), 0, depth + 1, true);
		} else if (t is DeclarationOperator decOp) {
			decOp.SetVars(Program.vars);
			decOp.Left = Parse(line, i + 1, depth+1);
			
			if (i + 2 < line.Length) // Only Parse right hand side if it exists
				decOp.Right = Parse(line, i + 2, depth + 1);
		} else if (t is AssignmentOperator assOp) {
			assOp.IsDone = true;
			
			assOp.SetVars(Program.vars);
			assOp.Left = Parse(line, i - 1, depth+1);
			assOp.Right = Parse(line, GetTopElementIndex(line, i+1, true), depth+1);
		} else if (t is ParenthesesOperator parOp) {
			parOp.Child = ParenthesesParse(line, i, depth + 1, line[i].Str == "(");
		} else if (t is MinusUnaryOperator minUnOp) {
			minUnOp.Child = Parse(line, i + 1, depth + 1);
		} else if (t is NotUnaryOperator notUnOp) {
			notUnOp.Child = Parse(line, i + 1, depth + 1);
		} else if (t is VariableToken vt) { // TODO: make sure multiple arguments get parsed properly
			if (i + 1 < line.Length && line[i+1] is ParenthesesOperator)
				vt.Args = Parse(line, i + 1, depth + 1);
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;
		
		return t;
	}
}