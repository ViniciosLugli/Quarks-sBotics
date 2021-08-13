public static class Analyzer {

	public static void setup() {
		bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/releases/AI.log");
		bc.EraseConsoleFile();
	}

	public static void log(object info) {
		bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/releases/AI.log");
		bc.WriteText(info.ToString());
	}

	public static void logLine(object info) {
		bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/releases/AI.log");
		var len = info.ToString().Length;
		bc.WriteText($"{(Calc.repeatString("-", (50 - (len / 2)) - 1))} {info.ToString()} {(Calc.repeatString("-", (50 - (len / 2)) - 1))}");
	}

	public static void logLine() {
		bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/releases/AI.log");
		bc.WriteText(Calc.repeatString("-", 101));
	}

	public static void clear() {
		bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/releases/AI.log");
		bc.EraseConsoleFile();
	}
}

public static class Result {
	public static void export(object info, string filename) {
		bc.SetFileConsolePath($"/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/src/Variables/{filename}");
		bc.WriteText($"[{Time.date}] {info.ToString()}");
	}

	public static void clear(string filename) {
		bc.SetFileConsolePath($"/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/src/Variables/{filename}");
		bc.EraseConsoleFile();
	}
}
