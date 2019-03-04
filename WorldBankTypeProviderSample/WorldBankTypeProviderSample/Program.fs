open System
open FSharp.Data

[<EntryPoint>]
let main argv =
    let bancoDadosGlobal = WorldBankData.GetDataContext()
    let paises = [|
        bancoDadosGlobal.Countries.Brazil
        bancoDadosGlobal.Countries.Canada
        bancoDadosGlobal.Countries.Ireland
        bancoDadosGlobal.Countries.``United Kingdom``
        bancoDadosGlobal.Countries.``United States``
        bancoDadosGlobal.Countries.China
        bancoDadosGlobal.Countries.Japan
    |]

    printfn "País;Capital;População;Homens;Mulheres;0-14 anos;15-64 anos;acima"
    paises
    |> Array.iter ( fun pais -> 
        printfn "%s;%s;%.0f;%.0f;%.0f;%.0f;%.0f;%.0f" 
            pais.Name 
            pais.CapitalCity
            pais.Indicators.``Population, total``.[2015]
            pais.Indicators.``Population, male``.[2015]
            pais.Indicators.``Population, female``.[2015]
            pais.Indicators.``Population ages 0-14, total``.[2015]
            pais.Indicators.``Population ages 15-64, total``.[2015]
            pais.Indicators.``Population ages 65 and above, total``.[2015]
    )


    Console.ReadKey() |> ignore
    0
