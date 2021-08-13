public class Neural {
	public class NeuralNetWork {
		private Random _radomObj;

		public NeuralNetWork(int synapseMatrixColumns, int synapseMatrixLines) {
			SynapseMatrixColumns = synapseMatrixColumns;
			SynapseMatrixLines = synapseMatrixLines;

			_Init();
		}

		public NeuralNetWork(int synapseMatrixColumns, int synapseMatrixLines, double[,] synapsesMatrixGived) {
			SynapseMatrixColumns = synapseMatrixColumns;
			SynapseMatrixLines = synapseMatrixLines;
			SynapsesMatrix = synapsesMatrixGived;
		}

		public int SynapseMatrixColumns { get; }
		public int SynapseMatrixLines { get; }
		public double[,] SynapsesMatrix { get; private set; }

		private void _Init() {
			// make sure that for every instance of the neural network we are geting the same radom values
			_radomObj = new Random(1);
			_generateSynapsesMatrix();
		}

		private void _generateSynapsesMatrix() {
			SynapsesMatrix = new double[SynapseMatrixLines, SynapseMatrixColumns];

			for (var i = 0; i < SynapseMatrixLines; i++) {
				for (var j = 0; j < SynapseMatrixColumns; j++) {
					SynapsesMatrix[i, j] = (2 * _radomObj.NextDouble()) - 1;
				}
			}
		}

		private double[,] _calculateSigmoid(double[,] matrix) {

			int rowLength = matrix.GetLength(0);
			int colLength = matrix.GetLength(1);

			for (int i = 0; i < rowLength; i++) {
				for (int j = 0; j < colLength; j++) {
					var value = matrix[i, j];
					matrix[i, j] = 1 / (1 + Math.Exp(value * -1));
				}
			}
			return matrix;
		}

		private double[,] _calculateSigmoidDerivative(double[,] matrix) {
			int rowLength = matrix.GetLength(0);
			int colLength = matrix.GetLength(1);

			for (int i = 0; i < rowLength; i++) {
				for (int j = 0; j < colLength; j++) {
					var value = matrix[i, j];
					matrix[i, j] = value * (1 - value);
				}
			}
			return matrix;
		}


		public double[,] think(double[,] inputMatrix) {
			var productOfTheInputsAndWeights = matrixDotProduct(inputMatrix, SynapsesMatrix);
			return _calculateSigmoid(productOfTheInputsAndWeights);
		}

		public double[,] think(double[,] inputMatrix, double[,] SynapsesMatrixGived) {
			var productOfTheInputsAndWeights = matrixDotProduct(inputMatrix, SynapsesMatrixGived);
			return _calculateSigmoid(productOfTheInputsAndWeights);
		}

		public void train(double[,] trainInputMatrix, double[,] trainOutputMatrix, int interactions) {
			// we run all the interactions
			for (var i = 0; i < interactions; i++) {
				// calculate the output
				var output = think(trainInputMatrix);

				// calculate the error
				var error = matrixSubstract(trainOutputMatrix, output);
				var curSigmoidDerivative = _calculateSigmoidDerivative(output);
				var error_SigmoidDerivative = matrixProduct(error, curSigmoidDerivative);

				// calculate the adjustment
				var adjustment = matrixDotProduct(MatrixTranspose(trainInputMatrix), error_SigmoidDerivative);

				SynapsesMatrix = matrixSum(SynapsesMatrix, adjustment);
			}
		}

		public static double[,] MatrixTranspose(double[,] matrix) {
			int w = matrix.GetLength(0);
			int h = matrix.GetLength(1);

			double[,] result = new double[h, w];

			for (int i = 0; i < w; i++) {
				for (int j = 0; j < h; j++) {
					result[j, i] = matrix[i, j];
				}
			}

			return result;
		}

		public static double[,] matrixSum(double[,] matrixa, double[,] matrixb) {
			var rowsA = matrixa.GetLength(0);
			var colsA = matrixa.GetLength(1);

			var result = new double[rowsA, colsA];

			for (int i = 0; i < rowsA; i++) {
				for (int u = 0; u < colsA; u++) {
					result[i, u] = matrixa[i, u] + matrixb[i, u];
				}
			}

			return result;
		}

		public static double[,] matrixSubstract(double[,] matrixa, double[,] matrixb) {
			var rowsA = matrixa.GetLength(0);
			var colsA = matrixa.GetLength(1);

			var result = new double[rowsA, colsA];

			for (int i = 0; i < rowsA; i++) {
				for (int u = 0; u < colsA; u++) {
					result[i, u] = matrixa[i, u] - matrixb[i, u];
				}
			}

			return result;
		}

		public static double[,] matrixProduct(double[,] matrixa, double[,] matrixb) {
			var rowsA = matrixa.GetLength(0);
			var colsA = matrixa.GetLength(1);

			var result = new double[rowsA, colsA];

			for (int i = 0; i < rowsA; i++) {
				for (int u = 0; u < colsA; u++) {
					result[i, u] = matrixa[i, u] * matrixb[i, u];
				}
			}

			return result;
		}

		public static double[,] matrixDotProduct(double[,] matrixa, double[,] matrixb) {

			var rowsA = matrixa.GetLength(0);
			var colsA = matrixa.GetLength(1);

			var rowsB = matrixb.GetLength(0);
			var colsB = matrixb.GetLength(1);

			if (colsA != rowsB)
				throw new Exception("Matrices dimensions don't fit.");

			var result = new double[rowsA, colsB];

			for (int i = 0; i < rowsA; i++) {
				for (int j = 0; j < colsB; j++) {
					for (int k = 0; k < rowsB; k++)
						result[i, j] += matrixa[i, k] * matrixb[k, j];
				}
			}
			return result;
		}

	}
}
