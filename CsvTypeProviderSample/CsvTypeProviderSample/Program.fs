open FSharp.Data
open System

[<Literal>]
let caminho = "C:\\Users\\re035148\\Documents\\Custos.csv"
type PlanilhaCustos = CsvProvider<caminho,";">

[<EntryPoint>]
let main argv =
    let dados = PlanilhaCustos.Load caminho
    
    for linha in dados.Rows do
        printfn "|%s | %s | %i" linha.Descricao linha.Tipo linha.Jan
    
    Console.ReadKey()
    |> ignore

    0 // return an integer exit code
