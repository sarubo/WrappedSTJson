namespace WrappedSTJson.Test

open Expecto
open WrappedSTJson
open WrappedSTJson.Test.Helper

module NullOptTest =
    [<Tests>]
    let tests =
        testList "Check NullOpt" [
            testCaseId "toOption: null is None"
            <| fun message ->
                let actual = NullOpt.toOption null
                Expect.isNone actual message

            testCaseId "toOption: string is Some"
            <| fun message ->
                let actual = NullOpt.toOption ""
                Expect.isSome actual message

            testCaseId "isNotNull: string is true"
            <| fun message ->
                let actual = NullOpt.isNotNull ""
                Expect.isTrue actual message

            testCaseId "isNotNull: null is false"
            <| fun message ->
                let actual = NullOpt.isNotNull null
                Expect.isFalse actual message

            testCaseId "toOptions: null sandwitch to None sandwich"
            <| fun message ->
                let actual = NullOpt.toOptions [| "one"; null; "two" |] |> Seq.toArray
                let expect = [| Some "one"; None; Some "two" |]
                Expect.equal actual expect message
            testCaseId "toOptions: every null to every None"
            <| fun message ->
                let actual: string option array =
                    NullOpt.toOptions [| null; null; null |] |> Seq.toArray

                let expect: string option array = [| None; None; None |]
                Expect.equal actual expect message

            testCaseId "toOptions: empty to empty"
            <| fun message ->
                let actual: string option array = NullOpt.toOptions [||] |> Seq.toArray
                Expect.isEmpty actual message

            testCaseId "toNonNulls: null sandwitch to bread"
            <| fun message ->
                let actual = NullOpt.toNonNulls [| "one"; null; "two" |] |> Seq.toArray
                let expect = [| "one"; "two" |]
                Expect.equal actual expect message

            testCaseId "toNonNulls: every null to empty"
            <| fun message ->
                let actual: string array = NullOpt.toNonNulls [| null; null; null |] |> Seq.toArray
                Expect.isEmpty actual message

            testCaseId "toNonNulls: empty to empty"
            <| fun message ->
                let actual: string array = NullOpt.toNonNulls [||] |> Seq.toArray
                Expect.isEmpty actual message

            testCaseId "toFiltered: null sandwitch to bread"
            <| fun message ->
                let actual = NullOpt.toFiltered [| Some "one"; None; Some "two" |] |> Seq.toArray
                let expect = [| "one"; "two" |]
                Expect.equal actual expect message

            testCaseId "toFiltered: every null to empty"
            <| fun message ->
                let actual: string array = NullOpt.toFiltered [| None; None; None |] |> Seq.toArray
                Expect.isEmpty actual message

            testCaseId "toFiltered: empty to empty"
            <| fun message ->
                let actual: string array = NullOpt.toFiltered [||] |> Seq.toArray
                Expect.isEmpty actual message

            testCaseId "filterMap: 0 or more is converted to a string"
            <| fun message ->
                let actual =
                    [| -2 .. 2 |]
                    |> NullOpt.filterMap (fun x -> if x >= 0 then Some(string x) else None)
                    |> Seq.toArray

                let expect = [| 0..2 |] |> Array.map string
                Expect.equal actual expect message

            testCaseId "filterMap: empty less than 0"
            <| fun message ->
                let actual =
                    [| -5 .. -2 |]
                    |> NullOpt.filterMap (fun x -> if x >= 0 then Some(string x) else None)
                    |> Seq.toArray

                Expect.isEmpty actual message
        ]
