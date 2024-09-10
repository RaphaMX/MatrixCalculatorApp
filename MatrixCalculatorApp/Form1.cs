using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatrixCalculatorApp
{
    public partial class Form1 : Form
    {
        private int[,] matrix;
        private TableLayoutPanel tblMatrix;
        private int matrixSize = 3; // Default matrix size
        private Panel matrixPanel;
        private ComboBox cmbMatrixSize;

        public Form1()
        {
            InitializeComponent();
            this.Padding = new Padding(2);
            this.BackColor = Color.FromArgb(98, 102, 244);

            // Initialize Matrix Panel
            InitializeMatrixPanel();
        }

        private void InitializeMatrixPanel()
        {
            // Create the panel where the matrix will be displayed
            matrixPanel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            // Create ComboBox for selecting matrix size
            cmbMatrixSize = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(20, 80),
                Size = new Size(100, 40)
            };

            // Populate ComboBox with matrix size options (from 2x2 to 5x5 for example)
            for (int i = 1; i <= 10; i++) // You can adjust the range
            {
                cmbMatrixSize.Items.Add(i.ToString());
            }
            cmbMatrixSize.SelectedIndex = 1; // Default to 3x3

            // Create buttons for matrix generation and determinant calculation
            Button generateMatrixButton = new Button
            {
                Text = "Gerar Matriz",
                Size = new Size(120, 40),
                Location = new Point(20, 20)
            };
            generateMatrixButton.Click += GenerateMatrixButton_Click;

            Button calculateDetButton = new Button
            {
                Text = "Calcular Determinante",
                Size = new Size(160, 40),
                Location = new Point(150, 20)
            };
            calculateDetButton.Click += CalculateDetButton_Click;

            // Add controls to panelDesktop
            panelDesktop.Controls.Add(generateMatrixButton);
            panelDesktop.Controls.Add(calculateDetButton);
            panelDesktop.Controls.Add(cmbMatrixSize);
            panelDesktop.Controls.Add(matrixPanel);
        }

        // Generate the matrix grid dynamically
        private void GenerateMatrixButton_Click(object sender, EventArgs e)
        {
            // Get matrix size from ComboBox
            matrixSize = int.Parse(cmbMatrixSize.SelectedItem.ToString());

            // Generate the matrix grid based on the selected size
            GenerateMatrixGrid(matrixSize);
        }

        // Method to generate the matrix grid dynamically inside the matrixPanel
        private void GenerateMatrixGrid(int size)
        {
            // Clear previous controls
            matrixPanel.Controls.Clear();

            // Create TableLayoutPanel to hold matrix cells
            tblMatrix = new TableLayoutPanel
            {
                RowCount = size,
                ColumnCount = size,
                AutoSize = true,
                Dock = DockStyle.None, // Remove DockStyle.Fill to allow manual positioning
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Set the alignment to center
            tblMatrix.Anchor = AnchorStyles.None;
            tblMatrix.Margin = new Padding(10); // Optional: Adds space around the matrix

            matrixPanel.Controls.Add(tblMatrix);

            // Center the matrixPanel controls manually
            tblMatrix.Location = new Point(
                (matrixPanel.ClientSize.Width - tblMatrix.Width) / 2,
                (matrixPanel.ClientSize.Height - tblMatrix.Height) / 2
            );

            matrix = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    TextBox txtBox = new TextBox
                    {
                        Name = $"txtMatrix_{i}_{j}",
                        Width = 50,
                        TextAlign = HorizontalAlignment.Center,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    tblMatrix.Controls.Add(txtBox, j, i);
                }
            }

            // Update location in case of resizing
            tblMatrix.LocationChanged += (s, e) =>
            {
                tblMatrix.Location = new Point(
                    (matrixPanel.ClientSize.Width - tblMatrix.Width) / 2,
                    (matrixPanel.ClientSize.Height - tblMatrix.Height) / 2
                );
            };
        }


        // Calculate the determinant when the button is clicked
        private void CalculateDetButton_Click(object sender, EventArgs e)
        {
            // Parse the values from TextBoxes into the matrix array
            try
            {
                for (int i = 0; i < matrixSize; i++)
                {
                    for (int j = 0; j < matrixSize; j++)
                    {
                        TextBox txtBox = (TextBox)tblMatrix.GetControlFromPosition(j, i);
                        matrix[i, j] = int.Parse(txtBox.Text);
                    }
                }

                int determinant = CalculateDeterminant(matrix);
                MessageBox.Show($"O determinante é: {determinant}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao calcular o determinante: " + ex.Message);
            }
        }

        // Function to calculate determinant recursively
        private int CalculateDeterminant(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            if (size == 1)
                return matrix[0, 0];

            if (size == 2)
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            int determinant = 0;
            for (int i = 0; i < size; i++)
            {
                int[,] subMatrix = CreateSubMatrix(matrix, 0, i);
                determinant += (int)Math.Pow(-1, i) * matrix[0, i] * CalculateDeterminant(subMatrix);
            }

            return determinant;
        }

        // Function to create a submatrix for determinant calculation
        private int[,] CreateSubMatrix(int[,] matrix, int excludeRow, int excludeCol)
        {
            int size = matrix.GetLength(0);
            int[,] subMatrix = new int[size - 1, size - 1];
            int r = -1;

            for (int i = 0; i < size; i++)
            {
                if (i == excludeRow)
                    continue;
                r++;
                int c = -1;
                for (int j = 0; j < size; j++)
                {
                    if (j == excludeCol)
                        continue;
                    subMatrix[r, ++c] = matrix[i, j];
                }
            }

            return subMatrix;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
