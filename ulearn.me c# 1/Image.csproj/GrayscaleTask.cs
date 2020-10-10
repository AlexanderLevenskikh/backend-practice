﻿namespace Recognizer
{
	public static class GrayscaleTask
	{
		/* 
		 * Переведите изображение в серую гамму.
		 * 
		 * original[x, y] - массив пикселей с координатами x, y. 
		 * Каждый канал R,G,B лежит в диапазоне от 0 до 255.
		 * 
		 * Получившийся массив должен иметь те же размеры, 
		 * grayscale[x, y] - яркость пикселя (x,y) в диапазоне от 0.0 до 1.0
		 *
		 * Используйте формулу:
		 * Яркость = (0.299*R + 0.587*G + 0.114*B) / 255
		 * 
		 * Почему формула именно такая — читайте в википедии 
		 * http://ru.wikipedia.org/wiki/Оттенки_серого
		 */

		public static double[,] ToGrayscale(Pixel[,] original)
		{
			var (firstDimension, secondDimension) = (original.GetLength(0), original.GetLength(1));
			var shades = new double[firstDimension,secondDimension];
			
			for (var i = 0; i < firstDimension; i++)
			{
				for (var j = 0; j < secondDimension; j++)
				{
					var pixel = original[i, j];
					shades[i, j] = (0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B) / 255;
				}
			}

			return shades;
		}
	}
}