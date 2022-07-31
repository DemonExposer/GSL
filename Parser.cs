using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Binary.Boolean;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Tokens.Separators;
using Interpreter.Tokens.Statements.Binary;
using Interpreter.Tokens.Statements.Unary;
using Interpreter.util;

namespace Interpreter; 

public class Parser {
	public static int GetTopElementIndex(Token[] line, int startIndex, bool isRightBound) {
		if (line[startIndex] is BinaryStatement)
			return startIndex;
	
		int highestPriorityNum = -1;
		int index = -1;
		for (int i = startIndex; i < line.Length && i >= 0; i += isRightBound ? 1 : -1) {
			int numBrackets = 0;
			if (Program.OpeningBrackets.Contains(line[i].Str))
				numBrackets++;
			else if (Program.ClosingBrackets.Contains(line[i].Str))
				numBrackets--;
			
			while (numBrackets != 0) {
				i += isRightBound ? 1 : -1;
				if (Program.OpeningBrackets.Contains(line[i].Str))
					numBrackets++;
				else if (Program.ClosingBrackets.Contains(line[i].Str))
					numBrackets--;
			}

			if (!line[i].IsDone && line[i] is BinaryOperator && Program.Priorities.Contains(line[i].Str)) {
				int priority = Program.Priorities[line[i].Str];
				if (isRightBound ? priority >= highestPriorityNum : priority > highestPriorityNum) {
					highestPriorityNum = priority;
					index = i;
				}
			}
		}

		if (index == -1)
			return startIndex;

		return index;
	}
	
	public static CheckedString[] CheckComment(CheckedString[] line) {
		for (int i = 0; i < line.Length; i++)
			if (line[i].Str == "#")
				return line.Take(i).ToArray();

		return line;
	}
	
	/**
	 * This now practically just parses everything, so maybe some refactoring is needed
	 */
	private static Token SymmetricBinaryOperatorParse(Token[] line, int i, DoublyLinkedList<string> list, ref int lineNo, int depth, bool isRightBound) {
		int startIndex = i + (isRightBound ? 1 : -1);
		int j = GetTopElementIndex(line, startIndex, isRightBound);
		if (j == startIndex && !isRightBound) {
			int numBrackets = 0;
			while (numBrackets != 0 || j >= 0 && line[j] is not BinaryOperator) {
				if (Program.OpeningBrackets.Contains(line[j].Str))
					numBrackets++;
				else if (Program.ClosingBrackets.Contains(line[j].Str))
					numBrackets--;

				j--;
			}
		}

		// BinaryOperator does not exist or is already done
		if (j < 0 || line[j].IsDone)
			if (!isRightBound)
				return Parse(line, j + 1, list, ref lineNo, depth + 1);
			else
				return Parse(line, i + 1, list, ref lineNo, depth + 1);

		// BinaryOperator does exist and is not done, parse it
		return Parse(line, j, list, ref lineNo, depth + 1);
	}

	/**
	 * Removes brackets and parses inside expression(s) by identifying the top operator and calling Parse
	 */
	private static Token[] BracketsParse(Token[] line, int i, DoublyLinkedList<string> list, ref int lineNo, int depth, bool isRightBound) {
		List<Token[]> arguments = new List<Token[]>();
		List<Token> subLine = new List<Token>();
		int numBrackets = 0;
		for (int j = i + (isRightBound ? 1 : -1); isRightBound ? !Program.ClosingBrackets.Contains(line[j].Str) : !Program.OpeningBrackets.Contains(line[j].Str); j += isRightBound ? 1 : -1) {
			if (Program.OpeningBrackets.Contains(line[j].Str))
				numBrackets++;
			else if (Program.ClosingBrackets.Contains(line[j].Str))
				numBrackets--;
			
			while (numBrackets != 0) {
				if (isRightBound)
					subLine.Add(line[j]);
				else
					subLine = subLine.Prepend(line[j]).ToList();
				
				j += isRightBound ? 1 : -1;
				
				if (Program.OpeningBrackets.Contains(line[j].Str))
					numBrackets++;
				else if (Program.ClosingBrackets.Contains(line[j].Str))
					numBrackets--;
			}

			// When a comma is found, add the current buffer as an argument and start on a new argument
			if (line[j] is CommaSeparator) {
				if (isRightBound)
					arguments.Add(subLine.ToArray());
				else
					arguments = arguments.Prepend(subLine.ToArray()).ToList();
				subLine = new List<Token>();
				continue;
			}

			if (isRightBound)
				subLine.Add(line[j]);
			else
				subLine = subLine.Prepend(line[j]).ToList();
		}

		if (subLine.Count > 0)
			if (isRightBound)
				arguments.Add(subLine.ToArray());
			else
				arguments = arguments.Prepend(subLine.ToArray()).ToList();
		
		// Parse each element in the brackets and put it in a new list
		List<Token> properArguments = new List<Token>();
		for (int j = 0; j < arguments.Count; j++)
			properArguments.Add(Parse(arguments[j], GetTopElementIndex(arguments[j].ToArray(), 0, isRightBound), list, ref lineNo, depth + 1));
		
		return properArguments.ToArray();
	}

