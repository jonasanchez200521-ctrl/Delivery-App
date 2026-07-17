namespace Nami.API.Patterns
{
    public class CalculationService : ICalculate
    {
        private const decimal IvaRate = 0.15m; // IVA Ecuador 15%

        public decimal CalculateIVA(decimal subtotal) => Math.Round(subtotal * IvaRate, 2);
    }
}
