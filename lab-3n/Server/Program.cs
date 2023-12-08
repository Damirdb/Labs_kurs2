﻿using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;

public struct Structure
{
    public double a;
    public double b;
    public double result;
}

class PipeServer
{
    private static PriorityQueue<Structure, int> dataQueue = new PriorityQueue<Structure, int>();
    private static Mutex mutex = new Mutex();
    private static Mutex mutFile = new Mutex();
    private static int count = 0;
    private static string path = "E:\\lab-3n\\Client\\bin\\Debug\\net7.0\\Client.exe";

    private static async Task Main()
    {
        dataQueue.Enqueue(new Structure()
        {
            a = double.MaxValue - 1,
            b = double.MinValue + 1
        }, 1);

        dataQueue.Enqueue(new Structure()
        {
            a = double.MaxValue - 1,
            b = double.MinValue + 1
        }, 1);

        dataQueue.Enqueue(new Structure()
        {
            a = double.MaxValue - 1,
            b = double.MinValue + 1
        }, 1);

        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token = source.Token;

        StreamWriter file = new StreamWriter("output.txt", true);
        string str = string.Empty;
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            source.Cancel();
        };

        try
        {
            await Task.WhenAll(SenderTask(token), ReceiverTask(token)); ;
        }
        catch { }
        finally { file.Close(); }

        Task SenderTask(CancellationToken token)
        {
            return Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    double _n, _m;
                    int _priority;
                    Console.Write("Введите a, b, приоритет: \n");
                    string? res = Console.ReadLine();
                    if (res == null)
                    {
                        Console.WriteLine("Некорректный ввод");

                    }
                    else
                    {
                        string[] parse = res.Split(' ');
                        if (parse.Length == 2)
                        {
                            _n = Convert.ToDouble(parse[0]);
                            _m = Convert.ToDouble(parse[1]);
                            _priority = 0;
                        }
                        else
                        {
                            _n = Convert.ToDouble(parse[0]);
                            _m = Convert.ToDouble(parse[1]);
                            _priority = int.Parse(parse[2]);
                        }
                        Structure data = new Structure
                        {
                            a = _n,
                            b = _m,
                        };
                        mutex.WaitOne();
                        dataQueue.Enqueue(data, _priority);
                        mutex.ReleaseMutex();
                    }
                }
            });
        }

        async Task ReceiverTask(CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                Structure st;
                int pr;
                while (dataQueue.Count > 0)
                {
                    mutex.WaitOne();
                    bool flag = dataQueue.TryDequeue(out st, out pr);
                    mutex.ReleaseMutex();
                    if (flag)
                    {
                        await ClientConnect(st, token, pr);
                    }
                }
            }
        }

        async Task ClientConnect(Structure st, CancellationToken token, int pr)
        {
            try
            {
                byte[] dataBytes = new byte[Unsafe.SizeOf<Structure>()];
                Unsafe.As<byte, Structure>(ref dataBytes[0]) = st;
                NamedPipeServerStream pipeServer = new($"channel{count}", PipeDirection.InOut);
                // Console.WriteLine("Ждём подключения клиента...");
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = path;
                myProcess.StartInfo.Arguments = $"channel{count}";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                await pipeServer.WaitForConnectionAsync();
                // Console.WriteLine("Клиент подключился!");
                await pipeServer.WriteAsync(dataBytes, 0, dataBytes.Length);
                byte[] receivedBytes = new byte[Unsafe.SizeOf<Structure>()];
                if (await pipeServer.ReadAsync(receivedBytes, 0, receivedBytes.Length) == receivedBytes.Length)
                {
                    st = Unsafe.As<byte, Structure>(ref receivedBytes[0]);
                }

                mutFile.WaitOne();
                file.WriteLine($"a = {st.a}; b = {st.b}; priority = {pr}; result = {st.result}");
                mutFile.ReleaseMutex();
                pipeServer.Close();
                count++;
                await myProcess.WaitForExitAsync(token);
            }
            catch (Exception) { }
        }
    }
}