using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Year2021;

// https://adventofcode.com/2021/day/16
public class Day16
{
    private readonly ITestOutputHelper _output;
    public Day16(ITestOutputHelper output)
    {
        _output = output;
    }

    public class Packet
    {
        public byte Version { get; init; }
        public byte TypeId { get; init; }
    }

    public class PacketLiteral : Packet
    {
        public long LiteralValue { get; init; }
    }

    public class PacketOperation : Packet
    {
        public List<Packet> Operations { get; init; }
    }

    public class PacketBuilder
    {
        public Packet? ReadNext(IEnumerator<byte> enumerator, int maxCount, Action<int> remainingCb = null)
        {
            byte version = 0;
            version = (byte)(version | (byte)(enumerator.Current << 2)); enumerator.MoveNext();
            version = (byte)(version | (byte)(enumerator.Current << 1)); enumerator.MoveNext();
            version = (byte)(version | enumerator.Current); enumerator.MoveNext();

            byte typeId = 0;
            typeId = (byte)(typeId | (byte)(enumerator.Current << 2)); enumerator.MoveNext();
            typeId = (byte)(typeId | (byte)(enumerator.Current << 1)); enumerator.MoveNext();
            typeId = (byte)(typeId | enumerator.Current); enumerator.MoveNext();

            maxCount -= 6;

            if (typeId == 4)
            {
                long litvalue = ReadLiteralValue(enumerator);
                return new PacketLiteral
                {
                    Version = version,
                    TypeId = typeId,
                    LiteralValue = litvalue
                };
            }
            else
            {
                // operation type packet
                byte lengthTypeId = enumerator.Current; enumerator.MoveNext();
                int length = lengthTypeId == 0 ? ReadNumber(enumerator, 15) : ReadNumber(enumerator, 11);
                var operations = new List<Packet>();
                if (lengthTypeId == 1)
                {
                    // length represents number of subpackets contained by this operation
                    // number of subpackets
                    for (int i = 0; i < length; i++)
                    {
                        operations.Add(ReadNext(enumerator, 0));
                    }
                }
                else
                {
                    // length of bits in the subpackets (could be remainders)
                    int remainder = length;
                    var subPacket = ReadNext(enumerator, length, remaining => { remainder -= remaining; });
                }
            }

            return null;
        }

        private int ReadNumber(IEnumerator<byte> enumerator, int size)
        {
            int value = 0;
            for (int i = 0; i < size; i++)
            {
                value = value << 1 | enumerator.Current;
                enumerator.MoveNext();
            }
            return value;
        }

        private long ReadLiteralValue(IEnumerator<byte> enumerator)
        {
            long value = 0;
            bool keepReading = true;
            while (keepReading)
            {
                keepReading = enumerator.Current != 0; enumerator.MoveNext();
                value = value << 4;
                value |= (enumerator.Current << 3); enumerator.MoveNext();
                value |= (enumerator.Current << 2); enumerator.MoveNext();
                value |= (enumerator.Current << 1); enumerator.MoveNext();
                value |= enumerator.Current; enumerator.MoveNext();
            }
            return value;
        }

        private long ConvertToLiteral()
        {
            throw new System.NotImplementedException();
        }

        internal void Reset()
        {
            throw new System.NotImplementedException();
        }
    }

    //private IEnumerable<Packet> ReadPackets(IEnumerable<byte> bytes)
    //{
    //    //var enumerator = bytes.GetEnumerator();
    //    //var builder = new PacketBuilder();
    //    //while(enumerator.MoveNext())
    //    //{
    //    //    yield return builder.ReadNext(enumerator);
    //    //    builder.Reset();
    //    //}
    //}

    private IEnumerable<byte> ReadBits(string variant)
    {
        var reader = InputClient.GetFileStreamReader(2021, 16, variant);

        int nextInput;

        while((nextInput = reader.Read()) != -1)
        {
            var num = byte.Parse(new string(new char[] { (char)nextInput }), NumberStyles.HexNumber);
            yield return (byte)((0x8 & num) >> 3);
            yield return (byte)((0x4 & num) >> 2);
            yield return (byte)((0x2 & num) >> 1);
            yield return (byte)(0x1 & num);
        }
    }

    [Fact]
    public void PartA()
    {
        //var bits = ReadBits("Sample");
        //var packets = ReadPackets(bits);

        //Assert.Equal(2021, ((PacketLiteral)packets.First()).LiteralValue);
    }

    [Fact]
    public void PartB()
    {
        Assert.True(false);
    }
}
