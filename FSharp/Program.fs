
// 1.
let fibonacci n =
    let rec helper n pp p =
        if n <= 0 then
            pp
        else
            helper (n - 1) p (pp + p)
    helper n 0 1

printfn "%d" <| fibonacci 10

// 2.
let invert lst =
    let rec helper xy z = 
        match xy with
        | x :: y -> helper y (x :: z)
        | []     -> z
    helper lst []

printfn "%A" (invert [1 .. 10] )

// 3.
let rec merge l r =
    match (l, r) with
    | [] , _ -> r
    | _ , [] -> l
    | (lh::lt), (rh::rt) -> 
        if lh < rh 
        then lh :: (merge lt r) 
        else rh :: (merge l rt)

let split = fun lst ->
    let len = List.length lst
    (List.take (len / 2) lst), (List.skip (len / 2) lst)

let rec mergesort (lst : list<'t>) =
    let (l, r) = split lst
    if l = [] then r else merge (mergesort l) (mergesort r)

printfn "%A" <| mergesort [2; 7; 5; 6; 4; 3; 0; 1; 12]

// 4.
type BinaryOperation =
    | Plus
    | Minus
    | Mult  
    | Div

type UnaryOperation =
    | Neg
    | Sqr

type Expression =
    | Num of double
    | BO of BinaryOperation * Expression * Expression
    | UO of UnaryOperation * Expression 

let rec eval (p: Expression) =
    let evalBinary (op: BinaryOperation) (x: double) (y: double) =
        match op with
        | Plus  -> x + y
        | Minus -> x - y
        | Mult  -> x * y
        | Div   -> x / y
    let evalUnary (op: UnaryOperation) (x: double)  =
        match op with
        | Neg -> - x
        | Sqr -> x * x
    match p with
    | Num(x) -> x 
    | BO (op, x, y) ->  evalBinary op (eval x) (eval y)
    | UO (op, x) -> evalUnary op (eval x)

// 2^2 + 1.34 * (-3.14)
printfn "%A" <| eval (BO(Plus, 
                         UO(Sqr, Num(2.0)),
                         BO(Mult, 
                            Num(1.34), 
                            UO(Neg, Num(3.14)))))

// 5.
let isPrime = fun index ->
    let rec check number dividor =
        if dividor >= number then true
        elif number % dividor = 0 then false
        else check number (dividor + 1)
    if index = 1 then false else check index 2

let primes = Seq.filter (fun x -> x > 0) <| Seq.initInfinite (fun i -> if isPrime i then i else 0)

Seq.take 25 primes |> Seq.iter (fun x -> printf "%d " x)

