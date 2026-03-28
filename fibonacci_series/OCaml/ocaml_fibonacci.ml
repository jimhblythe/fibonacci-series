let fibonacci (n : int) : int list =
  if n <= 0 then []
  else
    let rec loop i prev curr series =
      if i >= n then List.rev series
      else
        loop (i + 1) curr (prev + curr) (curr :: series)
    in
    loop 1 0 1 [0]

let () =
  print_endline "Enter the number of terms: ";
  let n =
    match read_line () |> String.trim with
    | "" -> 0
    | s -> (match int_of_string_opt s with Some i -> i | None -> 0)
  in
  let series = fibonacci n in
  List.iter (fun x -> print_endline (string_of_int x)) series
