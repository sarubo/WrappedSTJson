namespace WrappedSTJson.Test

open Expecto
open WrappedSTJson
open WrappedSTJson.Test.Helper
open System.Text.Json

module TranslationExtraTest =
    let private jsonExample =
        $$"""{
    "FarProperty": {
        "foo": {
            "bar": {
                "baz": "OK!"
            }
        }
    },
    "FarProperties": [
        {
            "foo": {
                "bar": {
                    "baz": 2
                }
            }
        },
        {
            "foo": {
                "bar": {
                    "baz": 3
                }
            }
        },
        {
            "foo": {
                "bar": {
                    "baz": 5
                }
            }
        }
    ],
    "shallowArray": [ 2, 3, 5 ]
}
"""

    [<Tests>]
    let tests =
        testList "Check TranslationExtra" [
            testCaseId "JsonElmExtra.getMappedFarProperty: FarProperty.foo.bar.baz is OK!"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElmExtra.getMappedFarProperty JsonElm.getString [ "FarProperty"; "foo"; "bar"; "baz" ]

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.equal actual "OK!" message)

            testCaseId "JsonElmExtra.getFarProperty: FarProperty.foo.bar.baz is OK!"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElmExtra.getFarProperty [ "FarProperty"; "foo"; "bar"; "baz" ]
                    |> Option.bind JsonElm.getString

                Expect.isSome actual "JsonElm.getString is wrong."
                actual |> Option.iter (fun actual -> Expect.equal actual "OK!" message)

            testCaseId "JsonElmExtra.getMappedFarProperties: sum FarProperties[].foo.bar.baz is 10"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "FarProperties"
                    |> Option.bind JsonElm.getArrayEnumerator
                    |> Option.map (
                        JsonElmExtra.getMappedFarProperties JsonElm.getInt [ "foo"; "bar"; "baz" ]
                        >> Seq.sumBy (Option.defaultValue 0)
                    )

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.equal actual 10 message)

            testCaseId "JsonElmExtra.getFarProperties: sum FarProperties[].foo.bar.baz is 10"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElm.getProperty "FarProperties"
                    |> Option.bind JsonElm.getArrayEnumerator
                    |> Option.map (
                        JsonElmExtra.getFarProperties [ "foo"; "bar"; "baz" ]
                        >> Seq.sumBy (Option.bind JsonElm.getInt >> Option.defaultValue 0)
                    )

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.equal actual 10 message)

            testCaseId "JsonElmExtra.getMappedFarProperties: sum shallowArray[] is 10"
            <| fun message ->
                use doc = JsonDocument.Parse jsonExample

                let actual =
                    doc.RootElement
                    |> JsonElmExtra.getMappedFarProperty JsonElm.getArrayEnumerator [ "shallowArray" ]
                    |> Option.map (
                        JsonElmExtra.getMappedFarProperties JsonElm.getInt []
                        >> Seq.sumBy (Option.defaultValue 0)
                    )

                Expect.isSome actual message
                actual |> Option.iter (fun actual -> Expect.equal actual 10 message)
        ]
