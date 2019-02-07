using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFMineSweeper
{
    class MineSweeper
    {
        private CellType[,] grid;
        private Random rnd = new Random();
        private WPFDraw WPFCanvas;
        private Canvas canvas;
        private int rowsCount;
        private int columnsCount;
        private int bombsCount;
        public MineSweeper(Canvas canvas_)
        {
            canvas = canvas_;
            canvas.MouseDown += new MouseButtonEventHandler(OnCanvasClicked);
            WPFCanvas = new WPFDraw(canvas_);
        }


        public void createGameGrid(int rows, int columns, int bombs)
        {
            rowsCount = rows;
            columnsCount = columns;
            bombsCount = bombs;
            grid = new CellType[rowsCount, columnsCount];
            fillGameGrid();
        }
        private void fillGameGrid()
        {
            int bombsPlaced = 0;
            int x, y;
            if (bombsCount > rowsCount * columnsCount)
                throw new Exception("TO MANY BOMBS");
            while (bombsPlaced < bombsCount)
            {
                x = rnd.Next(rowsCount);
                y = rnd.Next(columnsCount);

                if (grid[x, y] == null || grid[x, y].Type.Equals(CELLTYPES.EMPTY))
                {
                    grid[x, y] = new CellType(CELLTYPES.BOMB);
                    bombsPlaced++;
                }
            }
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    if (grid[i, j] == null)
                    {
                        if (getNeighborCount(i, j) > 0)
                            grid[i, j] = new CellType(CELLTYPES.NUMBER, getNeighborCount(i, j));
                        else
                            grid[i, j] = new CellType(CELLTYPES.EMPTY);
                    }
                }
            }
        }
        private int getNeighborCount(int x, int y)
        {
            int sum = 0;
            sum += isBomb(x + 1, y + 1) ? 1 : 0;
            sum += isBomb(x - 1, y - 1) ? 1 : 0;
            sum += isBomb(x + 1, y - 1) ? 1 : 0;
            sum += isBomb(x - 1, y + 1) ? 1 : 0;

            sum += isBomb(x + 1, y) ? 1 : 0;
            sum += isBomb(x, y + 1) ? 1 : 0;
            sum += isBomb(x - 1, y) ? 1 : 0;
            sum += isBomb(x, y - 1) ? 1 : 0;

            return sum;
        }
        private bool isBomb(int x, int y)
        {
            if (x >= rowsCount)
                return false;
            if (y >= columnsCount)
                return false;
            if (x < 0)
                return false;
            if (y < 0)
                return false;
            if (grid[x, y] == null)
                return false;
            return (grid[x, y].Type == CELLTYPES.BOMB) ? true : false;
        }
        private bool isEmpty(int x, int y)
        {
            if (x >= rowsCount)
                return false;
            if (y >= columnsCount)
                return false;
            if (x < 0)
                return false;
            if (y < 0)
                return false;
            if (grid[x, y] == null)
                return false;
            return (grid[x, y].Type == CELLTYPES.EMPTY) ? true : false;
        }
        public void DrawGameField()
        {
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;
            double rowHeight = height / rowsCount;
            double columnwidth = width / columnsCount;
            WPFCanvas.Clear();
            WPFCanvas.DrawGrid(rowsCount, columnsCount, Brushes.LightGray);
            canvas.Background = Brushes.DarkGray;
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    if (grid[i, j].Opened)
                    {
                        switch (grid[i, j].Type)
                        {
                            case CELLTYPES.BOMB:
                                WPFCanvas.DrawRectangle(j * rowHeight, i * columnwidth, columnwidth, rowHeight, Brushes.White);
                                WPFCanvas.DrawEllipse(j * rowHeight, i * columnwidth, columnwidth, rowHeight, Brushes.Black);
                                MessageBox.Show("Game Over");
                                restart();
                                break;
                            case CELLTYPES.NUMBER:
                                WPFCanvas.DrawRectangle(j * rowHeight, i * columnwidth, columnwidth, rowHeight, Brushes.White);
                                WPFCanvas.DrawText(i * columnwidth, j * rowHeight, rowHeight, columnwidth, grid[i, j].Value.ToString(), Colors.Blue);
                                break;
                            case CELLTYPES.EMPTY:
                                WPFCanvas.DrawRectangle(j * rowHeight, i * columnwidth, columnwidth, rowHeight, Brushes.White);
                                break;
                        }
                    }
                    else
                    {
                        if (grid[i, j].Flag)
                        {
                            WPFCanvas.DrawEllipse(j * rowHeight, i * columnwidth, columnwidth, rowHeight, Brushes.Red);
                        }
                    }
                }
            }
            CheckForWin();
        }
        private void restart()
        {
            createGameGrid(rowsCount, columnsCount, bombsCount);
            DrawGameField();
        }
        private void FloodFill(int x, int y)
        {
            if (x < 0 || x >= columnsCount)
                return;
            if (y < 0 || y >= rowsCount)
                return;
            if (grid[x, y].Opened)
                return;
            if(grid[x, y].Type == CELLTYPES.NUMBER)
            {
                grid[x, y].Opened = true;
                return;
            }
            if(grid[x,y].Type == CELLTYPES.EMPTY)
            {
                grid[x, y].Opened = true;
                FloodFill(x+1, y);
                FloodFill(x-1, y);
                FloodFill(x, y+1);
                FloodFill(x, y-1);

                FloodFill(x+1, y+1);
                FloodFill(x-1, y-1);
                FloodFill(x-1, y+1);
                FloodFill(x+1, y-1);
            }
        }
        private void CheckForWin()
        {
            int flags = 0;
            int open = 0;
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    switch (grid[i, j].Type)
                    {
                        case CELLTYPES.NUMBER:
                            if (grid[i, j].Opened)
                                open++;
                            break;
                        case CELLTYPES.EMPTY:
                            if (grid[i, j].Opened)
                                open++;
                            break;
                    }
                    if (grid[i, j].Flag)
                        flags++;
                }
            }
            if (bombsCount == flags && open == ((rowsCount * columnsCount) - flags))
            {
                MessageBox.Show("YOU WIN");
                Application.Current.Shutdown();
            }
        }
        private bool OpenNeighbors(int i, int j)
        {
            bool opened = false;
            if (isEmpty(i + 1, j + 1))
            {
                grid[i + 1, j + 1].Opened = true;
                opened = true;
            }
            if (isEmpty(i - 1, j - 1))
            {
                grid[i - 1, j - 1].Opened = true;
                opened = true;
            }
            if (isEmpty(i - 1, j + 1))
            {
                grid[i - 1, j + 1].Opened = true;
                opened = true;
            }
            if (isEmpty(i + 1, j - 1))
            {
                grid[i + 1, j - 1].Opened = true;
                opened = true;
            }
            if (isEmpty(i + 1, j))
            {
                grid[i + 1, j].Opened = true;
                opened = true;
            }
            if (isEmpty(i, j + 1))
            {
                grid[i, j + 1].Opened = true;
                opened = true;
            }
            if (isEmpty(i - 1, j))
            {
                grid[i - 1, j].Opened = true;
                opened = true;
            }
            if (isEmpty(i, j - 1))
            {
                grid[i, j - 1].Opened = true;
                opened = true;
            }
            return opened;
        }
        private void OnCanvasClicked(object sender, MouseButtonEventArgs e)
        {
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;
            double rowHeight = height / rowsCount;
            double columnwidth = width / columnsCount;
            int x = (int)(e.GetPosition(canvas).X / columnwidth);
            int y = (int)(e.GetPosition(canvas).Y / rowHeight);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (grid[x, y].Flag)
                    return;
                if (grid[x, y].Type.Equals(CELLTYPES.EMPTY))
                    FloodFill(x, y);
                grid[x, y].Opened = true;
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (!grid[x, y].Opened)
                    grid[x, y].Flag = !grid[x, y].Flag;
            }
            DrawGameField();
        }
    }
    class CellType
    {
        private CELLTYPES type;
        private int value;
        private bool opened = false;
        private bool flag = false;
        public CellType(CELLTYPES type_, int value_ = 0, bool flagSet = false)
        {
            type = type_;
            value = value_;
            flag = flagSet;
        }

        public int Value
        {
            get => value;
        }
        public CELLTYPES Type
        {
            get => type;
        }
        public bool Opened
        {
            get => opened;
            set => opened = value;
        }
        public bool Flag
        {
            get => flag;
            set => flag = value;
        }
    }
    public enum CELLTYPES
    {
        EMPTY = 0,
        BOMB = 1,
        NUMBER = 2,
    }
}
