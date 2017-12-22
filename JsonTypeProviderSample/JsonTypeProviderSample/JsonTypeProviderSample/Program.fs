open System
open FSharp.Data

[<Literal>]
let url = "https://api.github.com/users/gabrielschade"
type PerfilGitHub = JsonProvider<url>
[<EntryPoint>]
let main argv =
    let perfil = PerfilGitHub.Load url
    printfn "%s com %i seguidores" perfil.Name perfil.Followers
    printfn "%s" perfil.Bio
    printfn "Blog: %s" perfil.Blog

    Console.ReadKey() |> ignore
    0 // return an integer exit code