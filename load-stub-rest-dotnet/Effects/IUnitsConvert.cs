using Load.Stub.Rest.dotNet.Model;
using System.Text.RegularExpressions;

namespace Load.Stub.Rest.dotNet.Effects
{
    public interface IUnitsConvert
    {

    }

    public interface IUnitConverter<T> : IUnitsConvert
    {
        T Convert(string unit,double value);
    }

    public class NullConverter : IUnitConverter<double>
    {
        public double Convert(string unit,double value)
        {
            return value;
        }
    }

    public class TimeUnitConverter : IUnitConverter<TimeSpan>
    {
        private double Multiplier(string unit)
        {
            switch (unit)
            {
                case "ms":
                    return TimeSpan.FromMilliseconds(1).TotalMilliseconds;
                case "s":
                    return TimeSpan.FromSeconds(1).TotalMilliseconds;
                case "m":
                    return TimeSpan.FromMinutes(1).TotalMilliseconds;
                default:
                    throw new InvalidOperationException($"Invalid operation unit type: {unit} for {nameof(TimeUnitConverter)}");
            }
        }


        public TimeSpan Convert(string unit,double value)
        {

            return TimeSpan.FromMilliseconds(value * Multiplier(unit));
        }
    }
    public class BytesUnitConverter : IUnitConverter<long>
    {

        private double Multiplier(string unit)
        {
            switch (unit)
            {
                case "b":  return 1;
                case "kb": return 1024;
                case "mb": return 1024^2;
                case "gb": return 1024^3;
                default:
                    throw new InvalidOperationException($"Invalid operation unit type: {unit} for {nameof(BytesUnitConverter)}");
            }
        }

        public long Convert(string unit, double value)
        {

            return (long)(value * Multiplier(unit));
        }
    }


}