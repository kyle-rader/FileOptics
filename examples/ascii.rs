fn main() {
    for i in 0_u8..=127_u8 {
        println!("{i:000}: {}", i as char)
    }
}
