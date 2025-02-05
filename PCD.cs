using System.Buffers.Binary;

namespace WC4GameFlagChecker;

public class PCD(byte[] Data)
{
    public byte[] Data { get; init; } = Data;

    private readonly ushort GamesFlagOffset = 0x014C;

    private const ushort DiamondConstant = 0x0004;
    private const ushort PearlConstant = 0x0008;
    private const ushort PlatinumConstant = 0x0010;
    private const ushort HeartGoldConstant = 0x8000;
    private const ushort SoulSilverConstant = 0x0001;

    private ushort GameCompatibility
    {
        get => BinaryPrimitives.ReadUInt16BigEndian(Data.AsSpan(GamesFlagOffset));
        set => BinaryPrimitives.WriteUInt16BigEndian(Data.AsSpan(GamesFlagOffset), value);
    }

    public bool Diamond => GameCompatibility.HasFlag(DiamondConstant);
    public bool Pearl => GameCompatibility.HasFlag(PearlConstant);
    public bool Platinum => GameCompatibility.HasFlag(PlatinumConstant);
    public bool HeartGold => GameCompatibility.HasFlag(HeartGoldConstant);
    public bool SoulSilver => GameCompatibility.HasFlag(SoulSilverConstant);
}