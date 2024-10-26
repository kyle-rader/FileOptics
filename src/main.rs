use std::{fs::File, io::Read};

use clap::{self, Parser};
use file_optics::hex_print;

#[derive(Debug, Parser)]
struct Args {
    file: Option<String>,

    #[clap(short, long)]
    /// Print readable characters
    readable: bool,
}

/// Return an input source from either a file or stdin
fn input(file: Option<String>) -> Result<impl Read, std::io::Error> {
    match file {
        Some(file) => File::open(file).map(|f| Box::new(f) as Box<dyn Read>),
        None => Ok(Box::new(std::io::stdin())),
    }
}

fn main() -> anyhow::Result<()> {
    let args = Args::parse();

    if let Some(ref file) = args.file {
        println!("Input: {:?}", file);
    } else {
        println!("Input: stdin");
    }

    let input = input(args.file)?;

    Ok(hex_print(input, &mut std::io::stdout(), args.readable)?)
}
