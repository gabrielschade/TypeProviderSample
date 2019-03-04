open FSharp.Data
open XPlot.GoogleCharts

open System.IO
open System.Diagnostics

type ConfiguracoesGrafico = {
    CoresEixo : string array
    ValoresEixo : int array
    Descricao : string
}

[<Literal>]
let urlBase = 
    "http://api.openweathermap.org/data/2.5/forecast?appid=53340023da6269d559028b81bb474f37&units=metric&q="

type Clima = 
    JsonProvider<"http://api.openweathermap.org/data/2.5/forecast?appid=53340023da6269d559028b81bb474f37&units=metric&q=London,UK">
    

let escreverArquivoHtml html =
    File.AppendAllLines ("map.html",[html])

let obterGrafico configuracoes (valores: seq<string * float>) =
    let eixo = ColorAxis(values = configuracoes.ValoresEixo, colors = configuracoes.CoresEixo)
    let chart =
        valores
        |> Chart.Geo
        |> Chart.WithOptions(Options(colorAxis=eixo))
        |> Chart.WithLabel configuracoes.Descricao
    chart.GetHtml()

let obterIndicadorGlobal 
    (bancoDadosGlobal: WorldBankData.ServiceTypes.WorldBankDataService) 
    indicador =
        bancoDadosGlobal.Countries
        |> Seq.map( fun pais ->
                    pais.Name,
                    indicador pais
        )

let emissaoCO2Global 
    (bancoDadosGlobal: WorldBankData.ServiceTypes.WorldBankDataService) 
     ano = 
        obterIndicadorGlobal bancoDadosGlobal
         (fun pais -> pais.Indicators.``CO2 emissions (metric tons per capita)``.[ano])

let obterTemperatura local =
    try
        let clima = Clima.Load(urlBase + local)
        let amanha = Seq.head clima.List
        (float) amanha.Main.Temp
    with :? System.Net.WebException as ex ->
        0.0

let temperaturaGlobal 
    (bancoDadosGlobal: WorldBankData.ServiceTypes.WorldBankDataService) =
        obterIndicadorGlobal bancoDadosGlobal
         (fun pais -> obterTemperatura (pais.CapitalCity + "," + pais.Name))
        

[<EntryPoint>]
let main argv =
    let bancoDadosGlobal = WorldBankData.GetDataContext()
    let configuracoesCO2 = {
        CoresEixo = [| "#98f442";"#f1f441";"#efaf39";"#ef5a39" |]
        ValoresEixo = [| 0;+5;+10;+20 |]
        Descricao = "Emissão CO2"
    }

    emissaoCO2Global bancoDadosGlobal 2014
    |> obterGrafico configuracoesCO2  
    |> escreverArquivoHtml

    let configuracoesTemperatura ={
        CoresEixo = [| "#d8fffc";"#7badfc"; "#98f442";"#f1f441";"#ef5a39";"#ff3916" |]
        ValoresEixo = [| -20; 0;+15;+30;+40;+60 |]
        Descricao = "Temperatura"
    }

    temperaturaGlobal bancoDadosGlobal
    |> obterGrafico configuracoesTemperatura        
    |> escreverArquivoHtml
    
    Process.Start (@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", 
                    "file:\\" + Directory.GetCurrentDirectory() + "\\map.html")
    |> ignore

    
    0
