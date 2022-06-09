using System.Net.Sockets;

namespace ChessServer
{
	class ConnectedClient
	{
		public TcpClient Client { get; set; }
		public string Nick { get; set; }
		public bool IsAdmin { get; set; }

		public ConnectedClient(TcpClient client, string nick, bool isAdmin)
		{
			Client = client;
			Nick = nick;
			IsAdmin = isAdmin;
		}
	}
}