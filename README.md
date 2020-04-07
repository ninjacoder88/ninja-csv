# Introduction
NinjaCsv is a library which allows parsing CSV (or a delimiter of your choice) files into a object. The project is built in .NET Standard 2.0.

# Contribute

# How to Use
1. Add a reference

Add a reference to the nuget package NinjaCsv

2. Setup DTO (data transfer object)

- Create a class to server as the DTO
- Add the `Column` attribute to the properties that you want read from the CSV file.
- **Column construtor accepts an integer which represents the column in the CSV file. This value is ZERO based**.
- Properties to be mapped MUST have a setter, either public or private.
- At least one property MUST be mapped but not all properties have to be mapped.

```
public class Item
{
	[Column(0)]
	public int Id {get;set;}
	
	[Column(1)]
	public string Name {get; private set;}
	
	public string Description {get;}
}
```

3. Call the API

Create an instance of `CsvParser`

`var parser = new CsvParser();`

Call the `Parse` method where the generic parameter is the DTO, passing in the file path
`parser.Parse<Item>(@"C:\employees.csv");`

The `Parse` method also accepts an optional parameter of `CsvParserOptions`. The two options currently available are `ContainsHeaderRow` which defaults to _true_ and `Delimiter` which defaults to comma (,).