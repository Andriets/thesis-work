using TicTacTec.TA.Library;

namespace AutoTrader.Application
{
    public class TALibTest
    {
        public double[] Test()
        {
            double[] closePrices = new double[] 
            {
                28395, 28320, 28713, 29669, 29909, 
                29992, 33069, 33922, 34496, 34151,
                33892, 34081, 34525, 34476, 34639,
                35421, 34941, 34716, 35062, 35011,
                35046, 35399, 35624, 36701, 37301,
                37130, 37064, 36462, 35551, 37858,
                36163, 36613, 36568, 37359, 37448,
                35741,
            };
            int optInFastPeriod = 12;
            int optInSlowPeriod = 26;
            int optInSignalPeriod = 9;

            int outBegIdx;
            int outNbElement;
            double[] outMACD = new double[closePrices.Length];
            double[] outMACDSignal = new double[closePrices.Length];
            double[] outMACDHist = new double[closePrices.Length];

            var res = Core.Macd(0, closePrices.Length - 1, 
                closePrices, optInFastPeriod, 
                optInSlowPeriod, optInSignalPeriod,
                out outBegIdx, out outNbElement,
                outMACD, outMACDSignal, outMACDHist);

            return outMACD;
        }
    }
}
