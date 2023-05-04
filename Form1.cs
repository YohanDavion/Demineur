using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Demineur : Form
    {
        // Yohan Davion
        //########################################"
        // CONFIGURATION DU DEMINEUR
        // Taille de la grille
        private int gridSize = 10;
        // Nombre de Mine
        private int numberOfMine = 10;
        //########################################"

        private Random random = new Random();
        private Button[,] buttons = new Button [10,10];
        private bool[,] mines = new bool[10, 10];
        private int discoverButton = 0;

        public Demineur()
        {
            InitializeComponent();
            GenerateGrid();
            InitMines();
        }
        private void Demineur_Load(object sender, EventArgs e)
        {

        }

        private void GenerateGrid()
        {
            for (int i = 0; i < this.gridSize; i++)
            {
                for (int bt = 0; bt < this.gridSize; bt++)
                {
                    Button button = new Button();
                    button.Size = new Size(50, 50);
                    button.Location = new Point(bt * 50 + 10, 50 * i + 10); //Coordonnées x et y de création du bouton(x_index * x_taille + x_marge, y_index * y_taille + y_marge)
                    button.Font = new Font(button.Font.FontFamily, 20);
                    buttons[bt,i] = button;
                    button.MouseDown += new MouseEventHandler(Button_MouseDown);
                    Controls.Add(button);
                }
            }
        }

        private void InitMines()
        {
            int randX, randY;
            for (int i = 0; i < this.numberOfMine; i++)
            {
                randX = random.Next(0, this.gridSize);
                randY = random.Next(0, this.gridSize);

                while(mines[randX, randY])
                {
                randX = random.Next(0, this.gridSize);
                randY = random.Next(0, this.gridSize);
                }

                mines[randX, randY] = true;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            int row = (button.Location.X - this.gridSize) / 50;
            int col = (button.Location.Y - this.gridSize) / 50;

            if (e.Button == MouseButtons.Right)
            {
                showM(row, col);
            }
            else if (e.Button == MouseButtons.Left)
            {

                int nbMines = CountMines(row, col);
                if (!mines[row,col])
                {
                    if(nbMines==0)
                    {
                        RevealEmptyCells(row, col);
                    }
                    else
                    {
                        buttons[row, col].Enabled = false;
                        this.discoverButton++;
                        buttons[row, col].Text = nbMines.ToString();
                    }
                }
                else
                {
                    Loose(row,col);
                }
                CheckForWin();
            }

        }

        private void showM(int row, int col)
        {
            if (buttons[row, col].Text.Equals("M"))
            {
                buttons[row, col].Text = "";
                buttons[row, col].ForeColor = Color.Black;
            }
            else
            {
                buttons[row, col].Text = "M";
                buttons[row, col].ForeColor = Color.Red;
            }
        }

        private void RevealEmptyCells(int LigneX, int ColY)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (((LigneX - 1) + x < this.gridSize && (LigneX - 1) + x >= 0) && ((ColY - 1) + y < this.gridSize && (ColY - 1) + y >= 0) && buttons[(LigneX - 1) + x, (ColY - 1) + y].Enabled)
                    {
                        if ((CountMines((LigneX - 1) + x, (ColY - 1) + y) == 0))
                        {
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Enabled = false;
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Text = "";
                            this.discoverButton++;
                            RevealEmptyCellsBIS((LigneX - 1) + x, (ColY - 1) + y);
                        }
                        else
                        {
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Enabled = false;
                            this.discoverButton++;
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Text = CountMines((LigneX - 1) + x, (ColY - 1) + y).ToString();
                        }
                    }
                }
            }
        }

        private void RevealEmptyCellsBIS(int LigneX, int ColY)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (((LigneX - 1) + x < this.gridSize && (LigneX - 1) + x >= 0) && ((ColY - 1) + y < this.gridSize && (ColY - 1) + y >= 0) && buttons[(LigneX - 1) + x, (ColY - 1) + y].Enabled)
                    {
                        if ((CountMines((LigneX - 1) + x, (ColY - 1) + y) == 0))
                        {
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Enabled = false;
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Text = "";
                            this.discoverButton++;
                            RevealEmptyCells((LigneX - 1) + x, (ColY - 1) + y);
                        }
                        else
                        {
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Enabled = false;
                            this.discoverButton++;
                            buttons[(LigneX - 1) + x, (ColY - 1) + y].Text = CountMines((LigneX - 1) + x, (ColY - 1) + y).ToString();
                        }
                    }
                }
            }
        }

        private int CountMines(int LigneX,int ColY)
        {
            int nbMines = 0;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if ((((LigneX-1) + x < this.gridSize && (LigneX - 1) + x >= 0) && ((ColY-1) + y < this.gridSize && (ColY - 1) + y >= 0)) && (mines[(LigneX - 1) + x, (ColY - 1) + y]))
                    {
                            nbMines++;
                    }
                }
            }
            return nbMines;
        }

        private void CheckForWin()
        {
            if (this.discoverButton == (this.gridSize * this.gridSize) - this.numberOfMine)
            {
                Win();
            }
        }

        private void Loose(int row,int col)
        {
            buttons[row, col].Text = "X";
            buttons[row, col].ForeColor = Color.Red;
            MessageBox.Show("Vous avez Perdu !", "Perdu",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
            replay();
        }

        private void Win()
        {
            MessageBox.Show("Vous avez Gagné !", "Gagné",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
            replay();
        }

        private void replay()
        {
            DialogResult dialogResult = MessageBox.Show("Voulez-vous rejouer !", "Replay",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
        }

    }
}
