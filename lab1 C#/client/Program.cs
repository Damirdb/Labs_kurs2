using System;
using System.IO.Pipes;
using System.Text;

public struct MyData
{
    public int Number;
    public string Message;
}

class ClientProgram
{
    static void Main()
    {
        using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "MyPipe", PipeDirection.InOut))
        {
            Console.WriteLine("Подключение к серверу...");
            pipeClient.Connect();

            MyData sendData;
            sendData.Number = 123;
            sendData.Message = "Запрос от клиента";

            // Отправляем данные на сервер
            string message = $"{sendData.Number}|{sendData.Message}";
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            pipeClient.Write(buffer, 0, buffer.Length);
            Console.WriteLine($"Отправлено на сервер: {message}");

            // Читаем ответ от сервера
            byte[] responseBuffer = new byte[1024]; // Размер буфера должен быть достаточным для ответа сервера
            int bytesRead = pipeClient.Read(responseBuffer, 0, responseBuffer.Length);
            string responseMessage = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);

            string[] responseParts = responseMessage.Split('|');
            MyData receivedData;
            receivedData.Number = int.Parse(responseParts[0]);
            receivedData.Message = responseParts[1];

            Console.WriteLine($"Получен ответ от сервера: Number={receivedData.Number}, Message={receivedData.Message}");
        }
    }
}