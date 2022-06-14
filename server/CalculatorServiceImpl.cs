using Calculator;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using static Calculator.CalculatorService;

namespace server
{
    public class CalculatorServiceImpl : CalculatorServiceBase
    {
        public override async Task FindMaximun(IAsyncStreamReader<FindMaximunRequest> requestStream, IServerStreamWriter<FindMaximunReponse> responseStream, ServerCallContext context)
        {
            int? max = null;
            while (await requestStream.MoveNext())
            {
                var current = requestStream.Current.Value;
                Console.WriteLine($"Evaluating: {current}");
                if (max == null || max < current)
                {
                    max = current;
                    await responseStream.WriteAsync(new FindMaximunReponse { Result = max.Value });
                }
            }
        }
    }
}
