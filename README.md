# FileOptics

This is a command line interface (cli) for looking at files and their bytes.

![screenshot](img/screenshot-fileoptics.png)

## Installation

### From Source

This tool requires [Rust](https://rustup.rs/) to be installed.

```bash
git clone <repository-url>
cd file-optics
cargo build --release
```

The binary will be available at `target/release/file_optics`.

### Using Cargo

```bash
cargo install --path .
```

The tool will install and be accessible as `file_optics` on your command line.

## Usage

To get help run
```bash
file_optics -h
# or
file_optics --help
```
