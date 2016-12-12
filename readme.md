# Traction

Traction is a plugin for [StackExchange.Precompilation](https://github.com/StackExchange/StackExchange.Precompilation) that enables light-weight design-by-contract in C#.  StackExchange.Precompilation is a NuGet package for .NET which uses the [Roslyn SDK](https://github.com/dotnet/roslyn) to allow custom processes to be added to the compilation pipeline.  

Traction is exposed to consuming code as custom attributes which can target parameters, method return values, or properties.  During compilation, Traction will perform a transformation on the in-memory syntax trees to insert runtime assertions wherever these attributes are used.  

## Compatibility & Limitations

Traction can only be used with C#6 and newer; it does not support VB or other languages.  This limitation is due to several factors:
 1. Traction's implementation consists of C# syntax transformations, not IL manipulation like other aspect-oriented .NET tools (PostSharp, Fody, Microsoft Code Contracts).
 2. StackExchange.Precompilation uses `csc.exe` internally, and so cannot be used with VB or other languages.
 3. The Roslyn SDK only supports C#6/.NET4.6 and newer.

Traction does not directly support any static analysis tools, like Microsoft Code Contracts does; it just injects runtime assertions.  This is what "light-weight design-by-contract" refers to in the description.  It would not be impossible to use the Roslyn SDK to create analyzers that could perform a role similar to the Code Contracts static analyzer, but that it not a primary goal of Traction.  (At least not yet. If anyone wants take on the task, please do.)

## Contract Exceptions
All Traction contract attributes can be applied to parameters and return values of methods, operators, and conversions, as well as properties.  During compilation, runtime checks will be inserted into the marked members which will throw either a `Traction.PreconditionException` or `PostconditionException` if the contract condition is broken.  
 - Contracts on parameters will check the parameter value and throw a `PreconditionException` if the contract is broken.  
 - Contracts on return values will check all `return` statements and throw a `PostconditionException` if the contract is broken.
 - Contracts on properties with a `set` accessor will check the `value` and throw a `PreconditionException` if the contract is broken. 
 - Contracts on properties with a `get` accessor will check all `return` statements and throw a `PostconditionException` if the contract is broken.
 - (Contracts on properties with both a `get` and `set` accessor with be applied to both accessors.)

## Example

`NonNullAttribute` can be applied to a members to check for `null` values.  A method declaration like this:

    public void Write([NonNull] string text){
	    //Do stuff
	}  
	
will be transformed into this:

    public void Write([NonNull] string text){
	    if (Object.Equals(text, null)) throw new 
		    PreconditionException(nameof(text));
			
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
			    throw new PostconditionException("Return value cannot be null.");
			return result;
		}
	}
	
The transformations for properties are similar, with the precondition being inserted into the `set` accessor and postcondition in the `get` accessor.

## Tools
 - Roslyn SDK
 - StackExchange.Precompilation
 - Visual Studio 2015 (SE.Precompilation uses MSBuild internally)
 - ILSpy (Any .NET disassembler will do, but ILSpy is free and has a handy VS plugin.)
 - NUnit 3 for unit testing
 
## Contract Attributes
### Type Restrictions
Each contract attribute may have restrictions on the types of values or properties it can be applied to.  If a contract attribute is applied to a value of an invalid type, a compile error will be created and no code will be injected.  Currently there is no support for syntax highlighting for these errors.  Some contracts may also have slightly different behavior for different value types, which should be reflected in the documentation.  

`T` will be used for the type of the marked value or property.

###Default value contracts
 - `NonNullAttribute` 
   - Reference and nullable value types only
   - Throws an exception if the value is `null`. 
 - `NonDefaultAttribute` 
   - Any types
   - Throws an exception if the value is `default(T)`.  
   - For nullable value types, checks against the default for the underlying type, not the nullable type.  (This gives `NonNull` and `NonDefault` distinct uses for nullable value types.)
 - `NonEmptyAttribute` 
   - Types implementing `IEnumerable<T>`
   - Throws an exception if the value has 0 elements.
   - For reference types, also throws if value is `null`.
   - Can be used with `String`, since it implements `IEnumerable<char>`.

###Basic `IComparable<T>` contracts
 - `PositiveAttribute` - Throws an exception if the value is `<= default(T)`.
 - `NegativeAttribute` - Throws an exception if the value is `>= default(T)`.
 - `NonPositiveAttribute` - Throws an exception if the value is `> default(T)`.
 - `NonNegativeAttribute` - Throws an exception if the value is `< default(T)`.
 - Value types implementing `IComparable<T>`.
 - These cannot be used for reference types because `null` should be the "lowest" value for any implementation of `IComparable<T>`.  (If reference types were supported, for correct `IComparable<T>` implementations, these contracts would either always fail, always pass, or effectively become `NonNull`.)
 - For nullable value types, each checks against the default for the underlying type, not the nullable type.
 - These contracts all actually use the `IComparable<T>.CompareTo()` method for comparison, but I will use the `<` and `>` operators here to make the documentation clearer.    