	/**
	 * Note that this is a bad implementation with too strict constraints, but for now, only functionality is important
	 */
	private static MultilineStatementOperator CurlyBracketsParse(Token[] line, DoublyLinkedList<string> list, ref int i, Token parent, int depth) {
		// Get copy of vars so that it doesn't get affected by method calls lower in the recursion tree
		List<Token> tokens = new List<Token>();
		int initialIndex = ++i; // immediately increment i to fix the error messages (i.e. indicating the right line)
		int numBrackets = 1;
		list = list.Right; // go to next line so that this function doesn't try to parse itself
		for (; list != null!; i++, list = list.Right) {
			CheckedString[] lexedLine = Lexer.Lex(list.Value!, i + 1);

			lexedLine = CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;

			Token[] tokenizedLine = Tokenizer.Tokenize(lexedLine);

			int before = i;
			tokens.Add(Parse(tokenizedLine, GetTopElementIndex(tokenizedLine, 0, true), list, ref i, depth + 1));

			if (i != before) {
				for (; before < i; before++)
					list = list.Right;
				continue;
			}

			foreach (CheckedString cs in lexedLine) {
				if (cs.Str == "}")
					numBrackets--;
				else if (cs.Str == "{")
					numBrackets++;

				if (numBrackets == 0) {
					if (parent is OnStatement onStat && tokenizedLine.Length > 1 && tokenizedLine[1] is ElseStatement)
						onStat.ElseChild = (ElseStatement) Parse(tokenizedLine, 1, list, ref i, depth + 1);
					
					goto FullBreak;
				}
			}
		}
		FullBreak:
		
		if (list == null)
			throw new FormatException("no matched bracket for bracket on line " + initialIndex);

		((MultilineStatementOperator) line[^1]).Children = tokens.ToArray();

		return (MultilineStatementOperator) line[^1];
	}
	
