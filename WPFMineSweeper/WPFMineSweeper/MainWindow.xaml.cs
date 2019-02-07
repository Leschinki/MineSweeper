using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFMineSweeper
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MineSweeper game;
        public MainWindow()
        {
            InitializeComponent();
            game = new MineSweeper(mainCanvas);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            game.createGameGrid(16, 16,40);
            game.DrawGameField();
        }
    }
}
