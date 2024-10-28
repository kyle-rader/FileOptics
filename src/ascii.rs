use std::io::{self, Write};

use crate::show;

pub fn ascii_table(out: &mut impl Write) -> io::Result<()> {
    for b in u8::MIN..=u8::MAX {
        writeln!(out, "{b:000} = {}", b as char)?;
    }
    Ok(())
}

pub fn chars(out: &mut impl Write, chars: String) -> io::Result<()> {
    show(chars.as_bytes(), out, true)
}
