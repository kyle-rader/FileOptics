use std::io::{Read, Write};

use colored::{ColoredString, Colorize};

pub fn hex_print(input: impl Read, target: &mut impl Write, readable: bool) -> std::io::Result<()> {
    let mut scratch: String = String::with_capacity(128);

    for b in input.bytes() {
        let b = b?;
        let hex = color(b, format!("{:02x}", b).as_ref());

        write!(target, "{hex} ")?;

        // store readable data in scratch
        if readable {
            scratch.push(as_readable_char(b));
            scratch.push(' ');
            scratch.push(' ');
        }

        if b == b'\n' {
            writeln!(target)?;

            if readable {
                // Print the readable version of the line
                writeln!(target, "{scratch}")?;
                scratch.clear();
            }
        }
    }

    Ok(())
}

fn as_readable_char(b: u8) -> char {
    match b {
        b' ' | b'\n' => ' ',
        b'a'..=b'~' | b'!'..=b'^' => b as char,
        _ => '?',
    }
}

fn color(b: u8, s: impl Colorize) -> ColoredString {
    match b {
        b'\r' | b'\n' => s.color(colored::Color::Magenta),
        b' ' | b'\t' => s.color(colored::Color::Green),
        _ => s.normal(),
    }
}
