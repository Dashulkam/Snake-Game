using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SnakeGame
{
    public partial class MainWindow : Window, IGameUI
    {
        //commer
        private Game game; // Объявление переменной для игры
        private DispatcherTimer timer; // Таймер для обновления игры
        Results results;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame(); // Инициализация игры
            this.KeyDown += MainWindow_KeyDown; // Подписка на событие нажатия клавиш

            results = new Results();

        }

        private void InitializeGame()
        {
            game = new Game(400, 400); // Инициализация игры с размерами 400x400
            timer = new DispatcherTimer(); // Инициализация таймера
            timer.Interval = TimeSpan.FromMilliseconds(100); // Установка интервала таймера
            timer.Tick += (s, e) => UpdateGame(); // Подписка на событие Tick таймера
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartGame(); // Запуск игры при нажатии кнопки "Старт"
        }

        private void StartGame()
        {
            
            PlayerNameTextBox.IsEnabled = false; // Отключаем текстовое поле для ввода имени игрока
            game = new Game(400, 400); // Создаем новый экземпляр игры
            game.Score.Reset(); // Сброс счета

            // Установка уровня сложности
            string difficulty = GetSelectedDifficulty();
            game.InitializeObstacles(difficulty); // Инициализация препятствий в зависимости от уровня

            // Изменение скорости таймера
            SetTimerInterval(difficulty);

            timer.Start(); // Запуск таймера
            UpdateGame(); // Первоначальный вызов метода обновления для отрисовки начального состояния
            

        }

        private void SetTimerInterval(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    timer.Interval = TimeSpan.FromMilliseconds(150); // Медленная скорость
                    break;
                case "Medium":
                    timer.Interval = TimeSpan.FromMilliseconds(100); // Средняя скорость
                    break;
                case "Hard":
                    timer.Interval = TimeSpan.FromMilliseconds(70); // Быстрая скорость
                    break;
                default:
                    timer.Interval = TimeSpan.FromMilliseconds(150); // По умолчанию легкий уровень
                    break;
            }
        }

        private string GetSelectedDifficulty()
        {
            if (EasyLevel.IsChecked == true)
                return "Easy";
            if (MediumLevel.IsChecked == true)
                return "Medium";
            if (HardLevel.IsChecked == true)
                return "Hard";

            return "Easy"; // По умолчанию легкий уровень
        }

        public void UpdateScore(int score)
        {
            ScoreLabel.Content = $"Score: {score}"; // Обновление элемента UI, отображающего счет
        }

        public void ShowGameOver()
        {
            DatagradHighScore.ItemsSource = results.getPlayersResults();

            MessageBoxResult result = MessageBox.Show("Игра окончена! Хотите начать заново?",
                                                       "Конец игры",
                                                       MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                StartGame();
            }
            else
            {
                // Запрашиваем сохранение результата
                MessageBoxResult saveResult = MessageBox.Show("Хотите сохранить результат?",
                                                              "Сохранение результата",
                                                              MessageBoxButton.YesNoCancel);

                if (saveResult == MessageBoxResult.Yes)
                {
                    SaveGameResult(); // Метод для сохранения результата
                }
                else if (saveResult == MessageBoxResult.No)
                {
                    this.Close(); // Закрываем приложение без сохранения
                }
                // Если выбрано Cancel, ничего не делаем и возвращаемся в игру или главное меню
            }
        }

        private void SaveGameResult()
        { // Открываем диалоговое окно для сохранения файла, чтобы пользователь мог выбрать, где сохранить результат
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Сохранить результат"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                // Получаем имя игрока из текстового поля
                string playerName = PlayerNameTextBox.Text; // Используем ввод из TextBox
                int score = game.Score.CurrentScore; // Используем правильное имя свойства                                                     
                string difficulty = GetSelectedDifficulty(); // Получаем уровень сложности

                results.SaveRecord(playerName, score, difficulty);

                // Optionally, inform the user that their results have been saved
                MessageBox.Show("Результат успешно сохранён!", "Сохранение", MessageBoxButton.OK);
            }
        }
        public void RefreshBoard()
        {
            GameCanvas.Children.Clear(); // Очистка Canvas перед отрисовкой
            DrawGrid(); // Рисование сетки

            // Отрисовка сегментов змейки
            foreach (var segment in game.Snake.Segments)
                segment.Draw(GameCanvas.Children);

            // Отрисовка еды
            game.Food.Draw(GameCanvas.Children);

            // Отрисовка препятствий 
            foreach (var obstacle in game.Obstacles)
                obstacle.Draw(GameCanvas.Children);
        }

        private void UpdateGame()
        {
            game.Update(this); // Передаем ссылку на главное окно в метод Update игры
        }

        private void DrawGrid()
        {
            int cellSize = 20; // Размер клетки

            for (int x = 0; x < GameCanvas.Width; x += cellSize)
            {
                Line verticalLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = GameCanvas.Height,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(verticalLine);
            }

            for (int y = 0; y < GameCanvas.Height; y += cellSize)
            {
                Line horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = GameCanvas.Width,
                    Y2 = y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1
                };
                GameCanvas.Children.Add(horizontalLine);
            }
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    game.Snake.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    game.Snake.ChangeDirection(Direction.Down);
                    break;
                case Key.Left:
                    game.Snake.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    game.Snake.ChangeDirection(Direction.Right);
                    break;
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var column1 = new DataGridViewColumn();
            //column1.HeaderText = "Игрок"; //текст в шапке
            //column1.Width = 100; //ширина колонки
            //column1.ReadOnly = true; //значение в этой колонке нельзя править
        }
    }
}
