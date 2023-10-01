
using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

abstract class Worker
{
	protected string Name;
	protected string Position;
	protected int WorkDay;
	public Worker(string name) 
	{
		Name = name;
	}
	public void Call() 
	{
		Console.WriteLine($"{Name}: Call()");
	}
	public void WriteCode() 
	{
		Console.WriteLine($"{Name}: WriteCode()");
	}
	public void Relax() 
	{
		Console.WriteLine($"{Name}: Relax()");
	}
	public abstract void FillWorkDay();

	public string GetName() 
	{
		return Name;
	}

	public string GetPosition() 
	{ 
		return Position;
	}

	public int GetWorkDay() 
	{ 
		return WorkDay;
	}
}

class Developer : Worker 
{
	public Developer(string name) : base(name) 
	{
		Position = "Розробник";
	}
	public override void FillWorkDay() 
	{
		WriteCode();
		Call();
		Relax();
		WriteCode();
	}
}

class Manager : Worker
{
	private Random random = new Random();
	public Manager(string name) : base(name)
	{
		Position = "Менеджер";
	}
	public override void FillWorkDay()
	{
		for (int i = 0; i < random.Next(1, 11); ++i) {
			Call();
		}
		Relax();
		for (int i = 0; i < random.Next(1, 6); ++i)
		{
			Call();
		}
	}
}

class Team
{
	private string TeamName;
	private List<Worker> Workers = new List<Worker>(); 
	public Team(string name) 
	{
		TeamName = name;
	}
	public void AddWorker(Worker worker) 
	{
		Workers.Add(worker);
	}
	public void PrintInfo() 
	{
		Console.WriteLine($"Назва команди: {TeamName}");
		foreach (var worker in Workers) {
			Console.WriteLine(worker.GetName());
		}
	}
	public void PrintDetailedInfo() 
	{
		Console.WriteLine($"Назва команди: {TeamName}");
		foreach (var worker in Workers)
		{
			Console.WriteLine($"<{worker.GetName()}> - <{worker.GetPosition()}> - <{worker.GetWorkDay()}>");
		}
	}
	public string GetTeamName() 
	{
		return TeamName;
	}
}

class Program
{
	static void Main(string[] args)
	{
		Console.OutputEncoding = Encoding.UTF8;
		
		List<Team> teams = new List<Team>();

		while (true)
		{
			Console.WriteLine("------------------------------------------");
			Console.WriteLine("Оберіть опцію:");
			Console.WriteLine("1. Створити нову команду");
			Console.WriteLine("2. Додати співробітника до команди");
			Console.WriteLine("3. Вивести інформацію про команду");
			Console.WriteLine("4. Вивести детальну інформація про команду");
			Console.WriteLine("------------------------------------------");

			string option = Console.ReadLine();

			switch (option)
			{
				case "1":
					Console.WriteLine("Введіть назву команди:");
					string teamName = Console.ReadLine();
					Team team = new Team(teamName);
					teams.Add(team);
					Console.WriteLine($"Команда '{teamName}' створена.");
					break;

				case "2":
					if (teams.Count > 0)
					{
						Console.WriteLine("Оберіть команду для додавання співробітника:");
						for (int i = 0; i < teams.Count; i++)
						{
							Console.WriteLine($"{i + 1}. {teams[i].GetTeamName()}");
						}
						int teamChoice = int.Parse(Console.ReadLine()) - 1;

						Console.WriteLine("Введіть ім'я співробітника:");
						string workerName = Console.ReadLine();

						Console.WriteLine("Виберіть посаду співробітника (1 - Розробник, 2 - Менеджер):");
						int positionChoice = int.Parse(Console.ReadLine());
					
						Worker newWorker;

						if (positionChoice == 1)
						{
							newWorker = new Developer(workerName);
						}
						else if (positionChoice == 2)
						{
							newWorker = new Manager(workerName);
						}
						else
						{
							Console.WriteLine("Помилка: Некоректний вибір посади.");
							continue;
						}

						teams[teamChoice].AddWorker(newWorker);
						Console.WriteLine($"{workerName} доданий до команди '{teams[teamChoice].GetTeamName()}'.");

					}
					else 
					{
						Console.WriteLine("Помилка: Ви не створили жодної команди.");
					}
					break;

				case "3":
					if (teams.Count > 0)
					{
						Console.WriteLine("Виберіть команду для виведення інформації:");
						for (int i = 0; i < teams.Count; i++)
						{
							Console.WriteLine($"{i + 1}. {teams[i].GetTeamName()}");
						}
						int teamIndex = int.Parse(Console.ReadLine()) - 1;
						teams[teamIndex].PrintInfo();
					}
					else 
					{
						Console.WriteLine("Помилка: Ви не створили жодної команди.");
					}
					
					break;

				case "4":
					if (teams.Count > 0)
					{
						Console.WriteLine("Виберіть команду для виведення детальної інформації:");
						for (int i = 0; i < teams.Count; i++) 
						{
							Console.WriteLine($"{i + 1}. {teams[i].GetTeamName()}");
						}
						int teamIdx = int.Parse(Console.ReadLine()) - 1;

						teams[teamIdx].PrintDetailedInfo();
					}
					else
					{
						Console.WriteLine("Помилка: Ви не створили жодної команди.");
					}

					break;

				default:
					Console.WriteLine("Помилка: Некоректний вибір опції.");
					break;
			}
		}
	}
}
