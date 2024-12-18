using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    public class Food : GameObject
    {
        public Food(double x, double y) : base(x, y) { }

        public void GenerateNewPosition(int boardWidth, int boardHeight, List<Segment> snakeSegments, List<Obstacle> obstacles)
        {
            Random random = new Random();
            Point newPosition;

            do
            {
                newPosition = new Point(random.Next(0, boardWidth / 20) * 20,
                                        random.Next(0, boardHeight / 20) * 20);
            }
            while (IsPositionOccupied(newPosition, snakeSegments, obstacles));

            Position = newPosition; // Устанавливаем новую позицию еды
        }

        private bool IsPositionOccupied(Point position, List<Segment> snakeSegments, List<Obstacle> obstacles)
        {
            // Проверка на совпадение с сегментами змейки
            foreach (var segment in snakeSegments)
            {
                if (position == segment.Position)
                    return true;
            }

            // Проверка на совпадение с препятствиями
            foreach (var obstacle in obstacles)
            {
                if (position == obstacle.Position)
                    return true;
            }

            return false; // Позиция свободна
        }

        public override void Draw(UIElementCollection canvasChildren)
        {
            // Создание круга для еды
            var ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Red // Цвет еды
            };

            // Установка позиции еды на канвасе
            Canvas.SetLeft(ellipse, Position.X);
            Canvas.SetTop(ellipse, Position.Y);

            // Добавление еды на канвас
            canvasChildren.Add(ellipse);
        }
    }
}


