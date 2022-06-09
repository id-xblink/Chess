using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChessServer
{
	class Program
	{
		static bool IsFirstPlayerAdmin = true;
		static bool IsFirstPlayerWhite = true;
		static TcpListener Listener = new TcpListener(IPAddress.Any, 5050);
		static List<ConnectedClient> Clients = new List<ConnectedClient>();
		static readonly string[] Colors = { "white", "black" };
		static bool IsReady = false;
		static int ReadyCount = 0;
		
		static void Main(string[] args)
		{
			// Настройка сервера
			Console.WriteLine("---Настройка сервера---");
			Console.WriteLine();
			bool Init = false;
			bool Readed = false;
			bool Found = false;
			string[] data = null;
			string liner;
			int Counter = 0;
			try
			{
				StreamReader file = new StreamReader("config.txt");
				while ((liner = file.ReadLine()) != null)
				{
					if (liner.Length > 1 && liner.Substring(0, 2) == "&:")
					{
						string[] parts = liner.Replace("&:", "").Split('|');
						if (parts.Length == 2)
						{
							Readed = true;
							data = parts;
						}
						Found = true;
						break;
					}
					Counter++;
				}
				file.Close();
			}
			catch (Exception) { }
			if (Counter > 0)
			{
				if (Found)
				{
					if (Readed)
					{
						//Настройка параметров сервера
						if (data[0] == "0" || data[0] == "1")
						{
							IsFirstPlayerAdmin = data[0] == "0" ? false : true;
							if (data[1] == "0" || data[1] == "1")
							{
								IsFirstPlayerWhite = data[1] == "0" ? false : true;
								Init = true;
							}
						}
					}
					else
						Console.WriteLine("Строка настройки сервера имеет неверный формат");
				}
				else
					Console.WriteLine("Не найдено строки настройки сервера");
			}
			else
			{
				// Восстановление файла
				string[] text =
				{
					"Настройка сервера",
					"",
					"Параметр №1: назначить первого игрока админом",
					"Значения: 0 / 1",
					"",
					"Параметр №2: назначить первому игроку белый цвет фигур",
					"Значения: 0 / 1",
					"",
					"Параметры разделяются знаком '|'",
					"",
					"Чтение параметров начинается с символов \"&:\"",
					"",
					"Строка параметров",
					"&:1|1"
				};
				File.WriteAllLines("config.txt", text, System.Text.Encoding.Default);
				Console.WriteLine("Файл пуст или не был найден и был восстановлен");
				Console.WriteLine("Инициализация сервера с параметрами по умолчанию");
				Console.WriteLine();
				Init = true;
			}
			if (!Init)
			{
				Console.WriteLine("Не удалось инициализировать сервер");
				return;
			}
			else
			{
				Console.WriteLine($"Первый игрок админ: {IsFirstPlayerAdmin}");
				Console.WriteLine($"Первый игрок играет белыми фигурами: {IsFirstPlayerWhite}");
				Console.WriteLine();
				Console.WriteLine("Ожидание подключений...");
			}
			// Работа сервера
			Listener.Start();
			new Thread(delegate()
			{
				Task.Factory.StartNew(() =>
				{
					while (true)
					{
						try
						{
							if (Clients.Count == 2 && !IsReady && ReadyCount == 2)
							{
								IsReady = true;
								SendCommand("_go");
							}
							if (IsReady)
							{
								for (int i = 0; i < Clients.Count; i++)
								{
									try
									{
										if (!Clients[i].Client.Connected || Clients.Count != 2)
										{
											DropServer();
										}
									}
									catch (Exception) { }
								}
							}
							Task.Delay(10).Wait();
						}
						catch (Exception ex)
						{
							Console.WriteLine($"Что-то пошло не так: {ex.Message}");
							break;
						}
					}
				});
			}).Start();
			while (true)
			{
				TcpClient client = Listener.AcceptTcpClient();
				Task.Factory.StartNew(() =>
				{
					StreamReader sr = new StreamReader(client.GetStream());
					while (client.Connected)
					{
						string line = sr.ReadLine();
						if (line.Contains("_login|") && !string.IsNullOrWhiteSpace(line.Replace("_login|", "")))
						{
							string nick = line.Replace("_login|", "");
							if (Clients.FirstOrDefault(s => s.Nick == nick) == null)
							{
								if (Clients.Count == 0)
									Clients.Add(new ConnectedClient(client, nick, IsFirstPlayerAdmin));
								else
									Clients.Add(new ConnectedClient(client, nick, !Clients[0].IsAdmin));
								
								Console.WriteLine($"Новый игрок: {nick}");
								break;
							}
							else
							{
								StreamWriter sw = new StreamWriter(client.GetStream())
								{
									AutoFlush = true
								};
								sw.WriteLine("Пользователь с таким идентификатором уже есть в игре, вас взломали?)");
								client.Client.Disconnect(false);
							}
						}
					}
					while (client.Connected)
					{
						try
						{
							sr = new StreamReader(client.GetStream());
							string line = sr.ReadLine();
							if (line.Contains("|"))
							{
								//Команда
								string[] parts = line.Split('|');
								if (parts[0] == "!ready")
								{
									if (Convert.ToBoolean(parts[1]))
									{
										ReadyCount++;
									}
									else
									{
										ClientDisconnected(client);
									}
								}
								if (parts[0] == "!leave")
								{
									// Завершение работы
									DropServer();
								}
							}
							else
								SendToAllClients(line);
							Console.WriteLine(line);
						}
						catch (Exception) { }
					}
				});
			}
		}

		static async void SendToAllClients(string message)
		{
			await Task.Factory.StartNew(() =>
			{
				foreach (ConnectedClient client in Clients)
				{
					try
					{
						if (client.Client.Connected)
						{
							StreamWriter sw = new StreamWriter(client.Client.GetStream())
							{
								AutoFlush = true
							};
							sw.WriteLine($"_turn|{message}");
						}
						else
						{
							Clients.Remove(client);
						}
					}
					catch (Exception) { }
				}
			});
		}

		static async void SendCommand(string message)
		{
			await Task.Factory.StartNew(() =>
			{
				for (int i = 0; i < Clients.Count; i++)
				{
					try
					{
						if (Clients[i].Client.Connected)
						{
							StreamWriter sw = new StreamWriter(Clients[i].Client.GetStream())
							{
								AutoFlush = true
							};
							sw.WriteLine($"{message}|{(IsFirstPlayerWhite ? Colors[i] : Colors[1 - i])}|{Clients[i].IsAdmin}|{Clients[1 - i].Nick}");
						}
						else
						{
							Clients.RemoveAt(i);
						}
					}
					catch (Exception) { }
				}
			});
		}

		static async void ClientDisconnected(TcpClient client)
		{
			await Task.Factory.StartNew(() =>
			{
				try
				{
					int id = Clients.FindIndex(c => c.Client == client);
					Clients.First(c => c.Client == client).Client.Close();
					Clients.RemoveAt(id);
					ReadyCount--;
				}
				catch (Exception) { }
			});
		}

		static async void DropServer()
		{
			await Task.Factory.StartNew(() =>
			{
				try
				{
					Environment.Exit(0);
				}
				catch (Exception) { }
			});
		}
	}
}