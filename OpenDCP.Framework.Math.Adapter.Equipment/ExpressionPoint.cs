using OpenDCP.Framework.DataTypes.Equipment.Entities.SensorData;
using OpenDCP.Framework.Math.Interfaces;

namespace OpenDCP.Framework.Math.Adapter.Equipment
{
    public class ExpressionPoint : IMathExpressionPoint<SensorDataDTO>
    {
        private readonly ValueDTO _dto;
        public ExpressionPoint(ValueDTO dto) => _dto = dto;

        public DateTime Timestamp => _dto.ObsId;
        public bool Good => _dto.Good;
        public dynamic Value => _dto.Value;
    }
}
