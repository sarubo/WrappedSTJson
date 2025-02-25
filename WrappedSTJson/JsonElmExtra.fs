namespace WrappedSTJson

open System.Text.Json

module JsonElmExtra =
    let getMappedFarProperty: (JsonElement -> 'a option) -> string list -> JsonElement -> 'a option =
        fun binder fields elm ->
            let rec loop fields elmOpt =
                match fields with
                | field :: tail -> elmOpt |> Option.bind (JsonElm.getProperty field) |> loop tail
                | [] -> elmOpt |> Option.bind binder

            match fields with
            | field :: tail -> JsonElm.getProperty field elm |> loop tail
            | [] -> binder elm

    let getFarProperty: string list -> JsonElement -> JsonElement option =
        getMappedFarProperty Some

    let getMappedFarProperties
        : (JsonElement -> 'a option) -> string list -> JsonElement.ArrayEnumerator -> seq<'a option> =
        fun binder fields jsonArray -> jsonArray |> Seq.map (getFarProperty fields >> Option.bind binder)

    let getFarProperties: string list -> JsonElement.ArrayEnumerator -> seq<JsonElement option> =
        getMappedFarProperties Some
