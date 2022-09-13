using System.Net;
using System.Net.Sockets;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types;
using Interpreter.Types.Comparable.Numbers;
using Interpreter.Types.Function;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Builtin.Classes.Networking; 

public class ServerSocket : Class {
	private ServerSocketBody accept, close;
	private delegate Object ServerSocketAction(System.Net.Sockets.Socket socket, Object[] args);
	
	public ServerSocket() {
		Name = "ServerSocket";

		accept = new ServerSocketBody(null!);
		accept.Command = (socket, _) => ((Socket) Program.Vars["Socket"]).InstantiateWithExistingSocket(socket.Accept());
		ClassProperties["accept"] = new Function(new FunctionArgument[0], accept);

		close = new ServerSocketBody(null!);
		close.Command = (socket, _) => {
			socket.Close();
			return null!;
		};
		ClassProperties["close"] = new Function(new FunctionArgument[0], close);
	}

	public override Object Instantiate(params Object[] args) {
		if (args.Length != 1)
			throw new ArgumentException("File constructor takes 1 argument, " + args.Length + " were given");
		
		IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
		int port = ((Integer) args[0]).Int;
		
		IPEndPoint endpoint = new IPEndPoint(ipAddress, port);
		System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		socket.Bind(endpoint);
		socket.Listen();
		
		accept.Socket = close.Socket = socket;
		
		return new Instance {ClassType = this, Properties = ClassProperties};
	}
	
	private class ServerSocketBody : FunctionBody {
		public System.Net.Sockets.Socket Socket = null!;
		public ServerSocketAction Command = null!;
		
		public ServerSocketBody(MultilineStatementOperator expressions) : base(expressions) { }

		public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => Command(Socket, args);
	}
}