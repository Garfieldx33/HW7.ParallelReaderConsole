// See https://aka.ms/new-console-template for more information
using HW7.ParallelReaderConsole;
using System.IO;

Console.WriteLine("Введите путь к папке, из которой лежать файлы для чтения");
//string path = Console.ReadLine();
string path = @"C:\txts\";
if (path != string.Empty)
{
    if (Directory.Exists(path))
    {
        List<string> files = SearchTxtFilesInFolder(path);

        Console.WriteLine("Запускаем синхронное чтение и подсчет пробелов");
        double syncTime= WhiteSpaceCounter.GetTimeOfCounting(files);
        Console.WriteLine($"Синхронное выполнение операций завершено за {syncTime} секунд");

        Console.WriteLine("Запускаем асинхронное чтение и подсчет пробелов");
        double asyncTime = WhiteSpaceCounter.GetTimeOfCountingAsync(files).Result;
        Console.WriteLine($"Асинхронное выполнение операций завершено за {asyncTime} секунд");

        Console.WriteLine("Запускаем параллельное чтение и подсчет пробелов");
        double parTime = WhiteSpaceCounter.GetTimeOfCountingParallel(files).Result;
        Console.WriteLine($"Параллельное выполнение операций завершено за {parTime} секунд");
    }
    else
    {
        Console.WriteLine("Папка по указанному пути не найдена");
    }
}
else
{
    Console.WriteLine("Перезапустите приложение и введите правильный путь к папке с файлами");
}
Console.WriteLine("Нажмите на любую клавишу для выхода");
Console.ReadKey();


static List<string> SearchTxtFilesInFolder(string basePath)
{
    return Directory.GetFiles(basePath, "*.txt", SearchOption.TopDirectoryOnly).ToList();
}