using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class VehicleClassification
    {
        public VehicleClassification(int classNumber, int serialNumber)
        {
            ClassNumber = classNumber;
            SerialNumber = serialNumber;
        }

        [MaxLength(4)]
        public int ClassNumber { get; private set; }

        [MaxLength(4)]
        public int SerialNumber { get; private set; }

        public override bool Equals(object obj) => obj is VehicleClassification && obj.ToString() == ToString();

        public override int GetHashCode() => ClassNumber.GetHashCode() ^ SerialNumber.GetHashCode();

        public override string ToString() => $"{ClassNumber:D4}.{SerialNumber:D4}";
    }

}
