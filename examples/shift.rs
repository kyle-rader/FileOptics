fn main() {
    let i = 1;
    let k = 256;
    for j in 0..=8 {
        println!("{} << {} = {}", i, j, i << j);
        println!("{} >> {} = {}", k, j, k >> j);
    }
}
