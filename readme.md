# Traction

Traction is a plugin for [StackExchange.Precompilation](https://github.com/StackExchange/StackExchange.Precompilation) that enables light-weight design-by-contract in C#.  StackExchange.Precompilation is a NuGet package for .NET which uses the [Roslyn SDK](https://github.com/dotnet/roslyn) to allow custom processes to be added to the compilation pipeline.  

Traction is exposed to consuming code as custom attributes which can target parameters, method return values, or properties.  During compilation, Traction will perform a transformation on the in-memory syntax trees to insert runtime assertions wherever these attributes are used.  

## Example

`NonNullAttribute` can be applied to a members to signify that they cannot accept `null` values.  A method declaration like this:

    public void Write([NonNull] string text){
	    //Do stuff
	}  
	
will be transformed into this:

    public void Write([NonNull] string text){
	    if (Object.Equals(text, null)) throw new 
		    ArgumentNullException(nameof(text));
			
	    //Do stuff
	}
	
A similar transformation will happen for marked return values. This declaration:

    [return: NonNull]
    public string Read(){
		//Do stuff
	    return "abc";
	} 
	
will be transformed into this:

    [return: NonNull]
    public string Read(){
	    //Do stuff
		{
		    var result = "abc";
			if (Object.Equals(result, null))
			    throw new ReturnValueExcpetion("Return value cannot be null.");
			return result;
		}
	}
	
An extra `{}` block is created here to ensure no naming conflict with the local variable `result`.

The transformations for properties are similar, with the precondition being inserted into the `set` accessor and postcondition in the `get` accessor.

## Compatibility

Traction can only be used with C#6 and newer; it does not support VB or other languages.  This limitation is due to several factors:
 1. Traction's implementation consists of C# syntax transformations, not IL manipulation like other aspect-oriented .NET tools (PostSharp, Fody, Code Contracts).
 2. StackExchange.Precompilation uses `csc.exe` internally, and so cannot be used with VB or other languages.
 3. The Roslyn SDK only supports C#6/.NET4.6 and newer.