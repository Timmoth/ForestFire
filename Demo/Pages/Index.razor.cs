using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Components;
using Timmoth;

namespace Demo.Pages
{
    public partial class IndexPageBase : ComponentBase
    {
        protected int Width = 600;
        protected int Height = 600;
        public bool HasLoaded { get; set; }
        public int Frequency { get; set; } = 50;
        public int Delay { get; set; } = 50;
        public ForestFire ForestFireRef { get; set; } = default!;
        protected override async Task OnInitializedAsync()
        {
            await JSHost.ImportAsync("Index",
                "../Pages/Index.razor.js");

            Width = GetWidth();
            Height = GetHeight();
            HasLoaded = true;
            await InvokeAsync(StateHasChanged);
        }
        protected void Reset()
        {
            ForestFireRef.Reset();
        }

        [JSImport("GetWidth", "Index")]
        internal static partial int GetWidth();


        [JSImport("GetHeight", "Index")]
        internal static partial int GetHeight();
    }
}
