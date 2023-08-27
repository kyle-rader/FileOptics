use std::io::Read;

pub fn hex_print(input: Box<dyn Read>) {
    input.bytes().for_each(|b| {
        if let Ok(b) = b {
            print!("{:02x} ", b);
            if b == b'\n' {
                println!();
            }
        }
    });
}
