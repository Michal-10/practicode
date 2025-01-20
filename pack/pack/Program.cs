using System;
using System.CommandLine;

static string[] sortFiles(string sort, string[] arrFilesInDir)
{
    Console.WriteLine(sort);
    switch (sort)
    {
        //the options is code /
        //files - ברירת מחדל
        case "code":
            arrFilesInDir = arrFilesInDir.OrderBy(file => Path.GetExtension(file)).ToArray();
            break;
        default:
            arrFilesInDir = arrFilesInDir.OrderBy(file => file).ToArray();
            break;
    }
    return arrFilesInDir;
}
static void removeEmptyLinesFunc(string currentFile)
{
    var lines = File.ReadAllLines(currentFile);

    // סינון שורות ריקות
    var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line));

    // כתיבה חזרה לקובץ -מחליף את התוכן הישן
    File.WriteAllLines(currentFile, nonEmptyLines);

}
static void languagesFunc(string language, Dictionary<string, string[]> defineLanguages)
{
    if (language == "all")//כל השפות
    {
        foreach (var lan in defineLanguages)
        {
            lan.Value[lan.Value.Length - 1] = "true";
        }
    }
    else
    {
        var arrLanguage = language.Split(' ');//c# c css ....
        Console.WriteLine(arrLanguage.ToString() + "   " + arrLanguage.Length);

        foreach (string currentFile in arrLanguage)
        {
            if (defineLanguages.ContainsKey(currentFile))
            {
                Console.WriteLine(currentFile);
                defineLanguages[currentFile][defineLanguages[currentFile].Length - 1] = "true";
            }
            else
            {
                throw new Exception("The language is not allowed");
            }
        }
    }
}
static string GetUserInput(string prompt, bool isRequired = false)
{
    string input;
    do
    {
        Console.WriteLine(prompt);
        input = Console.ReadLine();
        if (isRequired && string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("This field is required!");
        }
    } while (isRequired && string.IsNullOrWhiteSpace(input));

    return input;
}


//language
var languageOption = new Option<string>("--language", "options of languages files")
{
    IsRequired = true
};
languageOption.AddAlias("-l");

//output
var outputOption = new Option<FileInfo>("--output", "File path and name"); ;
outputOption.AddAlias("-o");

//note
var noteOption = new Option<bool>("--note", "option to write the source code in a comment in the bundle file."); ;
noteOption.AddAlias("-n");

//sort
var sortOption = new Option<string>("--sort", () => "files", "sort the files")
{   //files / code
    IsRequired = false,

};
sortOption.Arity = ArgumentArity.ZeroOrOne;
sortOption.AddAlias("-s");

//remove
var removwEmptyLinesOption = new Option<bool>("--removeEmptyLines", "remove empty files")
{
    IsRequired = false //האופציה לא חובה
};
removwEmptyLinesOption.AddAlias("-r");

//author
var authorOption = new Option<string>("--author", "the name of the creator of the file."); ;
authorOption.AddAlias("-a");

var bundleCommand = new Command("bundle", "Bundle code files to a signle file");
bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(outputOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removwEmptyLinesOption);
bundleCommand.AddOption(authorOption);

bundleCommand.SetHandler((output, language, note, removwEmptyLines, author, sort) =>
{
    try
    {
        if(output == null)
        {
            throw new Exception("You did not set a file to save the data");
        }
        string dir = Environment.CurrentDirectory;//התיקייה הנוכחית שעליה רוצים לעבוד
        var currentDir = Path.GetFileName(dir.TrimEnd(Path.DirectorySeparatorChar));
        if (currentDir == "bin" || currentDir == "Debug")
        {
            Console.WriteLine("don't bundle files in a folder " + currentDir);
            Environment.Exit(0);
        }

        var arrFilesInDir = Directory.GetFiles(dir);

        if (string.IsNullOrEmpty(language))
        {
            throw new ArgumentException("The --language option requires a value.");
        }

        Dictionary<String, String[]> defineLanguages = new Dictionary<string, string[]> {
        { "c#", new[] { ".cs", "false" } },
        { "python", new[] { ".py", "false" } },
        { "c++", new[] { ".cpp",".h", "false" } },
        { "c", new[] { ".cpp",".h", "false" } },
        { "css", new[] { ".css", "false" } },
        { "word", new[] { ".docs", "false" } },
        { "json", new[] { ".json", "false" } },
        { "java", new[] { ".java", "false" } },
        { "javascript", new[] { ".js", "false" } },
        { "react", new[] { ".tsx","html","scc","css", "false" } },
        { "angular", new[] { ".ts", ".html", ".scc", "false" } },
        { "Assembly", new [] { ".asm", ".s","false" } },
        { "SQL", new []{ ".sql","true" } },
        };

        var file = new FileStream(output.FullName, FileMode.Create, FileAccess.Write);
        var fileToWrite = new BinaryWriter(file);
        Console.WriteLine("file was created");

        if (!string.IsNullOrEmpty(author))
        {
            fileToWrite.Write($"//author {author} \n\n");
        }

        languagesFunc(language, defineLanguages);//הגדרת השפות הרצויות

        //מיון הקבצים
        sort = sort == null ? "files" : sort;
        arrFilesInDir = sortFiles(sort, arrFilesInDir);

        foreach (string currentFile in arrFilesInDir)
        {
            string end = Path.GetExtension(currentFile);

            if (defineLanguages.Any(lang => lang.Value.Take(lang.Value.Length - 1).Contains(end) && lang.Value.Last() == "true"))

            {
                if (note == true)
                {
                    fileToWrite.Write($"// RelativePath: {Path.GetRelativePath(dir, currentFile)} {currentFile} \n\n\n");
                }

                if (removwEmptyLines)
                {
                    removeEmptyLinesFunc(currentFile);
                }
                fileToWrite.Write(File.ReadAllBytes(currentFile));
            }
        }
        fileToWrite.Close();
    }
    catch (IOException ex)
    {
        Console.WriteLine("Error: path is invalid");
    }
}, outputOption, languageOption, noteOption, removwEmptyLinesOption, authorOption, sortOption);

//command to create response file 
var create_rspCommand = new Command("bundleFiles", "make easyer to use the bundler command...");

create_rspCommand.SetHandler(() =>
{
    string output, languages, author, sort, note, toRemove;
    var rsp_file = new StreamWriter("res.rsp");

    output = GetUserInput("enter the name of the file and the path(not required - by default in this path)", true);
    languages = GetUserInput("enter the languages you want to include or all to include all languages \n if you want to choose some languages write in double quotes", true);
    author = GetUserInput("do you wand to enter the author name? enter it! ");
    sort = GetUserInput("enter how to sort (files / code) ");
    note = GetUserInput("do you want to write the source file? (y/n)");
    toRemove = GetUserInput("do you want to remove empty lines? (y/n)");

    //כתיבה לקובץ
    rsp_file.WriteLine("bundle ");
    rsp_file.WriteLine(" -o " + output);
    rsp_file.WriteLine(" -l " + languages);
    if (sort.Length > 0)
    {
        rsp_file.WriteLine(" -s " + sort);
    }
    if (note == "y" || note == "Y")
    {
        rsp_file.WriteLine(" -n ");
    }
    if (author.Length > 0)
    {
        rsp_file.WriteLine(" -a " + author);
    }
    if (toRemove == "y" || toRemove == "Y")
    {
        rsp_file.WriteLine(" -r ");
    }
    rsp_file.Close();
    Console.Write("you finish!!! now enter pack @res.rsp");
});


var rootCommand = new RootCommand("Root command file Bundler CLI");
rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(create_rspCommand);
await rootCommand.InvokeAsync(args);

