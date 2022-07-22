# GSL
<img src="img/logo.png" width=200 align="right" />
This repository contains the official interpreter for GSL (General Scripting Language).<br/>
First of all, this is the first interpreter I've ever made, so expect it to be slow and resource heavy.
If you can figure out how to make it better, make a pull request.<br/>

### Installation
#### Prerequisites
- dotnet SDK

#### Dependencies
Run `dotnet restore` in the project directory to install the dependencies.

#### Execution
Run `dotnet run <input file>` to build the project and run your input file. Of course, `<input file>` should be replaced with the name of the GSL file you want to run.

### Simplicity with understanding
GSL is designed as a simple language, which is supposed to make complicated tasks easier without losing the knowledge of what the programmer is doing.<br/>

Of course, a language like C is very complex with regard to performing more advanced I/O tasks, which can be exasperating to work with.
However, a language like Python overcompensates this to the point where the programmer may not understand what they are doing anymore and everything just seems like pure magic.

GSL wants to counter this by making tasks which would normally take multiple classes and method calls,
and simplifying it to a level where the programmer still understands what they are doing, but without the complexity languages such as C.<br/>

### Customization
#### Macros
GSL brings back the customization of C, using macros. Not many languages use macros anymore and there is no reason why.
To ensure a proper experience for the programmer, macros are reintroduced.

#### Arrays
Why do languages print arrays like this: `[Object, Object, Object]`? GSL automatically calls the `toString` method on every element in the array.
This makes sure array elements are readable and will display anything the programmer wants them to display.
