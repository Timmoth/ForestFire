using Aptacode.BlazorCanvas;
using Microsoft.AspNetCore.Components;

namespace Timmoth
{
    public class ForestFireBase : ComponentBase, IDisposable
    {
        protected BlazorCanvas Canvas { get; set; } = default!;

        [Parameter, EditorRequired]
        public int Width { get; set; }

        [Parameter, EditorRequired]
        public int Height { get; set; }
        [Parameter, EditorRequired]
        public RenderFragment ChildContent { get; set; } = default!;

        [Parameter] public int CellSize { get; set; } = 8;
        [Parameter] public int Delay { get; set; } = 50;

        private ForestFireSimulation ForestFire = default!;
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private bool _reset = false;

        protected override async Task OnInitializedAsync()
        {
            ForestFire = new ForestFireSimulation(Width / CellSize, Height / CellSize);
            
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(10 + Delay, _cancellationTokenSource.Token);

                if (Canvas is not { Ready: true })
                {
                    continue;
                }

                if (_reset)
                {
                    _reset = false;
                    ForestFire.Reset();
                }

                ForestFire.Next();
                Draw(Canvas);
            }
        }
        private void Draw(BlazorCanvas canvas)
        {
            Canvas.ClearRect(0, 0, Width, Height);

            // clear
            canvas.LineWidth(1);
            canvas.StrokeStyle("white");

            // Update Scene
            for (var i = 0; i < ForestFire.Width; i++)
            {
                for (var j = 0; j < ForestFire.Height; j++)
                {
                    var age = ForestFire.Cells[i, j];
                    switch (age)
                    {
                        case 0:
                            canvas.FillStyle("#e4ffcc");
                            break;
                        case 1:
                            canvas.FillStyle("#aded74");
                            break;
                        case 2:
                            canvas.FillStyle("#85e332");
                            break;
                        case 3:
                            canvas.FillStyle("#61b814");
                            break;
                        case 4:
                            canvas.FillStyle("#b87114");
                            break;
                        case 5:
                            canvas.FillStyle("#b84014");
                            break;
                        default:
                            canvas.FillStyle("#b82414");
                            break;
                    }

                    canvas.FillRect(CellSize * i, CellSize * j, CellSize, CellSize);
                    canvas.StrokeRect(CellSize * i, CellSize * j, CellSize, CellSize);
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
        }

        public void Reset()
        {
            _reset = true;
        }
    }
}
