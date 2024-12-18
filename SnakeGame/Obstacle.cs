using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    public class Obstacle : GameObject
    {
        public Obstacle(double x, double y) : base(x, y) { }

        public override void Draw(UIElementCollection canvasChildren)
        {
            // Создание прямоугольника для препятствия
            var rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Gray // Цвет препятствия
            };

            // Установка позиции препятствия на канвасе
            Canvas.SetLeft(rect, Position.X);
            Canvas.SetTop(rect, Position.Y);

            // Добавление препятствия на канвас
            canvasChildren.Add(rect);
        }
    }
}


