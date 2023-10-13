
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Converter 
{
	private Dictionary<string, decimal> _rates = new Dictionary<string, decimal>();

	public Converter()
	{
		_rates["UAH"] = 1;
	}
	public bool Check(string rate)
	{
		return _rates.ContainsKey(rate);
	}

	public bool CheckEmpty()
	{ 
		return _rates.Count == 0;
	}
	public int GetLen()
	{
		return _rates.Count;
	}

	public void AddRate(string rateName, decimal rate)
	{
		_rates[rateName] = rate;
	}
	public string GetRatesInfo()
	{
		if (CheckEmpty())
		{
			return "Error: No rates added yet";
		}
		else
		{
			StringBuilder output = new StringBuilder();
			foreach (var rate in _rates)
			{
				output.AppendLine($"{rate.Key} : {rate.Value} UAH");
			}

			if (output.Length > 0)
			{
				output.Length -= Environment.NewLine.Length;
			}

			return output.ToString();
		}
	}

	public List<string> GetRates(string exclude = null) 
	{
		List<string> output = new List<string>();
		foreach (var rate in _rates) 
		{
			if (exclude != rate.Key) 
			{
				output.Add(rate.Key);
			}
		}
		return output;
	}

	public decimal Translate(string inputRate, string outputRate, decimal value) 
	{
		return value * _rates[inputRate] / _rates[outputRate];
	}

	public void Delete(string rate)
	{
		_rates.Remove(rate);
	}
}

class Executor 
{
	Converter converter = new Converter();
	string option;


	public void Execute() 
	{
		Console.WriteLine("------------------------------------------");
		Console.WriteLine("Choose option:");
		Console.WriteLine("1. Create new rate");
		Console.WriteLine("2. Translate money");
		Console.WriteLine("3. View rates");
		Console.WriteLine("4. Update rate");
		Console.WriteLine("5. Delete rate");
		Console.WriteLine("6. Exit");
		Console.WriteLine("------------------------------------------");

		option = Console.ReadLine();

		switch (option)
		{
			case "1":
				Console.WriteLine("Enter a name of the rate (eg. USD):");
				string rateName = Console.ReadLine();
				if (!converter.Check(rateName))
				{
					Console.WriteLine("Enter rate value in hryvnia:");
					string rateValue = Console.ReadLine();
					if (decimal.TryParse(rateValue, out decimal value))
					{
						if (value > 0)
						{
							converter.AddRate(rateName, value);
							Console.WriteLine("Rate added successfuly!");
						}
						else
						{
							Console.WriteLine("Error: Non-positive value");
						}
					}
					else
					{
						Console.WriteLine("Error: Invalid format");
					}
				}
				else
				{
					Console.WriteLine("Error: Rate already exists");
				}
				break;

			case "2":
				if (converter.GetLen() > 1)
				{
					Console.WriteLine("Translate from:");
					var rates = converter.GetRates();
					for (int i = 0; i < rates.Count; i++)
					{
						Console.WriteLine($"{i + 1}. {rates[i]}");
					}
					option = Console.ReadLine();
					if (int.TryParse(option, out int optionInputInt))
					{
						if (optionInputInt <= rates.Count && optionInputInt > 0)
						{
							Console.WriteLine("Enter the amount:");
							string inputAmount = Console.ReadLine();
							if (decimal.TryParse(inputAmount, out decimal inputAmountDecimal))
							{
								if (inputAmountDecimal > 0)
								{

									Console.WriteLine("Enter the output rate:");
									var outputRates = converter.GetRates(rates[optionInputInt - 1]);
									for (int i = 0; i < outputRates.Count; i++)
									{
										Console.WriteLine($"{i + 1}. {outputRates[i]}");
									}
									option = Console.ReadLine();
									if (int.TryParse(option, out int optionOutputInt))
									{
										if (optionInputInt <= rates.Count && optionInputInt > 0)
										{
											Console.WriteLine($"{inputAmountDecimal} {rates[optionInputInt - 1]} = " +
												$"{converter.Translate(rates[optionInputInt - 1], outputRates[optionOutputInt - 1], inputAmountDecimal)} {outputRates[optionOutputInt - 1]}");
										}
										else
										{
											Console.WriteLine("Error: Invalid option");
										}
									}
									else
									{
										Console.WriteLine("Error: Invalid option");
									}
								}
								else
								{
									Console.WriteLine("Error: Non-positive value");
								}
							}
							else
							{
								Console.WriteLine("Error: Invalid option");
							}
						}
						else
						{
							Console.WriteLine("Error: Invalid option");
						}
					}
					else
					{
						Console.WriteLine("Error: Invalid option");
					}
				}
				else
				{
					Console.WriteLine("Error: Not enough rates");
				}

				break;

			case "3":
				Console.WriteLine(converter.GetRatesInfo());
				break;

			case "4":
				if (converter.GetLen() > 1)
				{
					Console.WriteLine("Choose rate to modify:");
					var rates = converter.GetRates("UAH");
					for (int i = 0; i < rates.Count; i++)
					{
						Console.WriteLine($"{i + 1}. {rates[i]}");
					}
					option = Console.ReadLine();
					if (int.TryParse(option, out int optionInt))
					{
						if (optionInt <= rates.Count && optionInt > 0)
						{
							Console.WriteLine("Enter new rate value in hryvnia:");
							string newRateValue = Console.ReadLine();
							if (decimal.TryParse(newRateValue, out decimal newRateValueDecimal))
							{
								if (newRateValueDecimal > 0)
								{
									converter.AddRate(rates[optionInt - 1], newRateValueDecimal);
								}
								else
								{
									Console.WriteLine("Error: Non-positive value");
								}
							}
							else
							{
								Console.WriteLine("Error: Invalid option");
							}
						}
						else
						{
							Console.WriteLine("Error: Invalid option");
						}
					}
					else
					{
						Console.WriteLine("Error: Invalid option");
					}
				}
				else
				{
					Console.WriteLine("Error: Can't change UAH");
				}
				break;

			case "5":
				if (converter.GetLen() > 1)
				{
					Console.WriteLine("Choose rate to delete:");
					var rates = converter.GetRates("UAH");
					for (int i = 0; i < rates.Count; i++)
					{
						Console.WriteLine($"{i + 1}. {rates[i]}");
					}
					option = Console.ReadLine();
					if (int.TryParse(option, out int optionInt))
					{
						if (optionInt <= rates.Count && optionInt > 0)
						{
							converter.Delete(rates[optionInt - 1]);
							Console.WriteLine($"Successfully deleted {rates[optionInt - 1]}!");
						}
						else
						{
							Console.WriteLine("Error: Invalid option");
						}
					}
					else
					{
						Console.WriteLine("Error: Invalid option");
					}
				}
				else
				{
					Console.WriteLine("Error: Can't delete UAH");
				}
				break;

			case "6":
				Environment.Exit(0);
				break;

			default:
				Console.WriteLine("Error: Invalid option");
				break;
		}
	}
}

class Program
{
	static void Main(string[] args)
	{
		Executor executor = new Executor();

		while (true)
		{
			executor.Execute();
		}
	}
}