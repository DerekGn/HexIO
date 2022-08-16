# HexIO

[![Build Status](https://dev.azure.com/DerekGn/GitHub/_apis/build/status/DerekGn.HexIO?branchName=master)](https://dev.azure.com/DerekGn/GitHub/_build/latest?definitionId=5&branchName=master)

[![NuGet Badge](https://buildstats.info/nuget/HexIO)](https://www.nuget.org/packages/HexIO/)

A library for reading and writing hex format streams. Initial release supports intel hex formatted streams.

## Installing HexIO

Install the HexIO package via nuget package manager console:

```
Install-Package HexIO
```

## Whats new in Version 3.0

The core HexIO classes where refactored to be derived from the System.IO.StreamReader and System.IO.StreamWriter classes. This is a more natural implementation with the .net framework that the previous class implementations. Note that this is a breaking change that will require refactoring of code that previously used version 2.0.

An additional class, the IntelHexStreamTransformer, has also been implemented to allow modification of an existing intel hex file.

## Supported .Net Runtimes

The HexIO package is compatible with the following runtimes:

* .NET Standard 2.0
* .NET Framework 4.8

## Using the IntelHexStreamReader

The following demonstrates the use of the IntelHexStreamReader class.

Given the contents of the file sample.hex as the following snippet of intel hex record data.

```
:100C6000C0011124B7018D5D984F05C0BF01645FBD
:100C70007F4F84E097E00E94C500E0914707F09124
:100C800048079E898689891748F481E00115110576
:100C900019F0F801808302C08093FD06DF91CF91A7
:100CA0001F910F91FF90EF90089584B78E7F84BFBE
:100CB0000FB6F894A89580916000886180936000D9
:100CC000109260000FBE809164008FEF80936400EB
:100CD000809165008D638093650080916400877FBB
:100CE000809364000E947504FFCF991B79E004C0D3
:100CF000991F961708F0961B881F7A95C9F780955B
:060D00000895F894FFCFF6
:060D060049444C450000C9
:020000023000CC
:00000001FF
```

```csharp
public void Execute(IIntelHexStreamReader hexStreamReader)
{
    if(hexStreamReader == null)
    {
        throw new ArgumentNullException(nameof(hexStreamReader));
    }

    do
    {
        IntelHexRecord intelHexRecord = hexStreamReader.ReadHexRecord();

        Console.WriteLine(intelHexRecord);

        if (intelHexRecord.RecordType != IntelHexRecordType.Data && intelHexRecord.RecordType != IntelHexRecordType.EndOfFile)
        {
            Console.WriteLine(hexStreamReader.State);
        }

    } while (!hexStreamReader.State.Eof);
}
```

Will produce the following output:

```txt
RecordType: Data RecordLength: 0x10 Offset: 0x0C60 Data: C0-01-11-24-B7-01-8D-5D-98-4F-05-C0-BF-01-64-5F CheckSum: 0xBD
RecordType: Data RecordLength: 0x10 Offset: 0x0C70 Data: 7F-4F-84-E0-97-E0-0E-94-C5-00-E0-91-47-07-F0-91 CheckSum: 0x24
RecordType: Data RecordLength: 0x10 Offset: 0x0C80 Data: 48-07-9E-89-86-89-89-17-48-F4-81-E0-01-15-11-05 CheckSum: 0x76
RecordType: Data RecordLength: 0x10 Offset: 0x0C90 Data: 19-F0-F8-01-80-83-02-C0-80-93-FD-06-DF-91-CF-91 CheckSum: 0xA7
RecordType: Data RecordLength: 0x10 Offset: 0x0CA0 Data: 1F-91-0F-91-FF-90-EF-90-08-95-84-B7-8E-7F-84-BF CheckSum: 0xBE
RecordType: Data RecordLength: 0x10 Offset: 0x0CB0 Data: 0F-B6-F8-94-A8-95-80-91-60-00-88-61-80-93-60-00 CheckSum: 0xD9
RecordType: Data RecordLength: 0x10 Offset: 0x0CC0 Data: 10-92-60-00-0F-BE-80-91-64-00-8F-EF-80-93-64-00 CheckSum: 0xEB
RecordType: Data RecordLength: 0x10 Offset: 0x0CD0 Data: 80-91-65-00-8D-63-80-93-65-00-80-91-64-00-87-7F CheckSum: 0xBB
RecordType: Data RecordLength: 0x10 Offset: 0x0CE0 Data: 80-93-64-00-0E-94-75-04-FF-CF-99-1B-79-E0-04-C0 CheckSum: 0xD3
RecordType: Data RecordLength: 0x10 Offset: 0x0CF0 Data: 99-1F-96-17-08-F0-96-1B-88-1F-7A-95-C9-F7-80-95 CheckSum: 0x5B
RecordType: Data RecordLength: 0x06 Offset: 0x0D00 Data: 08-95-F8-94-FF-CF CheckSum: 0xF6
RecordType: Data RecordLength: 0x06 Offset: 0x0D06 Data: 49-44-4C-45-00-00 CheckSum: 0xC9
RecordType: ExtendedSegmentAddress RecordLength: 0x02 Offset: 0x0000 Data: 30-00 CheckSum: 0xCC
ExtendedInstructionPointer: 0x00000000 UpperLinearBaseAddress: 0x0000 UpperSegmentBaseAddress: 0x3000 SegmentAddress: CodeSegment: 0x0000 InstructionPointer: 0x0000
RecordType: EndOfFile RecordLength: 0x00 Offset: 0x0000 Data:  CheckSum: 0xFF
```

## Using the IntelHexStreamWriter

The following demonstrates the use of the IntelHexStreamWriter class.

```csharp
public void Execute(IIntelHexStreamWriter writer, MemoryStream stream)
{
    if (writer == null)
    {
        throw new ArgumentNullException(nameof(writer));
    }

    if (stream is null)
    {
        throw new ArgumentNullException(nameof(stream));
    }

    writer.WriteDataRecord(0, new List<byte>() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });
    writer.WriteDataRecord(0x0B, new List<byte>() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });

    writer.WriteExtendedLinearAddressRecord(0x1000);
    writer.WriteExtendedSegmentAddressRecord(0x2000);
    writer.WriteStartLinearAddressRecord(0x3000);
    writer.WriteStartSegmentAddressRecord(0x4000, 0x5000);

    writer.Close();

    stream.Flush();
    stream.Position = 0;

    var readStream = new MemoryStream(stream.ToArray());

    using StreamReader reader = new StreamReader(readStream);
    Console.WriteLine(reader.ReadToEnd());
}
```

The following hex records are written to the file output.hex.

```txt
:0A0000000102030405060708090ABF
:0A000B000102030405060708090AB4
:020000041000EA
:020000022000DC
:0400000500003000C7
:040000034000500069
:00000001FF
```

## Using the IntelHexStreamTransformer

The following demonstrates the use of the IntelHexStreamTransformer class. This class applies a set of transformations to records that are matched via a match specified by the Transform.

```csharp
public void Execute(IIntelHexStreamTransformer transformer)
{
    if (transformer is null)
    {
        throw new ArgumentNullException(nameof(transformer));
    }
    
    var transformedFile = transformer.ApplyTransforms(
        "transform.hex",
        new List<Transform>()
        {
            new InsertTransform(
            new Matching.IntelHexRecordMatch()
                {
                    RecordType = IntelHexRecordType.EndOfFile
                },
                InsertPosition.Before,
                new IntelHexRecord(0, IntelHexRecordType.ExtendedLinearAddress, new List<byte>() { 0xFE, 0xED })),
            new InsertTransform(
                new Matching.IntelHexRecordMatch()
                {
                    RecordType = IntelHexRecordType.EndOfFile
                },
                InsertPosition.Before,
                new IntelHexRecord(0, IntelHexRecordType.Data, new List<byte>() { 0xBE, 0xEF }))
        });

    using var reader = new StreamReader(transformedFile);

    Console.WriteLine(reader.ReadToEnd());
}
```

The following hex records are written to the file transform.hex.transformed. The original file is not modified.

```txt
:02000004FEED0F
:02000000BEEF51
:00000001FF
```
