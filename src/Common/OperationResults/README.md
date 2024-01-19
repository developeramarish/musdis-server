
# Musdis.Results
The Musdis.Results package is a tool for developers working with C# who want to harness the full potential of result patterns in their code.

## Key Features:

- ### Simplified Error Handling
Result patterns allow you to handle errors and success cases in a clear and structured manner, reducing boilerplate code and improving code readability.

```c#
Result<string> GetData(string path)
{
    if (!File.Exists(path)) 
    {
        return new Error("File does not exist").ToValueResult<string>();
    }

    /* .... */
}
```

Instead of

```c#
string GetData(string path)
{
    if (!File.Exists(path)) 
    {
        throw new Exception("File does not exist");
    }

    /* .... */
}
```

- ### Pattern Matching 

```c#
var result = Result<string>.Success("Some value");

var str = result switch 
{
    // No dereference of a possibly null reference warnings.
    { IsSuccess: true } => $"Succeed with value {result.Value.ToString()}", 
    { IsSuccess: false } => $"Failed with error: {result.Error.ToString()}"
};
```