using OpenDCP.Framework.DataTypes.Equipment.Entities.SensorData;
using OpenDCP.Framework.Math.Constants;
using OpenDCP.Framework.Math.Interfaces;

namespace OpenDCP.Framework.Math.Adapter.Equipment
{
    public class MathSensor : IMathSensor<SensorDataDTO>
    {
        private readonly SensorDataDTO _dto;
        public MathSensor(SensorDataDTO dto) => _dto = dto;

        public string UniqueId => _dto.WebId;

        public InterpolationMode InterpolationMode => InterpolationMode.Linear;

        public List<IMathExpressionPoint<SensorDataDTO>> Points => _dto.DataPoints?
            .Select(dp => (IMathExpressionPoint<SensorDataDTO>)new ExpressionPoint(dp))
            .ToList() ?? new List<IMathExpressionPoint<SensorDataDTO>>();

        public static List<IMathSensor<SensorDataDTO>> ToMathData(IEnumerable<SensorDataDTO> rawData)
        {
            if (rawData == null) return new List<IMathSensor<SensorDataDTO>>();

            return rawData
                .Select(dto => (IMathSensor<SensorDataDTO>)new MathSensor(dto))
                .ToList();
        }
    }
}
