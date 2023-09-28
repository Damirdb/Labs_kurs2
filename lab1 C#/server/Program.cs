using System;
using System.IO.Pipes;
using System.Text;

public struct MyData
{
    public int Number;
    public string Message;
}

class ServerProgram
{
    static void Main()
    {
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.InOut))
        {
            Console.WriteLine("Сервер запущен. Ожидание клиента...");
            pipeServer.WaitForConnection();

            MyData receivedData;
            byte[] buffer = new byte[1024]; // Размер буфера должен быть достаточным для передачи данных структуры

            int bytesRead = pipeServer.Read(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Сервер получил сообщение: {receivedMessage}");

            // Моделируем некоторую обработку данных на сервере
            receivedData.Number = 42;
            receivedData.Message = "Ответ от сервера";

            // Отправляем ответ клиенту
            byte[] responseBytes = Encoding.UTF8.GetBytes($"{receivedData.Number}|{receivedData.Message}");
            pipeServer.Write(responseBytes, 0, responseBytes.Length);

            Console.WriteLine("Ответ отправлен клиенту.");
        }
    }
}