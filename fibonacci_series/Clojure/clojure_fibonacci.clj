(defn fib
  "Prints the first `n` fibonacci numbers.
   Example:
   user> (fib 8)
   0N  1N  1N  2N  3N  5N  8N  13N  ;; => nil"
  [n]
  (loop [i 0
         f1 0N
         f2 1N]
    (when (< i n)
      (print f1 " ")
      (recur (inc i) f2 (+ f1 f2)))))

