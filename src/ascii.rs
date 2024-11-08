use std::{
    io::{self, Write},
    ops::RangeInclusive,
};

use crate::show;

const SYMBOLS: [RangeInclusive<u8>; 4] = [33..=47, 58..=64, 91..=94, 123..=126];
const NUMBERS: RangeInclusive<u8> = 48..=57;
const UPPER_CASE: RangeInclusive<u8> = 65..=90;
const LOWER_CASE: RangeInclusive<u8> = 97..=122;

pub fn ascii_table(out: &mut impl Write) -> io::Result<()> {
    writeln!(out, "Symbols")?;
    print_range(out, SYMBOLS.into_iter().flatten())?;

    writeln!(out, "\n\nNumbers")?;
    print_range(out, NUMBERS)?;

    writeln!(out, "\n\nUpper Case")?;
    print_range(out, UPPER_CASE)?;

    writeln!(out, "\n\nLower Case")?;
    print_range(out, LOWER_CASE)?;

    Ok(())
}

fn print_range(out: &mut impl Write, range: impl Iterator<Item = u8>) -> io::Result<()> {
    let mut readable = String::from("char:");
    let mut base_10 = String::from("dec :");
    let mut base_16 = String::from("hex :");

    for b in range {
        readable.push_str(format!(" {}  ", b as char).as_str());
        base_10.push_str(format!("{b:-3} ").as_str());
        base_16.push_str(format!("{b:-3X} ").as_str());
    }
    writeln!(out, "{}", readable)?;
    writeln!(out, "{}", base_10)?;
    writeln!(out, "{}", base_16)?;

    Ok(())
}

pub fn chars(out: &mut impl Write, chars: String) -> io::Result<()> {
    show(chars.as_bytes(), out, true)
}
