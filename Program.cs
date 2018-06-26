using System;

namespace Progressions
{
	class ApplicationContext {
		public static void Main() {

			IProgressionTypeValidation progressionTypeValidation = new ProgressionTypeValidation();

			INumberValidation numberValidation = new NumberValidation();

			IParameters commandLineParameters = new CommandLineParameters(
				progressionTypeValidation, numberValidation);

			CalculationFactory calculationFactory = new CalculationFactory();

			Controller controller = new Controller(commandLineParameters, calculationFactory);

			controller.run();
		}
	}

	class Controller
	{
		private IParameters parameters;
		private CalculationFactory calculationFactory;
        
		public Controller(IParameters parameters, CalculationFactory calculationFactory) {
			this.parameters = parameters;
			this.calculationFactory = calculationFactory;
		}

		public void run()
		{
			try
			{
				Console.WriteLine("Begin");
    
				parameters.GetParams();

				ICalculation calculation = calculationFactory.getProgression(parameters.ProgressionType);

				calculation.Processing(parameters);
            
				Console.WriteLine("End!!!");
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				Console.ReadLine();
			}
		}
	}


	public interface IProgressionTypeValidation
	{
		bool IsValid(string progressionTye);
	}
    
	public interface INumberValidation
    {
        bool IsValid(string input);
    }


	public class NumberValidation : INumberValidation
    {
		public bool IsValid(string input)
        {
			bool isNumeric = true;
			foreach (char c in input)
            {
                if (!Char.IsNumber(c))
                {
                    isNumeric = false;
                    break;
                }
            }

			return isNumeric;
        }
    }

	public class ProgressionTypeValidation : IProgressionTypeValidation
	{

		public bool IsValid(string progressionTye)
		{
			return progressionTye == ProgressionTye.AP.ToString();
		}
	}

	public interface ICalculation
	{
		void Processing(IParameters parameters);
	}

    public abstract class Calculation : ICalculation
    {
        public abstract void Processing(IParameters parameters);
    }

	public class APCalculation : Calculation
	{
		public override void Processing(IParameters parameters)
		{
			var temp = parameters.Start;

			for (int i = 0; i < parameters.ElementCount; i++)
			{
				Console.WriteLine(temp);

				temp += parameters.Diff;

			}
		}
	}


	public class GPCalculation : Calculation
    {
		public override void Processing(IParameters parameters)
        {
            var temp = parameters.Start;

            for (int i = 0; i < parameters.ElementCount; i++)
            {
                Console.WriteLine(temp);

                temp *= parameters.Diff;
            }
        }
    }

    class CalculationFactory
	{
		public Calculation getProgression(string progressionTye)
		{
			switch (progressionTye)
			{
				case "AP":
					return new APCalculation();
				default:
					throw new Exception("Not implement noob!");
			}
		}
	}

	public interface IParameters
	{
		string ProgressionType { get; set; }
		int Start { get; set; }
		int Diff { get; set; }
		int ElementCount { get; set; }
		void GetParams();
	}
       
	public class CommandLineParameters : IParameters
	{
		INumberValidation _validation;
		IProgressionTypeValidation _progressionTypeValidation;

		public CommandLineParameters(IProgressionTypeValidation progressionTypeValidation, INumberValidation validation)
		{
			_progressionTypeValidation = progressionTypeValidation;
			_validation = validation;
			
		}

		public string ProgressionType { get; set; }
		public int Start { get; set; }
		public int Diff { get; set; }
		public int ElementCount { get; set; }

		public void GetParams()
		{
			Console.Write("Input progression type: ");
			string progressionTypeStr = Console.ReadLine();

			if (!_progressionTypeValidation.IsValid(progressionTypeStr))
            {
                throw new ArgumentException("No support");
            }

			ProgressionType = progressionTypeStr;

			Console.Write("Input start number: ");

			string startStr = Console.ReadLine();
			Start = checkNumber(startStr);

			Console.Write("Input difference: ");

            string diffStr = Console.ReadLine();
			Diff = checkNumber(diffStr);

			Console.Write("Input number of element: ");

			string elementCountStr = Console.ReadLine();
			ElementCount = checkNumber(elementCountStr);
		}
        
		private int checkNumber(string str) {
			if (!_validation.IsValid(str))
            {
                throw new ArgumentException("Invalid parameter");
            }
			return int.Parse(str);
		}
	}

	public enum ProgressionTye
	{
		AP,
		GP,
		HP
	}
}
