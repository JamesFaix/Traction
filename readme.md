# Traction

## Contents
 - [Overview](https://github.com/JamesFaix/Traction/blob/master/readme.md#L12)
 - [Abbreviations](https://github.com/JamesFaix/Traction/blob/master/readme.md#L30)
 - [Design by Contract](https://github.com/JamesFaix/Traction/blob/master/readme.md#L36)
 - [Why Traction?](https://github.com/JamesFaix/Traction/blob/master/readme.md#L45)
 - [Compatibility](https://github.com/JamesFaix/Traction/blob/master/readme.md#L106)
 - [Features](https://github.com/JamesFaix/Traction/blob/master/readme.md#L128)
 - [Contracts](https://github.com/JamesFaix/Traction/blob/master/readme.md#L193)

## Overview

Traction is a C# precompiler that implements a lightweight form of the design-by-contract pattern.
	
Traction is a plugin for the StackExchange.Precompilation package (SEP), which must be installed in a project before using Traction.  SEP is a build process tool that uses the .NET Compiler Platform (Roslyn) to allow the injection of custom compilation steps for C# and ASP.NET projects.  Traction adds a compilation step that scans the in-memory syntax trees for custom "contract" attributes, and injects corresponding runtime assertions.  For example, this source code:

	void ExampleMethod([NonNull] string text) { 
		//Processing...
	}

would be transformed into this in the compiled assembly:

	void ExampleMethod([NonNull] string text) {
		if (Object.Equals(text, null)) 
			throw new PreconditionException(nameof(text));
		//Processing...
	}
	
## Abbreviations
 - CC = Code Contracts
 - DbC = Design-by-contract
 - SEP = StackExchange.Precompilation
 - VS = Visual Studio

## Design by Contract

DbC is a pattern for ensuring software correctness.  It is generally used with object-oriented languages; the advanced type systems in some functional languages mitigates the need for DbC.
	
DbC emphasizes the definition of formal, testable interface specifications for components, which extend the ordinary concept of abstract data types.  In addition to specifying the types of members, other, more semantically rich conditions may be enforced on members.  These conditions are referred to as "contracts", and they come in three varieties:
 - Preconditions - Conditions that must hold before a method is executed.
 - Postconditions - Conditions that must hold before a method returns control to the calling method.
 - Object invariants (or just "invariants") - Conditions that must hold at all times for a given object.  These are usually tested before or after every method that is called on the object.

## Why Traction?

DbC is implemented as a first-class feature in some obscure .NET languages (Spec#, Eiffel.NET), but is not directly supported by any mainstream ones.  Although DbC has been a requested feature for several versions of C#, the C# design team has commented on GitHub and UserVoice that the cost in complexity of integrating DbC as a language feature is too great for the benefit it would provide.  Generally, it seems that Microsoft is no longer interested in DbC.  However, there are several tools available to use DbC in C#.

### Existing Tools

#### Code Contracts

CC is an open-source suite of tools for DbC in C# and VB.NET, which includes an API, assembly rewriter, and static analyzer.  It was originally created by Microsoft Research, but is now maintained by the open-source community.  Development has slowed since Microsoft pulled out, and there are still integration quirks with CC and the C#6 compiler.
			
CC is exposed to client code through several API methods for declaring contracts.  After normal compilation, the CC assembly rewriter can replace the contract declarations with runtime assertions, or remove them, depending on the build settings.  After rewriting, the static analyzer can be run to detect possible logical errors or invalid assumptions in the code.
			
The CC API has been included in the Base Class Libraries since .NET 4.0.  This means code using CC does not need to be distributed with any additional assemblies.  The assembly rewriter and static analyzer must be installed as a Visual Studio extension, and a secondary extension must be installed to view intellisense for declared contracts.
			
The API provides a thorough implementation of the DbC pattern, including precondition, postconditions, and invariants.  Contract conditions can be ad-hoc and of arbitrary complexity, although simpler is usually better.  Contracts on base classes are inherited by derived classes, and for compliance with the Liskov Substitution Principle, inherited members cannot have "stronger" preconditions than their base member.  There is also a mechanism for placing contracts on abstract or interface members.
			
Although CC can be configured to emit extra XML comments for declared contracts, a secondary VS extension is required to view these as intellisense.  CC contracts are not represented in assembly metadata, as attribute-based contracts would be, which means they cannot be queried by normal means like reflection.
			
#### PostSharp

PostSharp is a commercial VS extension and framework for aspect-oriented programming in C# or VB.  It comes with a basic implementation of DbC included, but can also be used for other tasks like simplifying logging or implementing standard design patterns without boilerplate code.
			
PostSharp is exposed to client code through custom attributes for declaring contracts or other behaviors.  After normal compliation, PostSharp rewrites the compiled assembly to inject runtime assertions (in the case of contracts).  
			
Aplications using PostSharp must be distributed with several PostSharp libraries, and a VS extension is required to use the rewriter.
			
The API provides a basic implementation of DbC, including preconditions and postconditions, but not object invariants.  New contract attributes can be created, with arbitrarily complicated logic, but ad-hoc contracts are not possible.  Contracts on base classes and inherited by derived classes, and contracts can be applied to abstract or interface members.
			
Since contracts are declared as attributes, they are represented in assembly metadata and can be queried with reflection.
			
Unfortunately, the trial version of PostSharp does not provide much functionality.

#### Fody *(honorable mention)*

Fody is an open-source framework for aspect-oriented programming in .NET.  It is distributed as a NuGet package and allows any number of assembly rewriter plugins to be added to the build process after normal compilation. 
			
There is currently no dedicated DbC plugin for it, but it would certainly be possible to implement.  Traction was originally going to use Fody before I discovered SEP.  One related plugin is NullGuard.Fody, which injects null checks for all reference-type parameters in an assembly, unless that parameter is marked with an AllowNullAttribute.
			
Fody is compatible with any .NET language, but implementation code must be written in IL.

### Traction

Differences between Traction and both CC and PostSharp:
 - No VS extension required; deployed as package
 - Only works for C#6 (or newer)
 - Internally uses Roslyn instead of IL rewriting, which makes maintenance and extension easier 
	
Differences between Traction and CC:
 - No static analyzer
 - No object invariants
 - Does not support ad-hoc or complex contracts
 - Exposed to client code as custom attributes, not methods
		
Differences between Traction and PostSharp:
 - Free & open-source
 - No libraries to distribute with client applications
 - Does not support general aspect-oriented programming

## Compatability

### Tools

Traction can only be used with C#6 (or newer).  It cannot be used with other languages because it is implemented as C# syntax transformations.
		
Traction was designed in Visual Studio 2015, but it may work for other IDE's that support C#6 and use MSBuild.

The Traction source solution also uses NUnit 3 for unit testing.

### Dependencies

Traction is dependent on two other packages:
 - Microsoft.CodeAnalysis.CSharp
 - StackExchange.Precompilation
			
Both of these packages include many dependencies that are not used by Traction.  Only the following libraries are actually required in client applications:
 - Microsoft.CodeAnalysis
 - Microsoft.CodeAnalysis.CSharp
 - StackExchange.Precompilation
 - System
 - System.Core
 - System.Collections.Immutable
 - System.Reflection.Metadata
	
## Features

### Contract Exceptions
All Traction contract attributes can be applied to parameters and return values of methods, operators, and conversions, as well as properties.  During compilation, runtime checks will be inserted into the marked members which will throw either a `Traction.PreconditionException` or `PostconditionException` if the contract condition is broken.  
 - Contracts on parameters will check the parameter value and throw a `PreconditionException` if the contract is broken.  
 - Contracts on return values will check all `return` statements and throw a `PostconditionException` if the contract is broken.
 - Contracts on properties with a `set` accessor will check the `value` and throw a `PreconditionException` if the contract is broken. 
 - Contracts on properties with a `get` accessor will check all `return` statements and throw a `PostconditionException` if the contract is broken.
 - (Contracts on properties with both a `get` and `set` accessor with be applied to both accessors.)

#### Example

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

### Inheritance
 
	//Fill this out later
	
### Iterator blocks

	//Fill this out later
 
### Type Restrictions
Each contract attribute may have restrictions on the types of values or properties it can be applied to.  If a contract attribute is applied to a value of an invalid type, a compile error will be created and no code will be injected.  Currently there is no support for syntax highlighting for these errors.  Some contracts may also have slightly different behavior for different value types, which should be reflected in the documentation.  

### PDB and XML files

	//Fill this out later
	
# Contracts

`T` will be used for the type of the marked value or property.

##Default value contracts
 - `NonNullAttribute` 
   - Reference types only. (Nullable value types are not allowed, because if null is invalid, the type should just be changed to its non-nullable counterpart.)
   - Throws an exception if the value is `null`. 
 - `NonDefaultAttribute` 
   - Any type.
   - Throws an exception if the value is `default(T)`.  
   - For nullable value types, checks against the default for the underlying type, not the nullable type.  (This gives `NonNull` and `NonDefault` distinct uses for nullable value types.)
 - `NonEmptyAttribute` 
   - Types implementing `IEnumerable<T>`
   - Throws an exception if the value has 0 elements.
   - For reference types, also throws if value is `null`.
   - Can be used with `String`, since it implements `IEnumerable<char>`.
   - **Note**, this will force at least partial iteration of a lazy sequence.

##Basic `IComparable<T>` contracts
 - `PositiveAttribute` - Throws an exception if the value is `<= default(T)`.
 - `NegativeAttribute` - Throws an exception if the value is `>= default(T)`.
 - `NonPositiveAttribute` - Throws an exception if the value is `> default(T)`.
 - `NonNegativeAttribute` - Throws an exception if the value is `< default(T)`.
 - Value types implementing `IComparable<T>`.
 - These cannot be used for reference types because `null` should be the "lowest" value for any implementation of `IComparable<T>`.  (If reference types were supported, for correct `IComparable<T>` implementations, these contracts would either always fail, always pass, or effectively become `NonNull`.)
 - For nullable value types, each checks against the default for the underlying type, not the nullable type.
 - These contracts all actually use the `IComparable<T>.CompareTo()` method for comparison, but I will use the `<` and `>` operators here to make the documentation clearer.    


# References

	//Fill out later
