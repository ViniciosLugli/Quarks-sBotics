const byte MARGIN_BLACK_VALUE = 53;

public struct Data {
	public double[] input;
	public double output;

	public Data(double[] _input, double _output) {
		this.input = _input;
		this.output = _output;
	}
}

public class Trainner {
	public static Data reflectives(double defaultOutput) {
		return new Data(
			new double[] { s2.light.raw, s2.isMat() ? 1 : 0 },
			defaultOutput
		);
	}
}
