
using System.Text;

class Converter 
{
	private decimal usdRate;
	private decimal euroRate;

	public Converter(decimal usdRate, decimal euroRate) 
	{ 
		this.usdRate = usdRate;
		this.euroRate = euroRate;
	}

	public decimal convertFromUAH(decimal sum, int rate) 
	{
		if (rate == 1)
		{
			return sum * usdRate;
		}
		else 
		{
			return sum * euroRate;
		}
	}

	public decimal convertFromUSD(decimal sum)
	{ 
		return sum / usdRate;
	}

	public decimal convertFromEURO(decimal sum)
	{
		return sum / euroRate;
	}
}

class Program
{
	static void Main(string[] args)
	{
		Console.OutputEncoding = Encoding.UTF8;

		Converter converter = new Converter(0.0270649m, 0.02586m);
		List<string> rates = new List<string>(new string[] { "UAH", "USD", "EURO" });

		Console.WriteLine("Введіть курс вхідних даних:");
		for (int i = 0; i < rates.Count; i++) { 
			Console.WriteLine($"{i + 1}. {rates[i]}");
		}
		int convertIn = int.Parse(Console.ReadLine());
		if (convertIn == 1 || convertIn == 2 || convertIn == 3)
		{
			Console.WriteLine("Введіть суму:");
			int convertSum = int.Parse(Console.ReadLine());

			if (convertSum <= 0)
			{
				Console.WriteLine("Помилка: некоректна сума");
				return;
			}

			if (convertIn == 1)
			{
				Console.WriteLine("Введіть курс вихідних даних:");

				for (int i = 1; i < rates.Count; i++)
				{
					Console.WriteLine($"{i}. {rates[i]}");
				}
				int convertOut = int.Parse(Console.ReadLine());
				if (convertOut == 1 || convertOut == 2)
				{
					Console.WriteLine($"{convertSum} грн = {converter.convertFromUAH(convertSum, convertOut)} {rates[convertOut]}");
				}
				else
				{
					Console.WriteLine("Помилка: некоректна команда");
					return;
				}
			}
			else 
			{

				decimal outputSum = (convertIn == 2) ? converter.convertFromUSD(convertSum) : converter.convertFromEURO(convertSum);
				Console.WriteLine($"{convertSum} {rates[convertIn - 1]} = {outputSum} грн");
			}
		}
		else
		{
			Console.WriteLine("Помилка: некоректна команда");
			return;
		}
	}
}