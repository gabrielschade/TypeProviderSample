open FSharp.Data

[<Literal>]
let url = "https://en.wikipedia.org/wiki/List_of_Marvel_Cinematic_Universe_films"
type PaginaMarvelCinematicUniverse = HtmlProvider<url>

[<EntryPoint>]
let main argv = 
    let pagina = PaginaMarvelCinematicUniverse.Load url
    let filmes = pagina.Tables.``Critical response``.Rows

    let resultado =
        filmes
        |> Seq.filter (fun filme -> filme.Film <> "Average")
        |> Seq.map (fun filme -> filme.Film, filme.``Rotten Tomatoes``)
        |> Seq.sortByDescending( fun (filme,rotten) -> rotten)

    for (filme, rotten) in resultado do
        printfn "%s | %s" filme rotten

    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
