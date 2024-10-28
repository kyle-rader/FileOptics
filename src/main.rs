use std::{fs::File, io::Read};

use clap::{self, Parser};
use file_optics::{ascii_table, chars, show};

#[derive(Debug, Parser)]
enum Cli {
    /// Show the bytes of a file or stdin
    Show(ShowArgs),

    /// Print an ASCII table
    Ascii(AsciiArgs),

    /// Show the byte value of a single character
    Char(CharArgs),
}

#[derive(Debug, Parser)]
struct ShowArgs {
    file: Option<String>,

    #[clap(short, long)]
    /// Print readable characters
    readable: bool,
}

#[derive(Debug, Parser)]
struct AsciiArgs {}

#[derive(Debug, Parser)]
struct CharArgs {
    /// The char(s) to show byte values of
    input: String,
}

/// Return an input source from either a file or stdin
fn input(file: Option<String>) -> Result<impl Read, std::io::Error> {
    match file {
        Some(file) => File::open(file).map(|f| Box::new(f) as Box<dyn Read>),
        None => Ok(Box::new(std::io::stdin())),
    }
}

fn main() -> anyhow::Result<()> {
    let args = Cli::parse();

    let out = &mut std::io::stdout();

    Ok(match args {
        Cli::Show(ShowArgs { file, readable }) => show(input(file)?, out, readable),
        Cli::Ascii(_) => ascii_table(out),
        Cli::Char(CharArgs { input }) => chars(out, input),
    }?)
}
