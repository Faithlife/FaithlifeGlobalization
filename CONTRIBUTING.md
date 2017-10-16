# Contributing to Faithlife.Globalization

## Prerequisites

* Install [Visual Studio 2017](https://www.visualstudio.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/) with the [editorconfig extension](https://github.com/editorconfig/editorconfig-vscode).
* Install [.NET Core 2.0](https://www.microsoft.com/net/core).

## Guidelines

* All new code **must** have complete unit tests.
* All public classes, methods, interfaces, enums, etc. **must** have correct XML documentation comments. After updating the XML documentation comments, run `.\build.ps1 -target=GenerateDocs` to generate updated documentation that can be committed with the code.
* Update [VersionHistory](VersionHistory.md) with a human-readable description of the change.

## How to Build

* Clone the repository: `git clone https://github.com/Faithlife/FaithlifeGlobalization.git`
* `cd FaithlifeGlobalization`
* `dotnet test tests/Faithlife.Globalization.Tests`