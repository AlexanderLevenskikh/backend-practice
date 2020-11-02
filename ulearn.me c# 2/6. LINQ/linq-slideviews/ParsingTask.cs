using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace linq_slideviews
{
    public class ParsingTask
    {
        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
        /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
        /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1)
                .Select(line => line.Split(';'))
                .Where(chunks => chunks.Length == 3 &&
                                 int.TryParse(chunks[0], out var id) &&
                                 Enum.TryParse(chunks[1], true, out SlideType slideType))
                .Select(chunks => (int.Parse(chunks[0]), Enum.Parse(typeof(SlideType), chunks[1], true), chunks[2]))
                .ToDictionary(x => x.Item1, x => new SlideRecord(
                    x.Item1,
                    (SlideType) x.Item2,
                    x.Item3));
        }

        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
        /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
        /// Такой словарь можно получить методом ParseSlideRecords</param>
        /// <returns>Список информации о посещениях</returns>
        /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1)
                .Select(line =>
                {
                    var chunks = line.Split(';');

                    if (
                        chunks.Length == 4 &&
                        int.TryParse(chunks[0], out var userId) &&
                        int.TryParse(chunks[1], out var slideId) &&
                        DateTime.TryParse($"{chunks[2]}T{chunks[3]}", out DateTime dateTime) &&
                        slides.ContainsKey(slideId)
                    )
                        return new VisitRecord(
                            userId,
                            slideId,
                            dateTime,
                            slides[slideId].SlideType);

                    throw new FormatException($"Wrong line [{line}]");
                });
        }
    }
}