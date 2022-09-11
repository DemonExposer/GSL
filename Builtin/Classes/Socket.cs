using System.Net;
using System.Net.Sockets;
using Interpreter.Tokens.Operators.N_Ary;
using static System.Text.Encoding;
using Interpreter.Types;
using Interpreter.Types.Comparable.Numbers;
using Interpreter.Types.Function;
using TrieDictionary;
using Object = Interpreter.Types.Object;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Builtin.Classes; 

public class Socket : Class {
	private SocketBody send, receive, close;
	private delegate Object SocketAction(System.Net.Sockets.Socket socket, Object[] args);
	
	public Socket() {
		Name = "Socket";
		
		send = new SocketBody(null!);
		send.Command = (socket, args) => {
			socket.Send(Default.GetBytes(((String) args[0]).Str));
			return null!;
		};
		ClassProperties["send"] = new Function(new[] {new FunctionArgument {ArgType = typeof(String), Name = "data"}}, send);

		close = new SocketBody(null!);
		close.Command = (socket, _) => {
			socket.Close();
			return null!;
		};
		ClassProperties["close"] = new Function(new FunctionArgument[0], close);
	}

	public override Object Instantiate(params Object[] args) {
		if (args.Length != 2)
			throw new ArgumentException("File constructor takes 2 arguments, " + args.Length + " were given");
		
		IPAddress ipAddress = IPAddress.Parse(((String) args[0]).Str);
		int port = ((Integer) args[1]).Int;
		
		IPEndPoint endpoint = new IPEndPoint(ipAddress, port);
		System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		socket.Connect(endpoint);

		send.Socket = receive.Socket = close.Socket = socket;
		
		return new Instance {ClassType = this, Properties = ClassProperties};
	}
	
	private class SocketBody : FunctionBody {
		public System.Net.Sockets.Socket Socket = null!;
		public SocketAction Command = null!;
		
		public SocketBody(MultilineStatementOperator expressions) : base(expressions) { }

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => Command(Socket, args);
	}
}