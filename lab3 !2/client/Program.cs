using System;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PipeClient
{
    public struct Data
    {
        public int Value;
        public bool Confirm;
    }

    class Program
    {
        static void Main()
        {
            try
            {
                Console.WriteLine("Соединение с сервером...");

                using (var pipeStream = new NamedPipeClientStream(".", "pipe", PipeDirection.InOut))
                {
                    pipeStream.Connect();

                    Console.WriteLine("Соединение установлено");

                    while (true)
                    {
                        // Получение данных от сервера
                        byte[] buffer = new byte[Unsafe.SizeOf<Data>()];
                        pipeStream.Read(buffer);

                        var data = MemoryMarshal.Read<Data>(buffer);
                        Console.WriteLine($"Получено: {data.Value}, {data.Confirm}");

                        // Выполнение приближенного расчета интеграла
                        double integralResult = PerformTrapezoidalIntegration();
                        Console.WriteLine($"Приближенное значение интеграла: {integralResult}");

                        data.Confirm = true;
                        Console.WriteLine($"Отправлено: {data.Value}, {data.Confirm}");

                        byte[] resBuffer = new byte[Unsafe.SizeOf<Data>()];
                        MemoryMarshal.Write(resBuffer, ref data);

                        // Отправка подтверждения серверу
                        pipeStream.Write(resBuffer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в клиенте: {ex.Message}");
            }
        }

        // Метод для приближенного расчета интеграла функции 2x^2 в интервале от -3 до 3 методом трапеций
        static double PerformTrapezoidalIntegration()
        {
            double a = -3;
            double b = 3;
            int n = 1000;

            double h = (b - a) / n;
            double result = (Function(a) + Function(b)) / 2.0;

            for (int i = 1; i < n; i++)
            {
                double x = a + i * h;
                result += Function(x);
            }

            return result * h;
        }

        // Функция 2x^2
        static double Function(double x)
        {
            return 2 * x * x;
        }
    }
}
