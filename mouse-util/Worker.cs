using PInvoke;
using System.Runtime.InteropServices;

namespace mouse_util;

public class Worker : BackgroundService {
    private readonly ILogger<Worker> _logger;
    private int _multiplier = -1;

    public Worker(ILogger<Worker> logger) {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            try {
                DoStuff();
                _multiplier *= -1;
                await Task.Delay(5000, stoppingToken);
            }
            catch (Exception ex) {
                _logger.LogError(ex.ToString());
            }
        }
    }

    private void DoStuff() {
        var input = new User32.INPUT() {
            type = User32.InputType.INPUT_MOUSE,
            Inputs = new User32.INPUT.InputUnion() {
                mi = new User32.MOUSEINPUT {
                    dx = 5 * _multiplier,
                    dy = 5 * _multiplier,
                    mouseData = 0,
                    dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_MOVE,
                    time = 0,
                    dwExtraInfo_IntPtr = IntPtr.Zero
                }
            }
        };

        var retVal = User32.SendInput(nInputs: 1, pInputs: new[] { input }, cbSize: Marshal.SizeOf<User32.INPUT>());
        if (retVal != 1) {
            var errorCode = Marshal.GetLastWin32Error();
            Console.WriteLine($"{retVal} | 0x{errorCode:x8}");
        }
    }
}