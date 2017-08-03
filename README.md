# HexIO

A library for reading and writing hex format streams. Initial release supports intel hex formatted streams.

## Installing HexIO

Install the HexIO package via nuget package manager console:

```
Install-Package HexIO
```

## Supported .Net Runtimes

The HexIO package is compatible with the following runtimes:

* .NET Standard 1.6
* .NET Framework 4.5
* .NET Framework 4.6

## Using the IntelHexReader

The following demonstrates the use of the IntelHexReader class. 

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
:00000001FF
```

Using the following snippet of code, note ToHexString extension method not shown for simplicity.

```csharp
static void Main(string[] args)
{
	uint address;
	IList<byte> data;

	// This can be any type of System.IO.Stream derived stream
	using (var fileStream = new FileStream("sample.hex", FileMode.Open))
	{
		using (IntelHexReader hexReader = new IntelHexReader(fileStream))
		{
			while (hexReader.Read(out address, out data))
			{
				Console.WriteLine($"Address: [0x{address:X4}] Count: [0x{data.Count:X2}] Data: [{data.ToHexString()}]");
			}
		}
	}
}
```

Will produce the following output:

```
Address: [0x0C60] Count: [0x10] Data: [C0011124B7018D5D984F05C0BF01645F]
Address: [0x0C70] Count: [0x10] Data: [7F4F84E097E00E94C500E0914707F091]
Address: [0x0C80] Count: [0x10] Data: [48079E898689891748F481E001151105]
Address: [0x0C90] Count: [0x10] Data: [19F0F801808302C08093FD06DF91CF91]
Address: [0x0CA0] Count: [0x10] Data: [1F910F91FF90EF90089584B78E7F84BF]
Address: [0x0CB0] Count: [0x10] Data: [0FB6F894A89580916000886180936000]
Address: [0x0CC0] Count: [0x10] Data: [109260000FBE809164008FEF80936400]
Address: [0x0CD0] Count: [0x10] Data: [809165008D638093650080916400877F]
Address: [0x0CE0] Count: [0x10] Data: [809364000E947504FFCF991B79E004C0]
Address: [0x0CF0] Count: [0x10] Data: [991F961708F0961B881F7A95C9F78095]
Address: [0x0D00] Count: [0x06] Data: [0895F894FFCF]
Address: [0x0D06] Count: [0x06] Data: [49444C450000]
```

## Using the IntelHexWriter

The following demonstrates the use of the IntelHexWriter class.

```csharp
static void Main(string[] args)
{
	// This can be any type of System.IO.Stream derived stream
	using (var fileStream = new FileStream("output.hex", FileMode.OpenOrCreate))
	{
		using (IntelHexWriter hexWriter = new IntelHexWriter(fileStream))
		{
			hexWriter.WriteAddress(AddressType.ExtendedLinearAddress, 0x1000);
			hexWriter.WriteData(0, new List<byte>() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });
			hexWriter.WriteData(0x0B, new List<byte>() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });
		}
	}
}
```

The following hex records are written to the file output.hex.

```
:020000040000FA
:0A0000000102030405060708090ABF
:0A000B000102030405060708090AB4
:00000001FF
```
