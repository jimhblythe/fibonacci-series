let rec fibonacci_rec (n : int) : int =
  match n with
  | 0 | 1 -> 0
  | 2 -> 1
  | n -> fibonacci_rec (n - 1) + fibonacci_rec (n - 2)

let fibonacci (n : int) : int list =
  if n <= 0 then []
  else List.init n (fun i -> fibonacci_rec (i + 1))

let () =
  print_endline "Enter the number of terms: ";
  let n =
    match read_line () |> String.trim with
    | "" -> 0
    | s -> (match int_of_string_opt s with Some i -> i | None -> 0)
  in
  let series = fibonacci n in
  List.iter (fun x -> print_endline (string_of_int x)) series
