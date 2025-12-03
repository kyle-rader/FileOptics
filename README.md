# FileOptics

This is a command line interface (cli) for looking at files and their bytes.

![screenshot](img/screenshot-fileoptics.png)

## Installation

### Rust (Recommended)

This tool requires [Rust](https://www.rust-lang.org/tools/install) to be installed.

Since this crate isn't published yet, install it from the local path:

```
cargo install --path .
```

The tool will install globally and be accessible as `file_optics` on your command line.

### .NET

This tool requires the [`dotnet` cli tool](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/install) (version >= `5.0.201`) be installed.

```
dotnet tool install -g FileOptics
```

The tool will install globally and be accessible as `fo` on your command line.

## Usage

To get help run
```
fo -h | --help
```
