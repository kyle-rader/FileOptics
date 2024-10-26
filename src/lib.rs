use std::io::{Read, Write};

use colored::Colorize;

pub fn hex_print(input: impl Read, target: &mut impl Write, readable: bool) -> std::io::Result<()> {
    for b in input.bytes() {
        if let Ok(b) = b {
            let output = format!("{:02x}", b);
            let output = match b {
                b'\r' | b'\n' => output.color(colored::Color::Magenta),
                b' ' | b'\t' => output.color(colored::Color::Green),
                _ => output.normal(),
            };

            write!(target, "{output} ")?;
            if b == b'\n' {
                writeln!(target)?;
            }
        }
    }

    Ok(())
}
