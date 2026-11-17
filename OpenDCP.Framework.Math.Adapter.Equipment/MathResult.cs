using OpenDCP.Framework.DataTypes.Equipment.Entities.SensorData;

namespace OpenDCP.Framework.Math.Adapter.Equipment
{
    public static class MathResult
    {
        public static List<ValueDTO> ToValueDTOs(this IEnumerable<(DateTime Timestamp, dynamic Value, bool Good)> tupleResults)
        {
            if (tupleResults == null)
            {
                return new List<ValueDTO>();
            }

            return tupleResults.Select(point => new ValueDTO
            {
                ObsId = point.Timestamp,
                Value = point.Value,
                Good = point.Good
            }).ToList();
        }
    }
}
