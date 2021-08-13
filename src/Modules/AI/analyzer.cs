public static class Analyzer {

	public static void setup() {
		Analyzer.setOrigin();
		bc.EraseConsoleFile();
	}

	public static void setOrigin() => bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/releases/AI.log");

	public static void log(object info) {
		Analyzer.setOrigin();
		bc.WriteText(info.ToString());
	}

	public static void logLine(object info) {
		Analyzer.setOrigin();
		var len = info.ToString().Length;
		bc.WriteText($"{(Calc.repeatString("-", (50 - (len / 2)) - 1))} {info.ToString()} {(Calc.repeatString("-", (50 - (len / 2)) - 1))}");
	}

	public static void logLine() {
		Analyzer.setOrigin();
		bc.WriteText(Calc.repeatString("-", 101));
	}

	public static void clear() {
		Analyzer.setOrigin();
		bc.EraseConsoleFile();
	}
}

public static class Result {
	public static void setOrigin(string filename) => bc.SetFileConsolePath($"/home/vinicioslugli/Documentos/scripts/sbotics/Codes/Quarks-sBotics/src/Variables/{filename}");

	public static void export(object info, string filename) {
		Result.clear(filename);
		bc.WriteText(info.ToString());
	}

	public static void clear(string filename) {
		Result.setOrigin(filename);
		bc.EraseConsoleFile();
	}
}