	public static Token Parse(Token[] line, int i, DoublyLinkedList<string> list, ref int lineNo, int depth) {
		Token t = line[i];

		switch (t) {
			// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
			case ArithmeticOperator or BooleanOperator or ConcatenationOperator or DotOperator: // BUG: DotOperator parsing incorrect: !file.exists() gets parsed as (!file).exists() instead of !(file.exists())
				line[i].IsDone = true;

				// Parse only the appropriate section (i.e. Left should only parse to the left and Right only to the right, that's what the array slicing does)
				((BinaryOperator) t).Left = SymmetricBinaryOperatorParse(line.Take(i+1).ToArray(), i, list, ref lineNo, depth + 1, false);
				((BinaryOperator) t).Right = SymmetricBinaryOperatorParse(new ArraySegment<Token>(line, i, line.Length - i).ToArray(), 0, list, ref lineNo, depth + 1, true);
				break;
			case DeclarationOperator decOp: {
				decOp.Left = Parse(line, i + 1, list, ref lineNo, depth+1);
			
				if (i + 2 < line.Length) // Only Parse right hand side if it exists
					decOp.Right = Parse(line, i + 2, list, ref lineNo, depth + 1);
				break;
			}
			case AssignmentOperator assOp: {
				assOp.IsDone = true;

				int numBrackets = 0;
				int j;
				// Gets the first variable token to the right of the nearest operator
				for (j = i; j >= 0 && line[j + 1] is not VariableToken || j == i; j--) {
					if (Program.OpeningBrackets.Contains(line[j].Str))
						numBrackets++;
					else if (Program.ClosingBrackets.Contains(line[j].Str))
						numBrackets--;
			
					while (numBrackets != 0) {
						j--;
				
						if (Program.OpeningBrackets.Contains(line[j].Str))
							numBrackets++;
						else if (Program.ClosingBrackets.Contains(line[j].Str))
							numBrackets--;
					}
				}

				if (j >= 0 && line[j] is DotOperator)
					j--;
				
				assOp.Left = Parse(line, j + 1, list, ref lineNo, depth+1);
				Token[] subLine = new ArraySegment<Token>(line, i, line.Length - i).ToArray();
				assOp.Right = Parse(subLine, GetTopElementIndex(subLine, 1, true), list, ref lineNo, depth+1);
				break;
			}
			case ParenthesesOperator parOp:
				parOp.Children = BracketsParse(line, i, list, ref lineNo, depth + 1, Program.OpeningBrackets.Contains(line[i].Str));
				break;
			case SquareBracketOperator sqOp:
				sqOp.Children = BracketsParse(line, i, list, ref lineNo, depth + 1, Program.OpeningBrackets.Contains(line[i].Str));
				break;
			case UnaryOperator unOp:
				unOp.Child = Parse(line, i + 1, list, ref lineNo, depth + 1);
				break;
			case VariableToken vt: {
				if (i + 1 < line.Length)
					switch (line[i+1]) {
						case ParenthesesOperator:
							vt.Args = Parse(line, i + 1, list, ref lineNo, depth + 1);
							break;
						case SquareBracketOperator:
							vt.Index = Parse(line, i + 1, list, ref lineNo, depth + 1);
							break;
						case UnlimitedArgumentOperator:
							vt.IsUnlimited = true;
							break;
					}
				break;
			}
			case BinaryStatement statement: {
				int addition = 1;
				if (t is FunctionStatement fs) {
					addition = 2;
					fs.Name = line[i + 1].Str;
				}
				
				Token left = Parse(line, i + addition, list, ref lineNo, depth + 1);
				if (left is not ParenthesesOperator po)
					throw new FormatException("statement condition/parameter declaration on line " + left.Line + " is missing parentheses");
			
				statement.Left = po;
			
				int numBrackets = 0;
				int j = i+1;
				do {
					if (Program.ClosingBrackets.Contains(line[j].Str))
						numBrackets--;
					else if (Program.OpeningBrackets.Contains(line[j].Str))
						numBrackets++;
				
					j++;
				} while (numBrackets != 0);
			
				statement.Right = CurlyBracketsParse(line, list, ref lineNo, statement, depth + 1);
				break;
			}
			case RequireStatement reqStat: {
				reqStat.Child = Parse(line, i + 1, list, ref lineNo, depth + 1);
				reqStat.Insert(list);
				lineNo--;
				break;
			}
			case ElseStatement or ClassStatement: {
				if (t is ClassStatement classStat)
					classStat.Name = line[i + 1].Str;
				
				Token child = CurlyBracketsParse(line, list, ref lineNo, t, depth + 1);
				if (child is not MultilineStatementOperator mso)
					throw new FormatException("statement argument on line " + child.Line + " needs curly brackets");

				((UnaryStatement) t).Child = mso;
				break;
			}
			case UnaryStatement unStat: {
				Token child = Parse(line, i + 1, list, ref lineNo, depth + 1);
				if (child is not ParenthesesOperator po)
					throw new FormatException("statement argument on line " + child.Line + " is missing parentheses");

				unStat.Child = po;
				break;
			}
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;
		
		return t;
	}
}