fn main() {
    let utf8 = "Hello, World! 🎉";
    let utf16 = utf8
        .encode_utf16()
        .flat_map(|c| [c as u8, (c >> 8) as u8])
        .collect::<Vec<u8>>();

    let utf8 = utf8.as_bytes();

    println!("utf  8:");
    print_bytes(utf8);

    println!("utf 16:");
    print_bytes(&utf16);
}

// define function print_bytes that takes anthing that can be turned into an Iterator over u8
fn print_bytes<'a, I>(bytes: I)
where
    I: IntoIterator<Item = &'a u8>,
{
    // for each byte in bytes
    for byte in bytes {
        // print the byte as a hexidecimal number
        print!("{:02x} ", byte);
    }
    // print a newline
    println!();
}
