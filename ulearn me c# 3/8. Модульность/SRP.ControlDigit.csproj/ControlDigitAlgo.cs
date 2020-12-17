using System.Collections.Generic;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
	    public static IEnumerable<int> ToDigitsEnumerable(this long number)
	    {
		    do
		    {
			    var digit = (int)(number % 10);
			    yield return digit;
			    number /= 10;

		    }
		    while (number > 0);
	    } 
    }

    public static class ControlDigitAlgo
	{
		public static int Upc(long number)
		{
			int sum = 0;
			int factor = 3;
			foreach (var digit in number.ToDigitsEnumerable())
			{
				sum += factor * digit;
				factor = 4 - factor;
			}
			int result = sum % 10;
			if (result != 0) result = 10 - result;
			return result;
		}

		public static char Isbn10(long number)
		{
			var sum = 0;
			var index = 2;
			foreach (var digit in number.ToDigitsEnumerable())
			{
				sum += digit * index; 
				index++;
			}
			var result = (11 - sum % 11) % 11;
			return result == 10 ? 'X' : (char)(result + 48);
		}

        public static int Luhn(long number)
        {
	        var sum = 0;
	        var index = 0;
	        foreach (var digit in number.ToDigitsEnumerable())
	        {
		        var next = index % 2 == 0 ? digit * 2 : digit;
		        if (next > 9) next -= 9;
		        sum += next;
		        index++;
	        }
	        return sum * 9 % 10;
        }
	}
}
