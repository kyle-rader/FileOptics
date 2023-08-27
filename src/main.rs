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
fn input(file: Option<String>) -> Result<Box<dyn Read>, String> {
    match file {
        Some(file) => File::open(file)
            .map(|f| Box::new(f) as Box<dyn Read>)
            .map_err(|e| e.to_string()),
        None => Ok(Box::new(std::io::stdin())),
    }
}

fn main() {
    let args = Args::parse();

    if let Some(ref file) = args.file {
        println!("Input: {:?}", file);
    } else {
        println!("Input: stdin");
    }

    match input(args.file) {
        Ok(input) => hex_print(input),
        Err(err) => eprint!("{:?}", err),
    }
}
