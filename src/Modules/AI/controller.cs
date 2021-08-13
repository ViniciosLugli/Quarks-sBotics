
public class Controller {
	static void logMatrix(double[,] matrix) {
		int rowLength = matrix.GetLength(0);
		int colLength = matrix.GetLength(1);

		for (int i = 0; i < rowLength; i++) {
			for (int j = 0; j < colLength; j++) {
				Analyzer.log($"[{i}, {j}] = {matrix[i, j]}");
			}
		}
		Analyzer.log("");
	}

	static string ToMatrixString(double[,] matrix) {
		var s = new System.Text.StringBuilder();

		for (var i = 0; i < matrix.GetLength(0); i++) {
			s.Append("[");
			for (var j = 0; j < matrix.GetLength(1); j++) {
				s.Append(matrix[i, j]).Append(",");
			}
			s.Remove(s.Length - 1, 1);
			s.Append("]");
		}

		return s.ToString();
	}

	public static void train(double[,] trainingInputs, double[,] trainingOutputs, double[,] thinkOutput, int interactions = 128000) {
		var curNeuralNetwork = new AI.Neural.NeuralNetWork(1, trainingInputs.GetLength(1));

		Analyzer.logLine("INFOS");
		Analyzer.log($"trainingInputs: [{ToMatrixString(trainingInputs)}]");
		Analyzer.log($"trainingOutputs: [{ToMatrixString(trainingOutputs)}]");
		Analyzer.log($"thinkOutput: [{ToMatrixString(thinkOutput)}]");
		Analyzer.log($"interactions: {interactions}lps");
		Analyzer.logLine();
		Analyzer.log("");

		Analyzer.logLine("TRAIN WORK");
		Analyzer.log($"[{Time.date}] Random synaptic weights before training:");
		Controller.logMatrix(curNeuralNetwork.SynapsesMatrix);

		curNeuralNetwork.train(trainingInputs, AI.Neural.NeuralNetWork.MatrixTranspose(trainingOutputs), interactions);

		Analyzer.log($"[{Time.date}] Result synaptic weights after training:");
		Controller.logMatrix(curNeuralNetwork.SynapsesMatrix);

		Log.debug($"Train finished in {Time.current.millis}ms");
		Analyzer.log($"[{Time.date}] Train finished in {Time.current.millis}ms");
		Analyzer.logLine();
		Analyzer.log("");

		Analyzer.logLine("THINK PROCESS");
		// testing neural networks against a new problem
		Analyzer.log($"[{Time.date}] result of problems => [{ToMatrixString(thinkOutput)}]:");

		Time.resetTimer();

		Controller.logMatrix(curNeuralNetwork.think(thinkOutput));

		Analyzer.log($"[{Time.date}] Think finished in {Time.current.millis}ms");
		Log.debug($"Think finished in {Time.current.millis}ms");
		Analyzer.logLine();
		Time.skipFrame();
	}
}
