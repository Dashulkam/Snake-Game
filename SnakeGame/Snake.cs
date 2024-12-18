using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SnakeGame
{
    public class Snake : GameObject
    {
        public List<Segment> Segments { get; private set; } // Список сегментов змейки
        public Direction CurrentDirection { get; private set; } // Текущее направление движения

        // Конструктор класса Snake, который принимает начальные координаты
        public Snake(double x, double y) : base(x, y) // Вызываем конструктор базового класса
        {
            Segments = new List<Segment>
            {
                new Segment(x, y), // Начальная позиция головы змейки
                new Segment(x - 20, y), // Второй сегмент
                new Segment(x - 40, y)  // Третий сегмент
            };
            CurrentDirection = Direction.Right; // Начальное направление
        }

        public void Move(Game game)
        {
            var head = Segments[0]; // Получаем текущую голову змейки
            double newX = head.Position.X;
            double newY = head.Position.Y;

            switch (CurrentDirection)
            {
                case Direction.Up: newY -= 20; break;
                case Direction.Down: newY += 20; break;
                case Direction.Left: newX -= 20; break;
                case Direction.Right: newX += 20; break;
            }

            // Добавляем новый сегмент в начало и удаляем последний сегмент
            Segments.Insert(0, new Segment(newX, newY));

            // Удаляем последний сегмент только если не съели еду
            if (newX != game.Food.Position.X || newY != game.Food.Position.Y)
                Segments.RemoveAt(Segments.Count - 1);
        }

        public bool CollidesWithSelf()
        {
            var head = Segments[0];

            for (int i = 1; i < Segments.Count; i++)
            {
                if (head.Position == Segments[i].Position) // Проверка на столкновение с другими сегментами
                    return true;
            }

            return false;
        }

        public void ChangeDirection(Direction direction)
        {
            // Изменение направления движения змейки, предотвращая обратное движение
            if ((CurrentDirection == Direction.Up && direction != Direction.Down) ||
                (CurrentDirection == Direction.Down && direction != Direction.Up) ||
                (CurrentDirection == Direction.Left && direction != Direction.Right) ||
                (CurrentDirection == Direction.Right && direction != Direction.Left))
            {
                CurrentDirection = direction;
            }
        }

        public void Grow()
        {
            var tail = Segments[Segments.Count - 1];
            Segments.Add(new Segment(tail.Position.X, tail.Position.Y)); // Добавляем новый сегмент в конец
        }

        public override void Draw(UIElementCollection canvasChildren)
        {
            foreach (var segment in Segments)
            {
                segment.Draw(canvasChildren); // Передаем коллекцию детей канваса для отрисовки каждого сегмента
            }
        }
    }

    public enum Direction // Переносим перечисление за пределы класса Snake
    {
        Up,
        Down,
        Left,
        Right
    }
}

