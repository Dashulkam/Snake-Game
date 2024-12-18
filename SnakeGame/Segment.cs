using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    public class Segment : GameObject
    {
        public Segment(double x, double y) : base(x, y) { }

        public override void Draw(UIElementCollection canvasChildren)
        {
            // Создание прямоугольника для сегмента змейки
            var rect = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green // Цвет сегмента змейки
            };

            // Установка позиции сегмента на канвасе
            Canvas.SetLeft(rect, Position.X);
            Canvas.SetTop(rect, Position.Y);

            // Добавление сегмента на канвас
            canvasChildren.Add(rect);
        }
    }
}
