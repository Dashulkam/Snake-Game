using System.Windows;
using System.Windows.Controls;

namespace SnakeGame
{
    public class GameObject
    {
        public Point Position { get; protected set; } // Позиция объекта

        public GameObject(double x, double y)
        {
            Position = new Point(x, y);
        }

        // Метод для отрисовки объекта (можно переопределить в производных классах)
        public virtual void Draw(UIElementCollection canvasChildren)
        {
            
        }
    }
}
